using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 锟斤拷锟斤拷锟斤拷锟斤拷锟秸硷拷锟皆凤拷锟绞筹拷锟斤拷锟斤拷锟斤拷锟斤拷锟斤拷

public class PlayerController : MonoBehaviour
{
    public float jumpForce; // 锟斤拷跃锟斤拷锟斤拷
    public float speed; // 锟斤拷锟狡斤拷色锟狡讹拷锟劫讹拷
    public KeyCode jumpKey = KeyCode.Space; // 锟斤拷跃锟斤拷锟斤拷锟斤拷默锟斤拷为锟秸革拷锟�
    private Rigidbody2D rb2d;



    public float textspeed = 0.2f;
    public RectTransform hintransform;
    private Vector2 hintstartPosition;
    public bool isHint = false;


    private bool isGrounded; // 是否接触地面
    public float immobilizeTime; // 球体不能移动的时间

    private SpriteRenderer spriteRenderer;
    private bool isImmobilized = false;
    private bool isJump = true;

    private Color originalColor;

    public Transform enemy;

    private bool canMoveFreely = false; // 锟斤拷锟斤拷锟斤拷锟斤拷锟狡讹拷锟侥诧拷锟斤拷锟斤拷锟斤拷
    public float FreeFlytime;
    public GameObject success;
    public GameObject restart;
    public GameObject nextlevel;
    public string nextsceneName;

    // Cannon launch direction indicator
    public LineRenderer directionIndicator;
    public float launchForce;  // 锟斤拷锟狡凤拷锟斤拷锟劫度的诧拷锟斤拷

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

    public bool isCooldown = false; // 锟斤拷锟斤拷锟角凤拷锟斤拷锟斤拷却状态
    public float cooldownTime = 0.6f; // 锟斤拷锟斤拷锟斤拷锟饺词憋拷锟�


    private bool isPaused = false; 
    public GameObject pauseMenuUI;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // 锟斤拷取SpriteRenderer锟斤拷锟�
        originalColor = spriteRenderer.color; // 锟斤拷锟斤拷原始锟斤拷色
        success.SetActive(false);
        restart.SetActive(false);
        nextlevel.SetActive(false);
        pauseMenuUI.SetActive(false);
        hintstartPosition = hintransform.anchoredPosition;

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
        // 锟斤拷锟斤拷锟狡讹拷时锟斤拷锟斤拷锟斤拷锟斤拷锟斤拷锟狡讹拷
        float moveVertical = canMoveFreely ? Input.GetAxis("Vertical") : 0;
        if (Input.GetMouseButtonDown(1)) // 鼠标右键的索引是1
        {
            TogglePause();
        }
        if(isHint){
            hintransform.anchoredPosition += Vector2.right * textspeed * Time.deltaTime;
            if (hintransform.anchoredPosition.x > Screen.width + hintransform.rect.width)
            {
                isHint = false;
            }
        }

