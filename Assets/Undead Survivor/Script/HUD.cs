using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType { Exp, Level, Kill, Time, Health }
    public InfoType type;

    Text Text;
    Slider GameSlider;

    private void Awake()
    {
        Text = GetComponent<Text>();
        GameSlider = GetComponent<Slider>();
    }

    private void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Exp:
                float curExp = GameManager.instance.exp;
                float maxExp = GameManager.instance.nextExp[GameManager.instance.level];
                GameSlider.value = curExp / maxExp;
                break;
            case InfoType.Level:
                Text.text = string.Format("Lv. {0:F0} ", GameManager.instance.level);
                break;
            case InfoType.Kill:
                Text.text = string.Format("{0:F0}", GameManager.instance.kill);
                break;
            case InfoType.Time:
                float remainingTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;
                int min = Mathf.FloorToInt(remainingTime / 60);
                int sec = Mathf.FloorToInt(remainingTime % 60);
                Text.text = string.Format("{0:D2}:{1:D2}", min, sec);
                break;
            case InfoType.Health:
                float curHealth = GameManager.instance.health;
                float maxHealth = GameManager.instance.maxHealth;
                GameSlider.value = curHealth / maxHealth;
                break;
        }
    }
}
