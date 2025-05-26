using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using AudienceNetwork;
public class FBRewardVideo : MonoBehaviour
{
    private void Awake()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        //AudienceNetworkAds.Initialize();
#endif
    }

    void OnDestroy()
    {
        // Dispose of rewardedVideo ad when the scene is destroyed
        Debug.Log("RewardedVideoAdTest was destroyed!");
    }
}
