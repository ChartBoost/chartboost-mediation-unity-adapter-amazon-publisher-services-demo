namespace AmazonAds {
    public class APSVideoAdRequest : AdRequest {
        public APSVideoAdRequest (int width, int height, string uid ,AdNetworkInfo adNetworkInfo ) :base(adNetworkInfo) {
            AdSize.Video size = new AdSize.Video (width, height, uid);
            client.SetSizes (size.GetInstance ());
        }
    }
}