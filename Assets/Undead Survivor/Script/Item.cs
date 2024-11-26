using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemData data;
    public int level;
    public Weapon weapon;
    public Equip equip;

    Image icon;
    Text textLevel;
    Text textName;
    Text textDescription;

    private void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.itemIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0];
        textName = texts[1];
        textDescription = texts[2];

        textName.text = data.itemName;
    }
    void OnEnable()
    {
        textLevel.text = "LV. " + level;
        switch (data.itemType)
        {
            case ItemData.ItemType.Book:
            case ItemData.ItemType.Range:
            case ItemData.ItemType.Pencil:
                textDescription.text = string.Format(data.itemDescription, (data.damages[level] * data.baseDamage), data.counts[level]);
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                textDescription.text = string.Format(data.itemDescription, data.damages[level] * 100);
                break;
            default:
                textDescription.text = string.Format(data.itemDescription);
                break;

        }
    }

    public void OnClick()
    {
        switch(data.itemType)
        {
            case ItemData.ItemType.Book:
            case ItemData.ItemType.Range:
                if (level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(data);
                }
                else
                {
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;

                    nextDamage += data.baseDamage * data.damages[level];
                    nextCount += data.counts[level];

                    weapon.LevelUp(nextDamage, nextCount);
                }
                level++;
                break;

            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                if (level == 0)
                {
                    GameObject newEquip = new GameObject();
                    equip = newEquip.AddComponent<Equip>();
                    equip.Init(data);
                }
                else
                {
                    float nextRate = data.damages[level];
                    equip.levelUp(nextRate);
                }
                level++;
                break;

            case ItemData.ItemType.Heal:
                GameManager.instance.health = GameManager.instance.maxHealth;
                break;
        }

        if (level == data.damages.Length)
        {
            GetComponent<Button>().interactable = false;
        }
    }
}
