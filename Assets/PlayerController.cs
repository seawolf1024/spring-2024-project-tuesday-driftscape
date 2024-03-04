using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 引入命名空间以访问场景管理功能

public class PlayerController : MonoBehaviour
{
    public float jumpForce; // 跳跃力度
    public float speed; // 控制角色移动速度
    public KeyCode jumpKey = KeyCode.Space; // 跳跃按键，默认为空格键
    private Rigidbody2D rb2d;

    private bool isGrounded; // 是否接触地面
    public float immobilizeTime; // 球体不能移动的时间
    private SpriteRenderer spriteRenderer;
    private bool isImmobilized = false;

    private Color originalColor;

    public Transform enemy;

    private bool canMoveFreely = false; // 控制自由移动的布尔变量
    public float FreeFlytime;
    public GameObject success;
    public GameObject restart;
    public GameObject nextlevel;
    public string nextsceneName;

    // Cannon launch direction indicator
    public LineRenderer directionIndicator;
    public float launchForce;  // 控制发射速度的参数

    public Transform CannonPlace;
    
    public Vector2 launchDirection = Vector2.right;
    public float forceMagnitude = 1000f;
    public bool launch = false;

    // gravity tool
    public Transform gravityTool;
    public int possession = 0;
    public GameObject gtool;

    // nisemono navmesh
    public Navigation navi;
    public Transform fbuild;
    public GameObject fgoal;
    public bool haveftool = false;

