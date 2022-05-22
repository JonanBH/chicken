using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdvertiseController : MonoBehaviour
{
    public System.Action<bool> OnRewardAdsComplete;
    public string AndroidGameId;
    public bool IsRewardAdsReady;
    public string IOSGameId;
    public bool TestMode = true;

    // Start is called before the first frame update
    void Start()
    {
        InitializeAds();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeAds()
    {
        string gameId = (Application.platform != RuntimePlatform.Android ? AndroidGameId : IOSGameId);
        Advertisement.Initialize(gameId, TestMode, this);
    }
}
