using System;

namespace AmazonAds {
    public enum ApsAdNetwork {
        UNKNOWN,
        GOOGLE_AD_MANAGER,
        ADMOB,
        AD_GENERATION,
        UNITY_LEVELPLAY,
        MAX,
        NIMBUS,
        CUSTOM_MEDIATION,
        OTHER
    }
    
    public class AdNetworkInfo {
        private ApsAdNetwork adNetwork;

        public AdNetworkInfo(ApsAdNetwork apsAdNetwork) {
            if (apsAdNetwork == null) {
                adNetwork = ApsAdNetwork.UNKNOWN;
            }
            adNetwork = apsAdNetwork;
        }

        public String getAdNetworkName() {
            return adNetwork.ToString();
        }

        internal ApsAdNetwork getAdNetwork() {
            return adNetwork;
        }
    }
}