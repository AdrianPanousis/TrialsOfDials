using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartGame : MonoBehaviour
{
    ModeMenu ModeMenu;
    GameMode ZenGame;
    TimeMode TimeGame;
    SurvivalMode SurvivalGame;

    //gathers all the game mode scripts and the mode menu that controls mode selection
    void Start()
    {
        ModeMenu = transform.GetComponent<ModeMenu>();
        ZenGame = transform.GetComponent<GameMode>();
        TimeGame = transform.GetComponent<TimeMode>();
        SurvivalGame = transform.GetComponent<SurvivalMode>();
    }

    public void Restart()
    {
        //checks which game mode is currently being played and restarts the game
        switch (ModeMenu.GetCurrentMode())
        {
            case "Zen":
                ZenGame.RestartGame();
                break;
            case "Timed":
                TimeGame.RestartGame();
                TimeGame.GameIsNotOver();
                break;
            case "Survival":
                SurvivalGame.RestartGame();
                SurvivalGame.GameIsNotOver();
                break;
        }
    }
}
