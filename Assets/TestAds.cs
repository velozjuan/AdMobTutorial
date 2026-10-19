/* Script made by: velozdev 
Please subscribe to support the channel and get more free scripts like this one: https://www.youtube.com/@velozdev 
 Follow on X: https://x.com/velozdev */

using UnityEngine;

public class TestAds : MonoBehaviour
{

    #region Instertitial Ad
   
    public void ShowInterstitialAd()
    {
        AdManager.instance.ShowInterstitialAd();
    }

    #endregion

    #region Banner Ad
    public void ShowBanner()
    {
        AdManager.instance.ShowBannerAd();
    }

    public void HideBanner()
    {
        AdManager.instance.HideBannerAd();
    }

    #endregion

    #region Rewarded Ad

    public void ShowRewardedAd()
    {
        AdManager.instance.ShowRewardedAd();
    }
    
    #endregion

}
