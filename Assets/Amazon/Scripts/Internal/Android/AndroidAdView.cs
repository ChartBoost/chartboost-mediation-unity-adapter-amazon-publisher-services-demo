using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AmazonAds.Android {
    public class AndroidAdView : IAdView {
        private AndroidJavaObject dtbAdView = null;
        private static readonly AndroidJavaClass SDKUtilitiesClass = new AndroidJavaClass(AmazonConstants.sdkUtilitiesClass);
        UnityEngine.AndroidJavaClass playerClass;
        UnityEngine.AndroidJavaObject currentActivityObject;

        public AndroidAdView (APSAdDelegate delegates) {
            playerClass = new UnityEngine.AndroidJavaClass(AmazonConstants.unityPlayerClass);
            currentActivityObject = playerClass.GetStatic<UnityEngine.AndroidJavaObject> ("currentActivity");

            DTBAdBannerListener adBannerListener = new DTBAdBannerListener();
            adBannerListener.adDelegate = delegates;
            dtbAdView = new AndroidJavaObject(AmazonConstants.dtbAdViewClass, currentActivityObject, adBannerListener);
        }

        public void Dispose() {
            playerClass.Dispose();
            currentActivityObject.Dispose();
            dtbAdView.Dispose();
        }

        public override void FetchAd (AdResponse adResponse) {
            if (dtbAdView != null) {
                AndroidJavaObject response = adResponse.GetAndroidResponseObject();
                string bidInfo = SDKUtilitiesClass.CallStatic<string>("getBidInfo", response);
                dtbAdView.Call("fetchAd", bidInfo);
            }
        }
    }
}