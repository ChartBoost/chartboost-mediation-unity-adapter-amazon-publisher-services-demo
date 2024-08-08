using System.Collections.Generic;
using AmazonAds;
using Chartboost.Core;
using Chartboost.Core.Initialization;
using Chartboost.Mediation;
using Chartboost.Mediation.Ad.Banner;
using Chartboost.Mediation.Ad.Fullscreen;
using Chartboost.Mediation.AmazonPublisherServices;
using Chartboost.Mediation.AmazonPublisherServices.Common;
using Chartboost.Mediation.Requests;
using UnityEngine;
using UnityEngine.UI;

namespace Sample
{
    public class AmazonChartboostDemo : MonoBehaviour
    {
        [SerializeField] private Image initializationImage;
        [SerializeField] private Image interstitialLoadImage;
        [SerializeField] private Image interstitialShowImage;
        [SerializeField] private Image rewardedLoadImage;
        [SerializeField] private Image rewardedShowImage;
        [SerializeField] private Image bannerImage;

        private readonly Color _orange = new(255, 128, 0);

        private IFullscreenAd _interstitialAd;
        private IFullscreenAd _rewardedAd;
        private IBannerAd _bannerAd;

        private bool _chartboostMediationInitialized;

        private const string AmazonAPIKeyAndroid = "25663269-1a14-4a45-b977-768c2278758d";
        private const string AmazonAPIKeyIOS = "f2b4893c-8585-42b3-ab6c-b0e943a17c0c";
        
        private const string AndroidAppId = "5a4e797538a5f00cf60738d6";
        private const string IOSAppId = "59c04299d989d60fc5d2c782";

        private const string BannerPlacement = "APSBannerStandard";
        private const string InterstitialPlacement = "APSInterstitial";
        private const string RewardedPlacement = "APSRewarded";

        private static string AppId => Application.platform switch
        {
            RuntimePlatform.Android => AndroidAppId,
            RuntimePlatform.IPhonePlayer => IOSAppId,
            _ => string.Empty
        };

        private static string AmazonAPIKey => Application.platform switch
        {
            RuntimePlatform.Android => AmazonAPIKeyAndroid,
            RuntimePlatform.IPhonePlayer => AmazonAPIKeyIOS,
            _ => string.Empty
        };

        private PreBiddingListener _preBiddingListener;

        public void Initialize()
        {
            InitializeAmazonPublisherServices();
            InitializeChartboostMediation();
        }
        
        public void IsInitialized()
        {
            if (initializationImage == null) return;
            if (Amazon.IsInitialized() && _chartboostMediationInitialized)
                initializationImage.color = Color.green;
            else if (!Amazon.IsInitialized() || !_chartboostMediationInitialized)
                initializationImage.color = _orange;
            else
                initializationImage.color = Color.red;
        }

        private void InitializeAmazonPublisherServices()
        {
            Amazon.Initialize(AmazonAPIKey);
            Amazon.UseGeoLocation(true);
            Amazon.IsLocationEnabled();
            Amazon.SetAdNetworkInfo(new AdNetworkInfo(DTBAdNetwork.OTHER));
            SetTestFlags();
#if UNITY_IOS
            Amazon.SetAPSPublisherExtendedIdFeatureEnabled(true);
#endif
            IsInitialized();
        }

        private void InitializeChartboostMediation()
        {
            if (_chartboostMediationInitialized)
                return;
            
            AmazonPublisherServicesAdapter.PreBiddingListener = new CustomAmazonPublisherServicesPreBiddingListener();

            ChartboostCore.ModuleInitializationCompleted += initializationResult =>
            {
                if (initializationResult.ModuleId != ChartboostMediation.CoreModuleId)
                    return;
                
                if (initializationResult.Error.HasValue)
                {
                    Debug.LogError(initializationResult.Error.Value.Message);
                    return;
                }

                _chartboostMediationInitialized = true;
                IsInitialized();
            };

            ChartboostMediation.DidReceivePartnerAdapterInitializationData += partnerInitializationData =>
            {
                Debug.Log($"Partner Initialization Data: {partnerInitializationData}");
            };

            var sdkConfiguration = new SDKConfiguration(AppId, null);
            ChartboostCore.Initialize(sdkConfiguration);
        }

