using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SurvivalMode : TimeMode
{
    private double timeBonus = 15;
    private float puzzleCount = 0;
    [SerializeField] private GameObject TimeExtension;
    private TextMeshProUGUI extensionText;
    private float fadeTime;
    private bool isFading;
   


    protected override void Start()
    {
        base.Start();

        //gets the high score for Survival Mode, sets it to 0 if it doesn't exist and displays it
        PlayerPrefs.GetInt("SurvivalHighScore", 0);
        ShowHighScore("SurvivalHighScore");

        extensionText = TimeExtension.transform.GetComponent<TextMeshProUGUI>();
        fadeTime = 1;
        isFading = false;
        
    }

    protected override void Update()
    {
        base.Update();
        if(isFading)
        {
            FadeExtensionText();
        }
    }

    //adds time to the clock during a game once a puzzle is completed
    public void AddTime()
    {
        //reduces the amount of extra time is added for completing puzzles over time until 5 puzzles have been completed when the bonus is only 5 seconds from 15
        if (puzzleCount < 5)
        {
            timeBonus -= puzzleCount;
            puzzleCount += 1;
        }

        Timer.addTime(timeBonus);
        ShowTextExtension();
    }

    public override void RestartGame()
    {
        base.RestartGame();
        ShowHighScore("SurvivalHighScore");
    }

    //shows how much time has been added next to the clock
    private void ShowTextExtension()
    {
        TimeExtension.SetActive(true);
        extensionText.text = "+" + timeBonus.ToString();
        extensionText.color = new Color(1, 1, 1, 1);

        //starts fading out the time extension text
        isFading = true;
        
    }

    //fades out the time extension text
    private void FadeExtensionText()
    {
        if(fadeTime > 0)
        {
            //fades out the text in two seconds
            fadeTime -= Time.deltaTime/2;
            extensionText.color = new Color(1, 1, 1, fadeTime);
        }

        //sets the values at the end of the fade time so the alpha channel is exactly 0
        else
        {
            extensionText.color = new Color(1, 1, 1, 0);
            fadeTime = 1;
            isFading = false;
            TimeExtension.SetActive(false);
        }
    }
}
