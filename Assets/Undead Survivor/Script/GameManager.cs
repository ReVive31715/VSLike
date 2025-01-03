using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Game Control")]
    public bool isLive = true;
    public float gameTime;
    public float maxGameTime = 2 * 10f;

    [Header("Player Info")]
    public float health;
    public float maxHealth = 100;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 10, 20, 30, 50, 80, 130, 210, 340, 550, 890 };

    [Header("Object")]
    public Player player;
    public PoolManager pool;
    public LevelUp uiLevelUp;
    public Result uiResult;
    public GameObject EnemyCleaner;
    void Awake()
    {
        instance = this;
    }

    public void StartGame()
    {
        health = maxHealth;
        uiLevelUp.Select(2);        //Temp

        Resume();
    }
    public void EndGame()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;
        yield return new WaitForSeconds(0.5f);
        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        Stop();
    }
    public void WinGame()
    {
        StartCoroutine(GameClearRoutine());
    }

    IEnumerator GameClearRoutine()
    {
        isLive = false;
        EnemyCleaner.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        Stop();
    }
    public void RetryGame()
    {
        SceneManager.LoadScene(0);
    }
    void Update()
    {
        if (!isLive) 
        {
            return;
        }
        gameTime += Time.deltaTime;
        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
            WinGame();
        }
    }

    public void GetExp()
    {
        if (!isLive)
        {
            return;
        }
        exp++;

        if (exp == nextExp[Mathf.Min(level, nextExp.Length - 1)])
        {
            level++;
            exp = 0;
            uiLevelUp.Show();
        }
    }

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
    }
}