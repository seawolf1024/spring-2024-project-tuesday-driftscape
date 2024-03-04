using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringBed : MonoBehaviour
{
    public float jumpForce = 10f;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // 获取玩家的位置和蹦床的位置
            Vector2 playerPosition = collision.gameObject.transform.position;
            Vector2 springBedPosition = transform.position;

            if (playerPosition.y > springBedPosition.y + 0.1f)
            {
                anim.SetTrigger("jump");
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
