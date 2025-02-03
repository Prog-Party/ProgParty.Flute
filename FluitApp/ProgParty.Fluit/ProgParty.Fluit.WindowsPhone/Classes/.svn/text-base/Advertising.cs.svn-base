using Google.AdMob.Ads.WindowsPhone7.WPF;
using Microsoft.Advertising.Mobile.UI;

namespace Fluit.Classes
{
    public class Advertising
    {
        public static AdControl CreateAdd()
        {
            AdControl advertising = new AdControl(Constants.AdApplicationId, Constants.AdUnitId, true)
            {
                Width = 480,
                Height = 80,
                VerticalAlignment = System.Windows.VerticalAlignment.Bottom
            };
            if (GeoProvider.DeviceLocation.HasValue)
            {
                advertising.Latitude = GeoProvider.DeviceLocation.Value.Latitude;
                advertising.Longitude = GeoProvider.DeviceLocation.Value.Longitude;
            }
            return advertising;
        }

        public static BannerAd CreateAddMob()
        {
            BannerAd add = new BannerAd();
            add.AdUnitID = Constants.AddMobUnitId;
            if (GeoProvider.DeviceLocation.HasValue)
            {
                add.GpsLocation = GeoProvider.DeviceLocation;
            }
            return add;
        }
    }
}
