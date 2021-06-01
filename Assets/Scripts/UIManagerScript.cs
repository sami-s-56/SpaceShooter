using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerScript : MonoBehaviour
{
    [SerializeField]
    private Text scoreText, gameOverText, highScoreText;

    [SerializeField]
    private List<Sprite> lifeSprites;

    [SerializeField]
    private Image lifeImage;

    [SerializeField]
    private GameObject mainmenuPanel, hudPanel, endGamePanel, gameManager;

    private bool canReplay = false;

    private void Start()
    {
        //Delegates
        GameManagerScript.GameStarted += StartGame;
        GameManagerScript.GameReset += ResetUI;
        GameManagerScript.GameEnded += EndGame;
        GameManagerScript.GameOver += GameOver;

        //Take highscore from game manager
        //Set the Text in main menu
    }

    private void Update()
    {
        if (canReplay)
        {
            gameManager.GetComponent<GameManagerScript>().TakeReplayInputs();//Replace with on destroyed delegate
        }
    }

    public void UpdateScore(int Score)
    {
        scoreText.text = "Score:" + Score;
    }

    public void UpdateLives(int livesLeft)
    {
        if(livesLeft >= 0)
        {
            lifeImage.sprite = lifeSprites[livesLeft];
        }
    }

    private void StartGame()
    {
        mainmenuPanel.SetActive(false);
        hudPanel.SetActive(true);
        UpdateScore(0);
        UpdateLives(3);
    }

    private void ResetUI()
    {
        canReplay = false;
        endGamePanel.SetActive(false);
        UpdateScore(0);
        UpdateLives(3);
        hudPanel.SetActive(true);
    }

    private void GameOver()
    {
        hudPanel.SetActive(false);
        endGamePanel.SetActive(true);
        canReplay = true;
    }

    //StartButton Function
    public void OnStart()
    {
        GameManagerScript.GameStarted();
    }

    //QuitButtonFunction
    public void OnQuit()
    {
        Application.Quit();
    }

    private void EndGame()
    {
        canReplay = false;
        mainmenuPanel.SetActive(true);
        endGamePanel.SetActive(false);
    }

    public void SetHighScore(int score)
    {
        highScoreText.text = "HIGH SCORE: " + score.ToString();
    }
}