        public async void LoadInterstitial()
        {
            _interstitialAd?.Dispose();
            interstitialLoadImage.color = Color.white;

            var interstitialAdLoadRequest = new FullscreenAdLoadRequest(InterstitialPlacement, new Dictionary<string, string>());
            interstitialLoadImage.color = _orange;
            var adLoadResult = await ChartboostMediation.LoadFullscreenAd(interstitialAdLoadRequest);

            if (adLoadResult.Error.HasValue)
            {
                interstitialLoadImage.color = Color.red;
                Debug.LogError($"CBMediation: Ad Failed to Load: {adLoadResult.Error.Value.Message}");
                return;
            }

            _interstitialAd = adLoadResult.Ad;
            interstitialLoadImage.color = Color.green;
        }

        public async void ShowInterstitial()
        {
            interstitialShowImage.color = Color.white;
            
            if (_interstitialAd == null)
                return;
            
            interstitialShowImage.color = _orange;
            var adShowResult = await _interstitialAd.Show();

            if (adShowResult.Error.HasValue)
            {
                Debug.LogError($"Failed to Show: {adShowResult.Error.Value.Message}");
                interstitialShowImage.color = Color.red;
                return;
            }
            
            interstitialShowImage.color = Color.green;
            Debug.Log("Showed Ad Successfully!");
        }

        public async void LoadRewarded()
        {
            _rewardedAd?.Dispose();
            rewardedLoadImage.color = Color.white;

            var rewardedAdLoadRequest = new FullscreenAdLoadRequest(RewardedPlacement, new Dictionary<string, string>());
            rewardedLoadImage.color = _orange;
            var adLoadResult = await ChartboostMediation.LoadFullscreenAd(rewardedAdLoadRequest);

            if (adLoadResult.Error.HasValue)
            {
                rewardedLoadImage.color = Color.red;
                Debug.LogError($"CBMediation: Ad Failed to Load: {adLoadResult.Error.Value.Message}");
                return;
            }

            _rewardedAd = adLoadResult.Ad;
            rewardedLoadImage.color = Color.green;
        }
        
        public async void ShowRewarded()
        {
            rewardedShowImage.color = Color.white;
            
            if (_rewardedAd == null)
                return;
            
            rewardedShowImage.color = _orange;
            var adShowResult = await _rewardedAd.Show();

            if (adShowResult.Error.HasValue)
            {
                Debug.LogError($"Failed to Show: {adShowResult.Error.Value.Message}");
                rewardedShowImage.color = Color.red;
                return;
            }
            
            Debug.Log("Showed Ad Successfully!");
            rewardedShowImage.color = Color.green;
        }

        public async void LoadBanner()
        {
            _bannerAd?.Dispose();
            bannerImage.color = Color.white;

            var bannerAdLoadRequest = new BannerAdLoadRequest(BannerPlacement, BannerSize.Standard);
            _bannerAd = ChartboostMediation.GetBannerAd();

            bannerImage.color = _orange;
            var adLoadResult = await _bannerAd.Load(bannerAdLoadRequest);
            
            if (adLoadResult.Error.HasValue)
            {
                bannerImage.color = Color.red;
                Debug.LogError($"CBMediation: Ad Failed to Load: {adLoadResult.Error.Value.Message}");
                return;
            }
            bannerImage.color = Color.green;
        }

        private void SetTestFlags()
        {
            Amazon.EnableTesting(true);
            Amazon.EnableLogging(true);
            AmazonPublisherServicesAdapter.TestMode = true;
            AmazonPublisherServicesAdapter.VerboseLogging = true;
        }
    }
}
