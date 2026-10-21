/* Script made by: velozdev 
Please subscribe to support the channel and get more free scripts like this one: https://www.youtube.com/@velozdev 
 Follow on X: https://x.com/velozdev */

using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;

public class AdManager : MonoBehaviour
{

    public static AdManager instance;

    // REMMEBER TO PUT YOUR OWN AD IDS INSTEAD OF THE TEST ONES BELOW
    private string intAdID = "ca-app-pub-3940256099942544/4411468910";
    private InterstitialAd interstitialAd;

    private string bannerAdID = "ca-app-pub-3940256099942544/2934735716";
    private BannerView bannerView;

    private string rewardedAdID = "ca-app-pub-3940256099942544/1712485313";
    private RewardedAd rewardedAd;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        MobileAds.Initialize((InitializationStatus initstatus) =>
        {
            if (initstatus == null)
            {
                Debug.LogError("Google Mobile Ads initialization failed.");
                return;
            }

            Debug.Log("Google Mobile Ads initialization complete.");

                MobileAdsEventExecutor.ExecuteInUpdate(() =>
                {
                    LoadInterstitialAd();
                    LoadRewardedAd();
                });
        });
    }

   #region Instertitial Ad

    public void LoadInterstitialAd()
    {
        var adRequest = new AdRequest();

        InterstitialAd.Load(intAdID, adRequest, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null)
            {
                Debug.LogError("Failed to load interstitial ad: " + error);
                LoadInterstitialAd(); // Retry loading the ad
                return;
            }
            interstitialAd = ad;
            interstitialAd.OnAdFullScreenContentClosed += () =>
            {
                onInsterstitialAdClosed();
            };
        });
    }

    public void ShowInterstitialAd()
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
        }
        else
        {
            Debug.LogWarning("Interstitial ad is not ready to be shown.");
        }
    }

    public void onInsterstitialAdClosed()
    {
        LoadInterstitialAd();
    }
    #endregion


   #region Banner Ad

    public void LoadBannerAd()
    {
        bannerView = new BannerView(bannerAdID, AdSize.GetPortraitAnchoredAdaptiveBannerAdSizeWithWidth(1080), AdPosition.Bottom);
        bannerView.LoadAd(new AdRequest());
        
    }

    public void ShowBannerAd()
    {
        if(bannerView == null)
        {
            LoadBannerAd();
            return;
        }
        bannerView.Show();
    }

    public void HideBannerAd()
    {
        bannerView.Hide();
    }

    public void DestroyBannerAd()
    {
        bannerView.Destroy();
        bannerView = null;
    }

    #endregion

    #region Rewarded Ad
    public void LoadRewardedAd()
    {
        var adRequest = new AdRequest();


        RewardedAd.Load(rewardedAdID, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null)
            {
                LoadRewardedAd(); //Retry loading the ad
                return;
            }
            rewardedAd = ad;

            rewardedAd.OnAdFullScreenContentClosed += () =>
            {
                LoadRewardedAd();
            };


        });
    }

    public void ShowRewardedAd()
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
                rewardedAd.Show((Reward reward) =>
                {
                    Stars.stars += 5;
                    PlayerPrefs.SetInt("Stars", Stars.stars);
                });
        }
    }

    #endregion

}