    public bool isCooldown = false; // 陷阱是否处于冷却状态
    public float cooldownTime = 0.6f; // 陷阱的冷却时间

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // 获取SpriteRenderer组件
        originalColor = spriteRenderer.color; // 保存原始颜色
        success.SetActive(false);
        restart.SetActive(false);
        nextlevel.SetActive(false);
    }

    void UpdateDirectionIndicator()
    {
        if (directionIndicator != null)
        {
            directionIndicator.SetPosition(0, transform.position);
            directionIndicator.SetPosition(1, transform.position + new Vector3(launchDirection.x, launchDirection.y, 0) * 6);
        }
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        // 自由移动时，允许上下移动
        float moveVertical = canMoveFreely ? Input.GetAxis("Vertical") : 0;
        if (canMoveFreely)
        {
            // 失去重力时的自由移动
            Vector2 movement = new Vector2(moveHorizontal, moveVertical) * speed;
            rb2d.velocity = movement;
        }
        if (enemy != null && !isImmobilized)
        {
            if (Vector3.Distance(enemy.position, transform.position) < 0.8f)
            {   
                isImmobilized = true;
            }
        }
        if (!isImmobilized) // 如果没有被定住
        {
            Move();
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                Jump();
            }
            if (Vector3.Distance(CannonPlace.position, transform.position) < 0.5f)
            {
                UpdateDirectionIndicator();
                directionIndicator.enabled = true;

                // 调整发射方向
                if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
                {
                    launchDirection = RotateVector2(launchDirection, 5); // 逆时针旋转
                    UpdateDirectionIndicator(); // 更新方向指示器
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
                {
                    launchDirection = RotateVector2(launchDirection, -5); // 顺时针旋转
                    UpdateDirectionIndicator(); // 更新方向指示器
                }
                // 发射
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
                {
                    // 获取 Rigidbody2D 组件
                    Rigidbody2D rb = GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        // 应用一个冲量，而不是设置初速度
                        rb.AddForce(new Vector2(launchDirection.x, launchDirection.y) * launchForce, ForceMode2D.Impulse);
                    }
                    launch = true;
                }
            }
            else
            {
                directionIndicator.enabled = false;
            }
            if (Vector2.Distance(fbuild.position, transform.position) < 0.5f)
            {
                haveftool = true;
                fgoal.SetActive(false);
            }
            if (Vector2.Distance(gravityTool.position, transform.position) < 0.5f)
            {
                possession += 1;
                gtool.SetActive(false);

            }
            if (possession > 0)
            {
                if (Input.GetKeyDown(KeyCode.G))
                {
                    possession -= 1;
                    Gupside();
                }

            }
            if (haveftool)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    fgoal.SetActive(true);
                    navi.getconfused = true;
                    StartCoroutine(FakeGoal(3f));
                    haveftool = false;
                }
            }
        }
        // 如果重启的UI显示，并且玩家按下了F键，则重新加载当前场景
        if (restart.activeSelf && Input.GetKeyDown(KeyCode.F))
        {
            ReloadCurrentScene();
        }
        // 如果重启的UI显示，并且玩家按下了O键，则重新加载当前场景
        if (nextlevel.activeSelf && Input.GetKeyDown(KeyCode.O))
        {
            ReloadNextScene();
        }
        // Check if the ESC key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Home");
        }
    }
    IEnumerator FakeGoal(float duration)
    {
        Debug.Log("fuse");
        navi.agent.SetDestination(fbuild.position);
        Debug.Log(navi.agent.destination);
        yield return new WaitForSeconds(duration);
        navi.getconfused = false;
        Debug.Log(navi.agent.destination);
    }
    void Gupside()
    {
        Physics2D.gravity = new Vector2(Physics2D.gravity.x, -Physics2D.gravity.y);
    }
    void Jump()
    {
        Vector2 v = new Vector2(Physics2D.gravity.x, -Physics2D.gravity.y/9.8f);
        rb2d.AddForce(v * jumpForce, ForceMode2D.Impulse);
    }
    void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(moveHorizontal * speed, rb2d.velocity.y);
        rb2d.velocity = movement;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Trap") && !isCooldown) // 检测是否碰撞到trap地板
        {
            StartCoroutine(Immobilize(immobilizeTime, other.gameObject));
            StartCoroutine(Cooldown());
        }
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // 接触地面时更新地面状态
        }
        if (other.gameObject.CompareTag("Goal")) // 检测是否碰撞到Goal
        {
            spriteRenderer.color = Color.green; // 将球体颜色改为绿色
            Time.timeScale = 0; // 静止场景
            success.SetActive(true); 
            nextlevel.SetActive(true); 
        }

    }
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = false; // 离开地面时更新地面状态
        }
        if (other.gameObject.CompareTag("Goal")) // 检测是否碰撞到Goal
        {
            spriteRenderer.color = originalColor; // 将球体颜色改为原本颜色
        }
    }
    Vector2 RotateVector2(Vector2 v, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);
        return new Vector2(cos * v.x - sin * v.y, sin * v.x + cos * v.y);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Tool"))
        {
            StartCoroutine(TemporaryLoseGravity(FreeFlytime));
        }
    }
    IEnumerator TemporaryLoseGravity(float duration)
    {
        rb2d.gravityScale = 0; // 玩家失去重力
        canMoveFreely = true; // 允许玩家自由移动
        yield return new WaitForSeconds(duration); // 等待指定时间
        rb2d.gravityScale = 1; // 恢复重力
        canMoveFreely = false; // 恢复正常移动限制
    }
    IEnumerator Immobilize(float time, GameObject other)
    {
        isImmobilized = true;
        rb2d.velocity = Vector2.zero;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(time);
        isImmobilized = false;
        spriteRenderer.color = originalColor;
    }
    IEnumerator Cooldown()
    {
        isCooldown = true; // 开始冷却
        yield return new WaitForSeconds(cooldownTime); // 等待冷却时间
        isCooldown = false; // 结束冷却
    }
    void ReloadCurrentScene()
    {
        Time.timeScale = 1; // 场景运动
        int sceneIndex = SceneManager.GetActiveScene().buildIndex; // 获取当前场景的索引
        SceneManager.LoadScene(sceneIndex); // 根据索引重新加载场景
        possession = 0;

        Vector2 originalGravity = Physics2D.gravity;
        Physics2D.gravity = new Vector2(0, -9.81f);
    }
    void ReloadNextScene()
    {
        Time.timeScale = 1; // 场景运动
        SceneManager.LoadScene(nextsceneName); // 加载指定场景
        possession = 0;
        Vector2 originalGravity = Physics2D.gravity;
        Physics2D.gravity = new Vector2(0, -9.81f);
    }
}
