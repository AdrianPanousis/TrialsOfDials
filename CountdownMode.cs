using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Advertisements;

public class CountdownMode : MonoBehaviour
{
    private double timeLeft;
    private bool timePaused;
    private bool soundPaused;
    private bool tickTockStarted;
    private bool TickTockNearEndStarted;

    //these are variables where the value can be dragged and dropped from Unity but is not public in code
    [SerializeField] private TextMeshProUGUI time;
    [SerializeField] private AudioSource soundSource;
    [SerializeField] private AudioClip TickTock;
    [SerializeField] private AudioClip TickTockNearEnd;
    [SerializeField] private AudioClip Alarm;
   
    void Start()
    {
        //sets the timer to two minutes
        timeLeft = 121f;
        soundPaused = false;
        tickTockStarted = false;
        TickTockNearEndStarted = false;
    }


    void Update()
    {
        //if the time is not paused and the time hasn't run out it starts counting down
        if (!timePaused)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                TimeDisplay();
            }
        }

        //if there is 15 seconds left, play the tick tock sound
        if (!tickTockStarted)
        {
            if (timeLeft < 15)
            {
                PlaySound(TickTock);
                //stops the sound from replaying every single frame so the function is only called once
                tickTockStarted = true;
            }
        }

        //if there is 9 seconds left, play the other tick tock sound that gets faster as the time approahces 0
        if (!TickTockNearEndStarted)
        {
            if(timeLeft <9)
            {
                PlaySound(TickTockNearEnd);
                //stops the sound from replaying every single frame so the function is only called once
                TickTockNearEndStarted = true;
            }
        }
    }

    //plays the sound clip sent to the function
    public void PlaySound(AudioClip clip)
    {
        //stores the clip currently playing
        soundSource.clip = clip;

        //plays the sound, checks if it is paused and unpauses it
        if (soundPaused)
        {
            soundSource.UnPause();
            soundPaused = false;
        }

        else
        {
            soundSource.Play();

        }
    }

    //controls whether the game is paused. Stops the countdown and pauses the tick tocking sound effect if it is playing
    public void PauseGame(bool isPaused)
    {
        timePaused = isPaused;
        soundPaused = isPaused;
        if(isPaused)
        {
            soundSource.Pause();
        }

        else
        {
            soundSource.UnPause();
        }
    }

    //resets all of the booleans and sets the time back to two minutes 
    public void TimeRestart()
    {
        timeLeft = 121f;
        timePaused = false;
        tickTockStarted = false;
        TickTockNearEndStarted = false;
    }

    //takes the time left and displays it in minutes and seconds rather than a single float value
    private void TimeDisplay()
    {
        var ts = TimeSpan.FromSeconds(timeLeft);
        time.text = string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
    }

    public double getTimeLeft()
    {
        return timeLeft;
    }

    //gets clip of alarm sound played when the time runs out
    public AudioClip getAlarmClip()
    {
        return Alarm;
    } 

    //extends time left in survival mode and restarts the sound booleans so they work again when the time goes down to 15 or 9 seconds
    public void addTime(double t)
    {
        timeLeft += t;
        soundSource.Stop();
        tickTockStarted = false;
        TickTockNearEndStarted = false;
    }
}
