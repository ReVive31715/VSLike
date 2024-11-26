using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int penetration;

    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int penetration, Vector3 dir)
    {
        this.damage = damage;
        this.penetration = penetration;

        if (penetration > -1) 
        {
            rigid.velocity = dir * 7;
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || penetration == -1)
        {
            return;
        }

        penetration--;

        if (penetration == -1)
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Area"))
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }
}
