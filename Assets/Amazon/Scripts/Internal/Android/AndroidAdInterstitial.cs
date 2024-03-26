using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AmazonAds.Android {
    public class AndroidAdInterstitial : IAdInterstitial {
        private AndroidJavaObject dtbAdInterstitial = null;
        private static readonly AndroidJavaClass SDKUtilitiesClass = new AndroidJavaClass(AmazonConstants.sdkUtilitiesClass);

        UnityEngine.AndroidJavaClass playerClass;
        UnityEngine.AndroidJavaObject currentActivityObject;

        public AndroidAdInterstitial (APSAdDelegate delegates) {
            playerClass = new UnityEngine.AndroidJavaClass(AmazonConstants.unityPlayerClass);
            currentActivityObject = playerClass.GetStatic<UnityEngine.AndroidJavaObject> ("currentActivity");

            DTBAdInterstitialListener adInterstitialListener = new DTBAdInterstitialListener();
            adInterstitialListener.adDelegate = delegates;
            dtbAdInterstitial = new AndroidJavaObject(AmazonConstants.dtbAdInterstitialClass, currentActivityObject, adInterstitialListener);
        }

        public void Dispose() {
            playerClass.Dispose();
            currentActivityObject.Dispose();
            dtbAdInterstitial.Dispose();
        }

        public override void FetchAd (AdResponse adResponse) {
            if (dtbAdInterstitial != null) {
                AndroidJavaObject response = adResponse.GetAndroidResponseObject();
                string bidInfo = SDKUtilitiesClass.CallStatic<string>("getBidInfo", response);
                dtbAdInterstitial.Call("fetchAd", bidInfo);
            }
        }

        public override void Show () {
            if (dtbAdInterstitial != null) {
                dtbAdInterstitial.Call("show");
            }
        }
    }
}