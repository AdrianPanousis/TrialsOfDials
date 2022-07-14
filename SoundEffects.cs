using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundEffects : MonoBehaviour
{
   
    private AudioSource SoundSource;
    [SerializeField] private AudioSource OpeningMusic;
    [SerializeField] private RectTransform SoundButton;
    [SerializeField] private GameObject SoundOffText;
    [SerializeField] private GameObject SoundOnText;

    [SerializeField] private RectTransform MusicButton;
    [SerializeField] private GameObject MusicOffText;
    [SerializeField] private GameObject MusicOnText;


    
    private bool isFadingMusicIn;
    private bool isFadingMusicOut;
    private int MusicSwitchValue;
    private int SoundSwitchValue;

    void Start()
    {
        SoundSource = transform.GetComponent<AudioSource>();
        isFadingMusicIn = false;
        isFadingMusicOut = false;
        OpeningMusic.volume = 0;
        MusicSwitchValue = 1;
        SoundSwitchValue = 1;
       
    }

   

    private void Update()
    {
        if(isFadingMusicIn)
        {
            FadingInMusic();
        }

        if(isFadingMusicOut)
        {
            FadingOutMusic();
        }
    }

    public void PlaySound(AudioClip sound)
    {
        SoundSource.clip = sound;
        SoundSource.Play();
    }

    //starts the process of fading in the music
    public void FadeInMusic()
    {
        isFadingMusicIn = true;
        OpeningMusic.Play();
    }

    //starts the process of fading out the music
    public void FadeOutMusic()
    {
        isFadingMusicOut = true;
    }

    //fades in the music over time by slowly increasing the volume
    private void FadingInMusic()
    {
        
        if (OpeningMusic.volume < 1)
        {
            OpeningMusic.volume += Time.deltaTime;
        }

        //stops the fading in process and sets the volume to 100%
        else
        {
            isFadingMusicIn = false;
            OpeningMusic.volume = 1;
        }
    }

    //fades out the music over time by slowly decreasing the volume
    private void FadingOutMusic()
    {
        if (OpeningMusic.volume > 0)
        {
            OpeningMusic.volume -= Time.deltaTime;
        }

        //stops the fading out process and sets the volume to 0
        else
        {
            isFadingMusicOut = false;
            OpeningMusic.volume = 0;
            OpeningMusic.Stop();
        }
    }

    //turns the music on and off when the music button is tapped in the pause menu
    public void MusicSwitch(int s)
    {
        MusicSwitchValue -= s;
        if(Mathf.Abs(MusicSwitchValue) == 1)
        {
            TurnOnMusic();
            MusicSwitchValue = Mathf.Abs(MusicSwitchValue);
        }

        else
        {
            TurnOffMusic();
        }
    }

    //turns the sound on and off when the music button is tapped in the pause menu
    public void SoundSwitch(int s)
    {
        SoundSwitchValue -= s;
        if(Mathf.Abs(SoundSwitchValue) == 1)
        {
            TurnOnSoundEffects();
            SoundSwitchValue = Mathf.Abs(SoundSwitchValue);
        }

        else
        {
            TurnOffSoundEffects();
        }
    }

    //disables the music and changes the UI for the music button
    private void TurnOffMusic()
    {
       OpeningMusic.enabled = false;
       MusicButton.localPosition = new Vector3(-24, -0.3f, 0);
       MusicOnText.SetActive(false);
       MusicOffText.SetActive(true);
    }

    //enables the music and changes the UI for the music button
    private void TurnOnMusic()
    {
        OpeningMusic.enabled = true;
        MusicButton.localPosition = new Vector3(24, -0.3f, 0); ;
        MusicOnText.SetActive(true);
        MusicOffText.SetActive(false);
    }

    //disables the sound and changes the UI for the sound button
    private void TurnOffSoundEffects()
    {
      
        OpeningMusic.transform.GetComponent<AudioListener>().enabled = false;
        SoundButton.localPosition = new Vector3(-24, -0.3f, 0);
        SoundOnText.SetActive(false);
        SoundOffText.SetActive(true);
    }

    //enables the sound and changes the UI for the sound button
    private void TurnOnSoundEffects()
    {
       
        OpeningMusic.transform.GetComponent<AudioListener>().enabled = true;
        SoundButton.localPosition = new Vector3(24, -0.3f, 0);
        SoundOnText.SetActive(true);
        SoundOffText.SetActive(false);
    }
   
}
