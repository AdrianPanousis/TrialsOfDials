using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;


public class CompanyLogo : MonoBehaviour
{

    public GuiFadeInFadeOut LogoText;
    public GuiFadeInFadeOut Logo;

    //a black image that is used to transition between scenes
    [SerializeField] private GuiFadeInFadeOut TransitionImage;
    [SerializeField] private SoundEffects OpeningMusic;

    //invisible button that takes up the whole screen during the intro
    [SerializeField] private Button TapAnywhereButton;

    void Start()
    {
        StartCoroutine(DelayFadeIn());        
        StartCoroutine(DelayFade());
    }

    //Fades out the company logo after 6 seconds
    private IEnumerator DelayFade()
    {
        yield return new WaitForSeconds(6.0f);
        LogoText.enabled = false;
        Logo.enabled = false;
        TransitionImage.FadeOut();
        OpeningMusic.FadeInMusic();
        TapAnywhereButton.enabled = true;
    }
  

    //Fades in the company logo at the start of the app
    private IEnumerator DelayFadeIn()  
    {
        yield return new WaitForSeconds(0.1f);
        ChangeDelay();
        LogoText.FadeIn("");
        Logo.FadeIn("");     
    }

    //fades out the company logo after 3 seconds
    private void ChangeDelay()
    {
        LogoText.ChangeFadeOutDelay(3.0f);
        Logo.ChangeFadeOutDelay(3.0f);   
    }
}
