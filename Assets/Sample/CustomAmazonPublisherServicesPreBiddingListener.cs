using System.Threading.Tasks;
using AmazonAds;
using Chartboost.Mediation.AmazonPublisherServices;
using Chartboost.Mediation.AmazonPublisherServices.Common;
using UnityEngine;

namespace Sample
{
    public class CustomAmazonPublisherServicesPreBiddingListener : PreBiddingListener
    {
        private const string Tag = "[APS PreBidding Listener]";
        
        public override Task<AmazonPublisherServicesAdapterPreBidAdInfo> OnPreBid(AmazonPublisherServicesAdapterPreBidRequest request)
        {
            AdRequest adRequest;
            var bannerSize = request.BannerSize;
            var amazonSettings = request.AmazonSettings;
            var amazonPlacement = amazonSettings.PartnerPlacement;
            
            Debug.Log($"{Tag} Format: {request.Format}, Chartboost Placement: {request.MediationPlacement}, Amazon Placement: {amazonPlacement}");

            switch (request.Format)
            {
                case "rewarded":
                    var width = amazonSettings.Width ?? 0;
                    var height = amazonSettings.Height ?? 0;
                    adRequest = new APSVideoAdRequest(width, height, amazonPlacement);
                    break;

                case "interstitial":
                case "rewarded_interstitial":
                    adRequest = new APSInterstitialAdRequest(amazonPlacement);
                    break;

                case "banner":
                case "adaptive_banner":
                    var bannerWidth = bannerSize.HasValue ? (int)bannerSize.Value.Width : 0;
                    var bannerHeight = bannerSize.HasValue ? (int)bannerSize.Value.Height : 0;
                    adRequest = new APSBannerAdRequest(bannerWidth, bannerHeight, amazonPlacement);
                    break;
                
                default:
                    Debug.LogWarning($"{Tag} Specified type is not valid, returning null values.");
                    return Task.FromResult(new AmazonPublisherServicesAdapterPreBidAdInfo(null, null));
            }

            foreach (var keyword in  request.Keywords)
            { 
                adRequest.PutCustomTarget(keyword.Key, keyword.Value);
            }

            var taskCompletionSource = new TaskCompletionSource<AmazonPublisherServicesAdapterPreBidAdInfo>();
            
            adRequest.onSuccess += response =>
            {
                Debug.Log($"{Tag} Response succeeded for: CBP: {request.MediationPlacement} - AMZP: {amazonPlacement}!");
                #if UNITY_IOS
                taskCompletionSource.SetResult(new AmazonPublisherServicesAdapterPreBidAdInfo(response.GetPricePoint(), response.GetMediationHints()));
                #elif UNITY_ANDROID
                taskCompletionSource.SetResult(new AmazonPublisherServicesAdapterPreBidAdInfo(response.GetPricePoint(), response.GetBidInfo()));
                #else
                taskCompletionSource.SetResult(new AmazonPublisherServicesAdapterPreBidAdInfo(null, null));   
                #endif
            };

            adRequest.onFailedWithError += error =>
            {
                Debug.LogError($"{Tag} Failed with Error: {error.GetMessage()} and Code: {error.GetCode()}");
                taskCompletionSource.SetResult(new AmazonPublisherServicesAdapterPreBidAdInfo(null, null));
            };

            adRequest.LoadAd();
            return taskCompletionSource.Task;
        }
    }
}
