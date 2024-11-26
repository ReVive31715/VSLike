using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
    public Scanner scanner;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator animator;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
    }
    void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }

        Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }
    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }

    void LateUpdate()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }

        animator.SetFloat("Speed", inputVec.magnitude);

        if (inputVec.x != 0) {
            spriteRenderer.flipX = inputVec.x < 0;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }

        GameManager.instance.health -= Time.deltaTime * 10;

        if (GameManager.instance.health < 0)
        {
            for (int i = 2; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            animator.SetTrigger("Dead");
            GameManager.instance.EndGame();
        }
    }
}
