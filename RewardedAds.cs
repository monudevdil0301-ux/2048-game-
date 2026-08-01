using UnityEngine;
using UnityEngine.Advertisements;

public class RewardedAds : MonoBehaviour, IUnityAdsShowListener
{
    public Board board;

    string rewardedAdUnitId = "Rewarded_Android";

    public void ShowReviveAd()
    {
        Advertisement.Show(rewardedAdUnitId, this);
    }

    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            board.ContinueWithAd();
        }
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message) {}
    public void OnUnityAdsShowStart(string adUnitId) {}
    public void OnUnityAdsShowClick(string adUnitId) {}
}