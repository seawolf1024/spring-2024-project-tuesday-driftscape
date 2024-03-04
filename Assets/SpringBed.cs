using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringBed : MonoBehaviour
{
    public float jumpForce = 10f;
    private float jumpCooldownTimer = 0f; // 跳跃冷却计时器
    private bool canJump = true; // 控制是否允许跳跃
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        jumpCooldownTimer += Time.deltaTime;
        // 如果计时器超过0.5秒，重置变量，允许跳跃
        if (jumpCooldownTimer >= 0.5f)
        {
            canJump = true;
            jumpCooldownTimer = 0f; // 重置计时器
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && canJump)
        {
            anim.SetTrigger("jump");
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            canJump = false; // 跳跃后立即禁止进一步跳跃
        }
    }
}
