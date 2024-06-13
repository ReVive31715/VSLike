using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs; //프리팹 보관 변수

    List<GameObject>[] pools; //풀 담당 리스트
    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for (int index = 0; index < pools.Length; index++) {
            pools[index] = new List<GameObject>(); // pools 모두 초기와
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        foreach (GameObject item in pools[index]) {
            if (!item.activeSelf) {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        if (select == null) {
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }
}
