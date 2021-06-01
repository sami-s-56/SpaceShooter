using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    private bool isGamePlaying = false;

    [SerializeField]
    private GameObject playerPrefab, uiManager;

    private int highScore;

    public delegate void GameDelegate();
    public static GameDelegate GameStarted;
    public static GameDelegate GameOver;
    public static GameDelegate GameEnded;
    public static GameDelegate GameReset;

    void Start()
    {
        GameStarted += StartGame;
        GameOver += OnGameOver;
        GameReset += StartGame;
        SetHighScore();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeReplayInputs()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameReset(); //GameResetDelegate
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Quit to Main Menu
            GameEnded(); //GameEnded Delegate Call
        }
    }

    private void StartGame()
    {
        GameObject.Instantiate(playerPrefab);
    }

    private void SetHighScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore");
        uiManager.GetComponent<UIManagerScript>().SetHighScore(highScore);
    }

    private void OnGameOver()
    {
        int currScore = GameObject.FindGameObjectWithTag("player").GetComponent<PlayerScript>().GetScore();
        if (currScore > highScore)
        {
            highScore = currScore;
            PlayerPrefs.SetInt("HighScore", highScore);
        }
    }
}
