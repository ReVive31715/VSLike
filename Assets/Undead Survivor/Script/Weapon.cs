using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    public Vector2 inputVec;
    float timer;
    Player player;

    void Awake()
    {
        player = GameManager.instance.player;
    }

    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }
    void Update()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }

        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);

                break;
            default:
                timer += Time.deltaTime;

                if (timer > speed)
                {
                    timer = 0f;
                    switch (id)
                    {
                        case 1:
                            Fire();
                            break;
                        case 2: 
                            Slash();
                            break;
                    }
                }
                break;

        }
    }

    public void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        if (id == 0)
        {
            Place();
        }

        player.BroadcastMessage("ApplyEquip", SendMessageOptions.DontRequireReceiver);
    }
    public void Init(ItemData data)
    {
        name = "Weapon " + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        id = data.itemId;
        damage = data.baseDamage;
        count = data.baseCount;
        
        for (int i = 0; i < GameManager.instance.pool.prefabs.Length; i++)
        {
            if (data.projectile == GameManager.instance.pool.prefabs[i])
            {
                prefabId = i;
                break;
            }
        }

        switch (id)
        {
            case 0:
                speed = 150;
                Place();

                break;
            case 1:
                speed = 1.5f;
                break;
            case 2:
                speed = 1.7f;
                break;
        }

        player.BroadcastMessage("ApplyEquip", SendMessageOptions.DontRequireReceiver);
    }

    void Place()
    {
        for (int index = 0; index < count; index++)
        {
            Transform bullet;

            if (index < transform.childCount)
            {
                bullet = transform.GetChild(index);
            }
            else
            {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform;
            }

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotationVector = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotationVector);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero); //-1Àº ¹«ÇÑ.
        }
    }

    void Fire()
    {
        if (!player.scanner.nearestTarget)
        {
            return; 
        }

        switch (id)
        {
            case 1:
                Vector3 targetPos = player.scanner.nearestTarget.position;
                Vector3 dir = targetPos - transform.position;
                dir = dir.normalized;

                Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.position = transform.position;
                bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
                bullet.GetComponent<Bullet>().Init(damage, count, dir);

                break;
        }
    }
    void Slash()
    {
        Vector3 dir = Vector3.zero;
        if (player.GetComponent<SpriteRenderer>().flipX == false)
        {
            dir = Vector3.right;
        }
        else
        {
            dir = Vector3.left;
        }
        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.parent = transform;
        bullet.localPosition = Vector3.zero;
        bullet.localRotation = Quaternion.identity;

        bullet.rotation = Quaternion.LookRotation(Vector3.forward, dir);
        bullet.Translate(bullet.up * 1.5f, Space.World);

        bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero);
        StartCoroutine(DisableSlash(bullet.gameObject));

    }

    IEnumerator DisableSlash(GameObject bullet)
    {
        yield return new WaitForSeconds(0.1f);
        bullet.SetActive(false);
    }
}
