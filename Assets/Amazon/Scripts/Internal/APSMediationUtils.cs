using System;
using UnityEngine;

namespace AmazonAds {
    public class APSMediationUtils
    {
        public static string APS_IRON_SOURCE_NETWORK_KEY = "APS";
        private static string APS_REWARDED_VIDEO_KEY = "rewardedvideo";
        private static string APS_INTERSTITIAL_KEY = "interstitial";

        public static string GetInterstitialNetworkData(string amazonSlotId, AmazonAds.AdResponse adResponse)
        {
            return GetNetworkData(amazonSlotId, adResponse, APS_INTERSTITIAL_KEY);

        }

        public static string GetRewardedNetworkData(string amazonSlotId, AmazonAds.AdResponse adResponse)
        {
            return GetNetworkData(amazonSlotId, adResponse, APS_REWARDED_VIDEO_KEY);
        }

        private static string GetNetworkData(string amazonSlotId, AmazonAds.AdResponse adResponse , string adInventoryType)
        {
            APSIronSourceNetworkInputData ironSourceInputData = new APSIronSourceNetworkInputData();
#if UNITY_ANDROID
            ironSourceInputData.bidInfo = adResponse.GetBidInfo();

#endif
            ironSourceInputData.pricePointEncoded = adResponse.GetPricePoint();
            ironSourceInputData.uuid = amazonSlotId;
            APSIronSourceNetworkData networkData = new APSIronSourceNetworkData();
            networkData.networkInputData = ironSourceInputData;

#if UNITY_IOS
            string mediationHints = "\"mediationHints\" :" + adResponse.GetMediationHints();
            string jsonData = "{\""+adInventoryType+"\" :" + JsonUtility.ToJson(ironSourceInputData);
            jsonData = jsonData.Remove(jsonData.Length - 1);
            jsonData = jsonData + ", " + mediationHints + "}}";
#else
            string jsonData = "{\""+adInventoryType+"\" :" + JsonUtility.ToJson(ironSourceInputData) + "}";
#endif
            return jsonData;
        }


        public static string GetBannerNetworkData(string amazonSlotId, AmazonAds.AdResponse adResponse)
        {
            APSIronSourceNetworkBannerInputData ironSourceInputData = new APSIronSourceNetworkBannerInputData();
#if UNITY_ANDROID
            ironSourceInputData.bidInfo = adResponse.GetBidInfo();
#endif
            ironSourceInputData.pricePointEncoded = adResponse.GetPricePoint();
            ironSourceInputData.uuid = amazonSlotId;
            ironSourceInputData.width = adResponse.GetWidth();
            ironSourceInputData.height = adResponse.GetHeight();

            APSIronSourceBannerNetworkData networkData = new APSIronSourceBannerNetworkData();
            networkData.banner = ironSourceInputData;

#if UNITY_IOS
            string mediationHints = "\"mediationHints\" :" + adResponse.GetMediationHints();
            string jsonData = "{ \"banner\" :" + JsonUtility.ToJson(ironSourceInputData);
            jsonData = jsonData.Remove(jsonData.Length - 1);
            jsonData = jsonData + ", " + mediationHints + "}}";
#else
            string jsonData = "{ \"banner\" :" + JsonUtility.ToJson(ironSourceInputData) + "}";
#endif            
            return jsonData;
        }

        public class APSIronSourceNetworkInputData
        {
            public string uuid;
            public string pricePointEncoded;
#if UNITY_ANDROID
            public string bidInfo;

#endif
        }

        public class APSIronSourceNetworkBannerInputData
        {
#if UNITY_ANDROID
            public string bidInfo;

#endif
            public string pricePointEncoded;
            public string uuid;
            public int width;
            public int height;
        }

        public class APSIronSourceBannerNetworkData
        {
            public APSIronSourceNetworkBannerInputData banner;
        }

        public class APSIronSourceNetworkData
        {
            public APSIronSourceNetworkInputData networkInputData;
        }

        private APSMediationUtils()
        {
        }
    }
}
