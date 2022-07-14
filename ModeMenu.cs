using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeMenu : MonoBehaviour
{
    //stores all of the scripts of the game modes or ones that effect them
    [SerializeField] private GameMode Zen;
    [SerializeField] private TimeMode Time;
    [SerializeField] private SurvivalMode Survival;
    [SerializeField] private CountdownMode Countdown;
    [SerializeField] private RestartGame Restart;

    //stores all the of UI elements that will be changed when modes are changed
    [SerializeField] private GameObject ModeSelect;
    [SerializeField] private GameObject TitleScreen;
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject UpperBarZen;
    [SerializeField] private GameObject UpperBar;
    [SerializeField] private GameObject LowerBarZen;
    [SerializeField] private GameObject LowerBar;

    //stores all of the dials either in game or at the starting screen
    [SerializeField] private GameObject OpeningDials;
    [SerializeField] private GameObject DialStorage;

    //stores which mode is currently being played
    private string currentMode;

    //stors the music in the opening screen
    [SerializeField ]private SoundEffects sound;
    

    public void ZenModeOn()
    {
        Zen.enabled = true;

        //changes which UI elements are enabled and disabled
        ModeSelect.SetActive(false);
        TitleScreen.SetActive(false);
        UpperBarZen.SetActive(true);
        LowerBarZen.SetActive(true);

        currentMode = "Zen";
        sound.FadeOutMusic();
    }

    public void ZenModeOff()
    {
        BackToOpening();
        Zen.enabled = false;
    }

    public void TimedModeOn()
    {
        Time.enabled = true;
        ClockBasedModeOn("Timed");
    }

    public void TimedModeOff()
    {
        BackToOpening();
        Countdown.enabled = false;
        Time.enabled = false;
    }

    public void SurvivalModeOn()
    {
        Survival.enabled = true;
        ClockBasedModeOn("Survival");
    }

    public void SurvivalModeOff()
    {
        BackToOpening();
        Countdown.enabled = false;
        Survival.enabled = false;
    }

    public string GetCurrentMode()
    {
        return currentMode;
    }

    private void BackToOpening()
    { 
        //turns the opening menu back on
        OpeningDials.SetActive(true);
        TitleScreen.SetActive(true);
        TitleScreen.transform.GetChild(1).gameObject.SetActive(true);

        //checks if the Zen UI elements or the others will be disabled
        if (currentMode == "Zen")
        {
            UpperBarZen.SetActive(false);
            LowerBarZen.SetActive(false);
        }

        else
        {
            UpperBar.SetActive(false);
            LowerBar.SetActive(false);

            //resets the clock
            Countdown.TimeRestart();
            Countdown.PauseGame(true);
        }

        PauseMenu.SetActive(false);

        // removes all dial puzzles from the scene
        foreach (Transform child in DialStorage.transform)
        {
            Destroy(child.gameObject);
        }

        sound.FadeInMusic();
    }

    private void ClockBasedModeOn(string mode)
    {
        //sets the countdown and timers so they work properly
        Countdown.enabled = true;
        Countdown.TimeRestart();
        Countdown.PauseGame(true);
        Restart.Restart();

        //changes which UI elements are enabled and disabled
        ModeSelect.SetActive(false);
        TitleScreen.SetActive(false);
        UpperBar.SetActive(true);
        LowerBar.SetActive(true);

        currentMode = mode;
        sound.FadeOutMusic();
    }
}
