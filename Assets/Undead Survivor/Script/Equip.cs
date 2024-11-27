using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equip : MonoBehaviour
{
    public ItemData.ItemType type;
    public float rate;

    public void Init(ItemData data)
    {
        name = "Equipment " + data.itemId;
        transform.parent = GameManager.instance.player.transform;
        transform.localPosition = Vector3.zero;

        type = data.itemType;
        rate = data.damages[0];
        ApplyEquip();
    }

    public void LevelUp(float rate)
    {
        this.rate = rate;
        ApplyEquip();
    }

    void ApplyEquip()
    {
        switch(type)
        {
            case ItemData.ItemType.Glove:
                RateUp(); 
                break;
            case ItemData.ItemType.Shoe: 
                SpeedUp();
                break;
        }
    }
    void RateUp()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach(Weapon weapon in weapons)
        {
            switch(weapon.id)
            {
                case 0:
                    weapon.speed = 150 + (150 * rate); 
                    break;
                case 1:
                    weapon.speed = 1.5f * (1f - rate);
                    break;
                case 2:
                    weapon.speed = 1.7f * (1f - rate);
                    break;
            }
        }
    }

    void SpeedUp()
    {
        float speed = 3;
        GameManager.instance.player.speed = speed + (speed * rate);
    }
}
