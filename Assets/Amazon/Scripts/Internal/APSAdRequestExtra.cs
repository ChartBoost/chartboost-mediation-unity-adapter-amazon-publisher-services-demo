using System;
using System.Collections.Generic;
using UnityEngine;

namespace AmazonAds
{
    public class APSAdRequestExtra
    {
        private static readonly string ADMOB_SLOTUUID_KEY = "amazon_custom_event_slot_uuid";
        private static readonly string ADMOB_REQUEST_ID_KEY = "amazon_custom_event_request_id";
        private static readonly string APS_ADAPTER_VERSION = "amazon_custom_event_adapter_version";
        private static readonly string ADMOB_ISVIDEO_KEY = "amazon_custom_event_is_video";
        private static readonly string ADMOB_WIDTH_KEY = "amazon_custom_event_width";
        private static readonly string ADMOB_HEIGHT_KEY = "amazon_custom_event_height";
        private static readonly string APS_ADMOB_CONST_CCPA_APS_PRIVACY = "aps_privacy";
        private static readonly string APS_ADMOB_CONST_CCPA_US_PRIVACY = "us_privacy";
        private static readonly AndroidJavaClass dtbAdUtil = new AndroidJavaClass ("com.amazon.device.ads.DTBAdUtil");

        private APSAdRequestExtra (Builder builder) {
            this.SlotUUID = builder.SlotUUID;
            this.BannerAdHeight = builder.AdHeight;
            this.BannerAdWidth = builder.AdWidth;
            this.CCPAPrivacy = builder.CCPAPrivacy;
        }

        public string SlotUUID { get; private set; }
        public int BannerAdHeight { get; private set; }
        public int BannerAdWidth { get; private set; }
        public Dictionary<string, string> CCPAPrivacy { private get;  set; }

        public Dictionary<string, string> GetAdMobInterstitialRequestExtras ()
        {
            Dictionary<String, String> AdMobRequestExtras = createExtras ();
            if (SlotUUID == null)
            {
                return AdMobRequestExtras;
            }

#if UNITY_ANDROID
            AndroidJavaObject bundle = dtbAdUtil.CallStatic<AndroidJavaObject>("createAdMobInterstitialVideoRequestBundle", SlotUUID);
            AdMobRequestExtras.Add (ADMOB_SLOTUUID_KEY, bundle.Call<String>("getString", ADMOB_SLOTUUID_KEY));
            AdMobRequestExtras.Add (ADMOB_REQUEST_ID_KEY, bundle.Call<String>("getString", ADMOB_REQUEST_ID_KEY));
            AdMobRequestExtras.Add (APS_ADAPTER_VERSION, bundle.Call<String>("getString", APS_ADAPTER_VERSION));       
#else
            AdMobRequestExtras.Add (ADMOB_SLOTUUID_KEY, SlotUUID);
#endif
            return AdMobRequestExtras;

        }

        public Dictionary<string, string> GetAdMobBannerRequestExtras ()
        {
            Dictionary<String, String> AdMobRequestExtras = createExtras();
            if (SlotUUID == null || BannerAdWidth  == 0 || BannerAdHeight == 0)
            {
                return AdMobRequestExtras;
            }

#if UNITY_ANDROID
            AndroidJavaObject bundle = dtbAdUtil.CallStatic<AndroidJavaObject> ("createAdMobBannerRequestBundle", SlotUUID, BannerAdWidth, BannerAdHeight);
            AdMobRequestExtras.Add (ADMOB_SLOTUUID_KEY, bundle.Call<String>("getString", ADMOB_SLOTUUID_KEY));
            AdMobRequestExtras.Add (ADMOB_HEIGHT_KEY, bundle.Call<int>("getInt", ADMOB_HEIGHT_KEY).ToString());
            AdMobRequestExtras.Add (ADMOB_WIDTH_KEY, bundle.Call<int>("getInt", ADMOB_WIDTH_KEY).ToString());
            AdMobRequestExtras.Add (ADMOB_REQUEST_ID_KEY, bundle.Call<String>("getString", ADMOB_REQUEST_ID_KEY));
            AdMobRequestExtras.Add (APS_ADAPTER_VERSION, bundle.Call<String>("getString", APS_ADAPTER_VERSION));
#else

            AdMobRequestExtras.Add (ADMOB_SLOTUUID_KEY, SlotUUID);
            AdMobRequestExtras.Add (ADMOB_WIDTH_KEY, BannerAdWidth.ToString());
            AdMobRequestExtras.Add (ADMOB_HEIGHT_KEY, BannerAdHeight.ToString());

#endif

            return AdMobRequestExtras;
        }

        private Dictionary<String, String> createExtras()
        {
            Dictionary<String, String> extrasMap = new Dictionary<string, string>();
            if (CCPAPrivacy != null)
            {
                if (CCPAPrivacy.TryGetValue (APS_ADMOB_CONST_CCPA_APS_PRIVACY, out string ccpa_aps_privacy))
                {
                    extrasMap.Add (APS_ADMOB_CONST_CCPA_APS_PRIVACY, ccpa_aps_privacy);
                }
                if (CCPAPrivacy.TryGetValue (APS_ADMOB_CONST_CCPA_US_PRIVACY, out string ccpa_us_privacy))
                {
                    extrasMap.Add (APS_ADMOB_CONST_CCPA_US_PRIVACY, ccpa_us_privacy);
                }

            }
            return extrasMap;

        }


        public class Builder
        {
            public Builder()
            {
                this.SlotUUID = null;
                this.AdHeight = 0;
                this.AdWidth = 0;
                this.CCPAPrivacy = new Dictionary<string, string>();
            }

            internal string SlotUUID { get; private set; }
            internal int AdHeight { get; private set; }
            internal int AdWidth { get; private set; }
            internal Dictionary<string, string> CCPAPrivacy { get; private set; }

            public Builder AddSlotUUID(string SlotUUID)
            {
                this.SlotUUID = SlotUUID;
                return this;
            }

            public Builder AddHeight(int AdHeight)
            {
                this.AdHeight = AdHeight;
                return this;
            }

            public Builder AddWidth(int AdWidth)
            {
                this.AdWidth = AdWidth;
                return this;
            }

            public Builder AddCCPAPrivacy(Dictionary<string, string> CCPAPrivacy)
            {
                this.CCPAPrivacy = CCPAPrivacy;
                return this;
            }

            public APSAdRequestExtra Build()
            {
                return new APSAdRequestExtra(this);
            }
        }
    }

}


