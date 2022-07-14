using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeMode : GameMode
{
    protected CountdownMode Timer;
    protected bool GameIsOver;

    //an invisbile 3d object that when activiated overlays the dials so they can be touched and rotated
    //[SerializeField] private GameObject DialBlocker;
    [SerializeField] private GameObject GameOverScreen;
    [SerializeField] private TextMeshProUGUI EndScore;
    
    protected override void Start()
    {
        base.Start();
        GameIsOver = false;
        Timer = transform.GetComponent<CountdownMode>();

        //gets the high score forTimed Mode, sets it to 0 if it doesn't exist and displays it
        PlayerPrefs.GetInt("TimedHighScore", 0);
        ShowHighScore("TimedHighScore");
    }

    protected override void Update()
    {
        base.Update();

        //bool is used to check the time left and ends the game once the time runs out
        if (!GameIsOver)
        {
            if (Timer.getTimeLeft() <= 0)
            {
                EndGame();

            }
        }
    }

    public override void RestartGame()
    {
        base.RestartGame();
        ShowHighScore("TimedHighScore");
    }

    //executed once the time has run out
    private void EndGame()
    {
        //stops the player from turning the dials
        DialBlocker.SetActive(true);

        //shows the game over screen
        GameOverScreen.SetActive(true);
        EndScore.text = Score.ToString();

        //plays the alarm sound
        Timer.PlaySound(Timer.getAlarmClip());

        //stops checking if their is no time left so this function doesn't constantly excute every frame
        GameIsOver = true;
    }

    //resets the bool so the time remaining is checked
    public void GameIsNotOver()
    {
        GameIsOver = false;
    }

    //adds points to the scoreboard
    public void AddScore(int s, string mode)
    {
        Score += s;
        ScoreText.text = Score.ToString();

        //checks the mode and changes the high score if it has been broken for the current mode
        if (mode == "Timed")
        {
            if (PlayerPrefs.GetInt("TimedHighScore") < Score)
            {
                PlayerPrefs.SetInt("TimedHighScore", Score);
                ShowHighScore("TimedHighScore");
            }
        }

        else
        {
            if (PlayerPrefs.GetInt("SurvivalHighScore") < Score)
            {
                PlayerPrefs.SetInt("SurvivalHighScore", Score);
                ShowHighScore("SurvivalHighScore");
            }
        }
    }

    //changes the text to display the high score
    protected virtual void ShowHighScore(string highScoreType)
    {
        HighScoreText.text = PlayerPrefs.GetInt(highScoreType, 0).ToString();
    }


}
