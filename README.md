# AdMobTutorial

Unity testing project for AdMob in iOS and Android.

Companion project for the **@velozdev** video *"AdMob in Unity: Interstitial, Banner & Rewarded Ads"*. It's a small 2D scene with four buttons that show **Interstitial**, **Rewarded** and **Banner** ads using Google's **test ad units**, so you can see the whole flow working before you touch real ads.

- YouTube: [@velozdev](https://www.youtube.com/@velozdev)
- X: [@velozdev](https://x.com/velozdev)
- Video: [https://youtu.be/wirLRhVqImI](https://youtu.be/wirLRhVqImI)

---

## What's in the scene

`Assets/Scenes/SampleScene.unity` has a Canvas with these buttons and a star counter.

| Button | Calls | Result |
|---|---|---|
| **ShowInterstitial** | `TestAds.ShowInterstitialAd()` | Shows a full-screen interstitial ad. |
| **ShowRewardedAd** | `TestAds.ShowRewardedAd()` | Shows a rewarded ad. Watch it to the end and you get **+5 stars**. |
| **ShowBanner** | `TestAds.ShowBanner()` | Loads the banner the first time, then shows it at the bottom of the screen. |
| **HideBanner** | `TestAds.HideBanner()` | Hides the banner. |

The top of the screen shows the player's stars (`StarsText`).

---

## Requirements

| | |
|---|---|
| Unity | `6000.3.9f1` (Unity 6), 2D URP template |
| Google Mobile Ads plugin | `com.google.ads.mobile` **11.6.0** |
| Platforms | iOS and Android |
| Other | TextMeshPro (for the star counter) |

The plugin comes from OpenUPM and is already in `Packages/manifest.json`:

```json
"scopedRegistries": [
  {
    "name": "OpenUPM",
    "url": "https://package.openupm.com",
    "scopes": [ "com.google" ]
  }
]
```

---

## Getting started

1. Clone the repo and open the folder with Unity Hub (Unity `6000.3.9f1`).
2. Let Unity import the packages. The Google Mobile Ads plugin installs automatically from OpenUPM.
3. Open **Assets > Google Mobile Ads > Settings** and set your **App IDs**:
   - Android App ID: `adMobAndroidAppId`
   - iOS App ID: `adMobIOSAppId`
   - To test with Google's sample apps, use the test App IDs below.
4. Open `Assets/Scenes/SampleScene.unity` and press **Play** (ads are placeholders in the Editor) or build to a device.

> **Platform note:** in this repo the iOS App ID field is filled in and the Android one is empty. Add the Android App ID before building for Android, or the app will crash on launch.

---

## Test IDs (Google's public sample IDs)

### App IDs

| Platform | Test App ID |
|---|---|
| Android | `ca-app-pub-3940256099942544~3347511713` |
| iOS | `ca-app-pub-3940256099942544~1458002511` |

### Ad unit IDs

| Format | Android | iOS |
|---|---|---|
| Banner | `ca-app-pub-3940256099942544/6300978111` | `ca-app-pub-3940256099942544/2934735716` |
| Interstitial | `ca-app-pub-3940256099942544/1033173712` | `ca-app-pub-3940256099942544/4411468910` |
| Rewarded | `ca-app-pub-3940256099942544/5224354917` | `ca-app-pub-3940256099942544/1712485313` |

> ⚠️ **Never test with your real ad unit IDs.** Showing or clicking your own live ads while developing can get your AdMob account limited or banned. Swap in your real IDs only for the production build.

---

## Project structure

```
Assets/
├── AdManager.cs        # All AdMob logic (init, load, show, events)
├── TestAds.cs          # Button handlers that forward to AdManager
├── Stars.cs            # Star counter (saved in PlayerPrefs)
├── Scenes/
│   └── SampleScene.unity
└── GoogleMobileAds/    # Plugin settings (App IDs)
```

### `AdManager.cs`
- Singleton (`AdManager.instance`) set in `Awake()`. Any script can call it without a reference.
- `Start()` calls `MobileAds.Initialize(...)`. When initialization completes, it loads the **interstitial** and **rewarded** ads (through `MobileAdsEventExecutor.ExecuteInUpdate`, so the calls run on Unity's main thread).
- Loaded ads are stored in a private field (`interstitialAd = ad;`). Without that line, `Show` does nothing because the ad is never "ready".
- When an interstitial or rewarded ad is **closed**, the next one is loaded automatically. If a load fails, it tries again.

| Region | Methods |
|---|---|
| Interstitial | `LoadInterstitialAd()`, `ShowInterstitialAd()`, `onInsterstitialAdClosed()` |
| Banner | `LoadBannerAd()`, `ShowBannerAd()`, `HideBannerAd()`, `DestroyBannerAd()` |
| Rewarded | `LoadRewardedAd()`, `ShowRewardedAd()` |

### `TestAds.cs`
Only forwards the UI button clicks to `AdManager.instance`. The UI doesn't need its own copy of the ad logic.

### `Stars.cs`
Shows the star count in a `TextMeshProUGUI` and loads it from `PlayerPrefs` key `"Stars"`. The rewarded ad adds 5 stars (`Stars.stars += 5`) and saves it.

> `PlayerPrefs` is fine for a demo but not recommended for real game progress.

---

## Things worth knowing

- **Always load after initialization.** Loading before the SDK has initialized gives *"ad is not ready to be shown"*.
- **Reload after closing** an interstitial or rewarded ad so the next one is ready.
- **Banner:** the first `ShowBanner` creates and loads it; later `Show` / `Hide` reuse the same banner (no cloning). `DestroyBannerAd()` removes it completely and sets the reference to `null`, so the next `ShowBannerAd()` creates a new one. It isn't wired to a button in the scene.
- **Banner sizes:** the banner uses an anchored adaptive size. *Large Banner* is another option to try. *Smart Banner* is obsolete.
- **Rewarded amount:** you can set the reward amount and item in the AdMob dashboard (*Ad unit > Rewards*). In this project the amount is hardcoded to 5 in `AdManager.cs`.
- Rewarded **server-side verification (SSV)** is not covered.

---

## Going to production

- [ ] Replace the test App IDs with your own in *Google Mobile Ads > Settings* (Android and iOS).
- [ ] Replace the test ad unit IDs in `AdManager.cs` with your own, one per format and platform.
- [ ] Pick IDs by platform (`#if UNITY_ANDROID` / `#elif UNITY_IOS`) and keep test IDs for Editor and development builds.
- [ ] Link the app in AdMob to its store listing.
- [ ] Handle user consent (GDPR/UMP) and, on iOS, App Tracking Transparency where they apply.
- [ ] Test on a real device before releasing.

---

## Not covered in this video

App Open, Native and Rewarded Interstitial ads, and mediation. I may cover them in another video.

---

## Credits

Made by **Juan Veloz (@velozdev)**. If this helped you, subscribe on YouTube to support the channel and get more free scripts like this one.

Google Mobile Ads and AdMob are products of Google LLC. This project is not affiliated with Google.
