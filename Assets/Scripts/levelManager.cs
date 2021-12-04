using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class levelManager : MonoBehaviour
{
    public static levelManager LM;
    [SerializeField] GameObject mGameOver;
    [Header("Win Panel")]
    [SerializeField] GameObject mWinPanel;
    [SerializeField] Text timeText;
    [SerializeField] Text recordText;

    void Start()
    {
        LM = this;
        mGameOver.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        mGameOver.SetActive(true);
    }

    public void WinGame(string time, string record)
    {
        Time.timeScale = 0;
        timeText.text = "Time: " + time;
        recordText.text = "Record: " + record;
        mWinPanel.SetActive(true);
    }

    public void Restart()
    {
        Application.LoadLevel(0);
        mGameOver.SetActive(false);
        Time.timeScale = 1;
    }

}