        if (canMoveFreely)
        {
            // 失去锟斤拷锟斤拷时锟斤拷锟斤拷锟斤拷锟狡讹拷
            Vector2 movement = new Vector2(moveHorizontal, moveVertical) * speed;
            Debug.Log("movement"+rb2d.velocity);
            rb2d.velocity = movement;
        }
        if (enemy != null && !isImmobilized)
        {
            if (Vector3.Distance(enemy.position, transform.position) < 0.8f)
            {   
                isImmobilized = true;
            }
        }
        if (!isImmobilized) // 锟斤拷锟矫伙拷斜锟斤拷锟阶�
        {
            if(!launch){
                Move();
            } 
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded && isJump)
            {
                Jump();
            }
            if (Vector3.Distance(CannonPlace.position, transform.position) < 0.5f)
            {
                UpdateDirectionIndicator();
                directionIndicator.enabled = true;

                // 锟斤拷锟斤拷锟斤拷锟戒方锟斤拷
                if (Input.GetKeyDown(KeyCode.W))
                {
                    launchDirection = RotateVector2(launchDirection, 5); // 锟斤拷时锟斤拷锟斤拷转
                    UpdateDirectionIndicator(); // 锟斤拷锟铰凤拷锟斤拷指示锟斤拷
                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    launchDirection = RotateVector2(launchDirection, -5); // 顺时锟斤拷锟斤拷转
                    UpdateDirectionIndicator(); // 锟斤拷锟铰凤拷锟斤拷指示锟斤拷
                }
                // 锟斤拷锟斤拷
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
                {
                    // 锟斤拷取 Rigidbody2D 锟斤拷锟�
                    if (rb2d != null)
                    {
                        // 应锟斤拷一锟斤拷锟斤拷锟斤拷锟斤拷锟斤拷锟斤拷锟斤拷锟斤拷锟矫筹拷锟劫讹拷
                        Debug.Log(launchDirection);
                        rb2d.AddForce(new Vector2(launchDirection.x, launchDirection.y) * launchForce, ForceMode2D.Impulse);
                        // Debug.Log(launchDirection);
                        Debug.Log("Velocity set to: " + rb2d.velocity);
                        launch = true;
                        StartCoroutine(EnableGravityAfterDelay(1.5f));

                    }
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
        // 锟斤拷锟斤拷锟斤拷锟斤拷锟経I锟斤拷示锟斤拷锟斤拷锟斤拷锟斤拷野锟斤拷锟斤拷锟紽锟斤拷锟斤拷锟斤拷锟斤拷锟铰硷拷锟截碉拷前锟斤拷锟斤拷
        if (restart.activeSelf && Input.GetKeyDown(KeyCode.F))
        {
            ReloadCurrentScene();
        }
        // 锟斤拷锟斤拷锟斤拷锟斤拷锟経I锟斤拷示锟斤拷锟斤拷锟斤拷锟斤拷野锟斤拷锟斤拷锟絆锟斤拷锟斤拷锟斤拷锟斤拷锟铰硷拷锟截碉拷前锟斤拷锟斤拷
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
    IEnumerator EnableGravityAfterDelay(float delay)
    {
        // 绛夊緟鎸囧畾鐨勫欢杩熸椂闂�
        yield return new WaitForSeconds(delay);
        // 灏唃ravityScale璁剧疆涓�1锛屼娇閲嶅姏鐢熸晥
        launch = false;
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
        isJump = true;
        Vector2 v = new Vector2(Physics2D.gravity.x, -Physics2D.gravity.y/9.8f);
        //Vector2 v = new Vector2(5, 10);
        rb2d.AddForce(v * jumpForce, ForceMode2D.Impulse);

    }
    void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(moveHorizontal * speed, rb2d.velocity.y);
        rb2d.velocity = movement;
        Debug.Log("move"+rb2d.velocity);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Trap") && !isCooldown) // 锟斤拷锟斤拷欠锟斤拷锟阶诧拷锟絫rap锟截帮拷
        {
            StartCoroutine(Immobilize(immobilizeTime, other.gameObject));
            StartCoroutine(Cooldown());
        }
        if (other.gameObject.CompareTag("Ground"))
        {

            isGrounded = true; // 接触地面时更新地面状态
            isJump = true;

        }
        if (other.gameObject.CompareTag("Goal")) // 锟斤拷锟斤拷欠锟斤拷锟阶诧拷锟紾oal
        {
            spriteRenderer.color = Color.green; // 锟斤拷锟斤拷锟斤拷锟斤拷色锟斤拷为锟斤拷色
            Time.timeScale = 0; // 锟斤拷止锟斤拷锟斤拷
            success.SetActive(true); 
            nextlevel.SetActive(true); 
        }

        if (other.gameObject.tag == "SpringBed") // Do not jump when player using spring bed tool
        {
            isJump = false;
        }

    }
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = false; // 锟诫开锟斤拷锟斤拷时锟斤拷锟铰碉拷锟斤拷状态
        }
        if (other.gameObject.CompareTag("Goal")) // 锟斤拷锟斤拷欠锟斤拷锟阶诧拷锟紾oal
        {
            spriteRenderer.color = originalColor; // 锟斤拷锟斤拷锟斤拷锟斤拷色锟斤拷为原锟斤拷锟斤拷色
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
        isHint = true;
        yield return new WaitForSeconds(duration); // 等待指定时间
        rb2d.gravityScale = 1; // 恢复重力
        canMoveFreely = false; // 恢复正常移动限制

    }
    IEnumerator Immobilize(float time, GameObject other)
    {
        isImmobilized = true;
        rb2d.velocity = Vector2.zero;
         Debug.Log("Immobilize"+rb2d.velocity);
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(time);
        isImmobilized = false;
        spriteRenderer.color = originalColor;
    }
    IEnumerator Cooldown()
    {
        isCooldown = true; // 锟斤拷始锟斤拷却
        yield return new WaitForSeconds(cooldownTime); // 锟饺达拷锟斤拷却时锟斤拷
        isCooldown = false; // 锟斤拷锟斤拷锟斤拷却
    }
    void ReloadCurrentScene()
    {
        Time.timeScale = 1; // 锟斤拷锟斤拷锟剿讹拷
        int sceneIndex = SceneManager.GetActiveScene().buildIndex; // 锟斤拷取锟斤拷前锟斤拷锟斤拷锟斤拷锟斤拷锟斤拷
        SceneManager.LoadScene(sceneIndex); // 锟斤拷锟斤拷锟斤拷锟斤拷锟斤拷锟铰硷拷锟截筹拷锟斤拷
        possession = 0;

        Vector2 originalGravity = Physics2D.gravity;
        Physics2D.gravity = new Vector2(0, -9.81f);
    }
    void ReloadNextScene()
    {
        Time.timeScale = 1; // 锟斤拷锟斤拷锟剿讹拷
        SceneManager.LoadScene(nextsceneName); // 锟斤拷锟斤拷指锟斤拷锟斤拷锟斤拷
        possession = 0;
        Vector2 originalGravity = Physics2D.gravity;
        Physics2D.gravity = new Vector2(0, -9.81f);
    }
    void TogglePause()
    {
        isPaused = !isPaused;

        Time.timeScale = isPaused ? 0 : 1;

        if (isPaused)
        {
            pauseMenuUI.SetActive(true);
        }
        else
        {
            pauseMenuUI.SetActive(false);
        }
    }

}
