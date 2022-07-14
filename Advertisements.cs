using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class Advertisements : MonoBehaviour,IUnityAdsInitializationListener,IUnityAdsLoadListener,IUnityAdsShowListener
{
   
    //checks if it is the apple version of the app
    public bool isAppleVersion;
    //turns on and off test mode for ads
    public bool testMode;
    //used to pause and unpause the game when an add is played
    private CountdownMode Timer;
    //used as a switch to show an ad or not
    private int adShownValue;

    //app store IDs for the Unity ads to load
    private string GooglePlayID = "4811811";
    private string AppStoreID = "4811810";

    private void Start()
    {
        Timer = transform.GetComponent<CountdownMode>();
        adShownValue = 1;
        StartCoroutine( CheckPlatform());
    }

    //checks if the platform is IOS or Android
    private IEnumerator CheckPlatform()
    {
        yield return new WaitForSeconds(0.5f);
        //initialises the ad so it is ready to play the moment the user changes the scene
        if (isAppleVersion)
        {
            Advertisement.Initialize(AppStoreID, testMode,this);

        }

        else
        {
            Advertisement.Initialize(GooglePlayID, testMode,this);     
            
        }

    }


    public void PlayInterstitialAd()
    {
        //value is switched so the ad plays every second time the function is called
        adShownValue -= 1;
        adShownValue = Mathf.Abs(adShownValue);
        if (adShownValue == 0)
        {
            Advertisement.Show("video", this);
        }         
    }

    public void PlayBannerAd()
    {
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Show("banner");   
    }

    public void HideBannnerAd()
    {
        Advertisement.Banner.Hide();
    }

  

    public void OnInitializationComplete()
    {
        //do nothing
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        //do nothing
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        //do nothing
    }

    //pauses the game when the ad is shown so the timer doesn't countdown
    public void OnUnityAdsShowStart(string placementId)
    {
        Timer.PauseGame(true);
    }

    //unpauses the game
    public void OnUnityAdsShowClick(string placementId)
    {
        Timer.PauseGame(false);
    }

    //unpauses the game
    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Timer.PauseGame(false);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        //do nothing
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        //do nothing
    }
}
