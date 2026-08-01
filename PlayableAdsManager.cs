using UnityEngine;
using System;

public class PlayableAdsManager : MonoBehaviour
{
    public static PlayableAdsManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowRewardedAd(Action onReward)
    {
        Debug.Log("Rewarded Ad Placeholder");

        // Temporary reward for testing
        onReward?.Invoke();
    }
}