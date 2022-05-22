using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdvertiseController : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public System.Action<bool> OnRewardAdsComplete;
    public string AndroidGameId;
    public bool IsRewardAdsReady;
    public string IOSGameId;
    public bool TestMode = true;

    private string rewardedPlacementId = "Rewarded_Android";

    public System.Action OnRewardedComplete;
    public System.Action OnRewardedFailed;

    // Start is called before the first frame update
    void Awake()
    {
        InitializeAds();
    }

    private void Start()
    {
       // LoadRewardedAds();
    }
    public void LoadRewardedAds()
    {
        //string rewardedId = Application.platform == RuntimePlatform.Android ? "Rewarded_Android" : "Rewarded_iOS";
        Debug.Log("Loading Ad :" + rewardedPlacementId);
        Advertisement.Load(rewardedPlacementId, this);
    }

    public void InitializeAds()
    {
        string gameId = (Application.platform != RuntimePlatform.Android ? AndroidGameId : IOSGameId);

        gameId = AndroidGameId;
        Advertisement.Initialize(gameId, TestMode, this);
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Initialization completed");
        LoadRewardedAds();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log("Initialization failed");
        //InitializeAds();
    }
    public void ShowRewardedAd()
    {
        Advertisement.Show(rewardedPlacementId, this);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("Ad Loaded : " + placementId);
        IsRewardAdsReady = true;
    }


    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log("Fail to load Ad : " + placementId);
        Debug.Log("Reason : " + message);
        IsRewardAdsReady = false;
        //LoadRewardedAds();
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log("Fail to show Ad : " + placementId);
        IsRewardAdsReady = false;
        //LoadRewardedAds();
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log("Now showing " + placementId);
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log("Now clicked " + placementId);
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log("Now completed " + placementId);
        if (showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            OnRewardedComplete?.Invoke();
        }
        else
        {
            OnRewardedFailed?.Invoke();
        }

        IsRewardAdsReady = false;
        LoadRewardedAds();
    }
}
