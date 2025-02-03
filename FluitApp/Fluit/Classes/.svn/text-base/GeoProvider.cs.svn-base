using System;
using System.Device.Location;
using Google.AdMob.Ads.WindowsPhone7;

namespace Fluit.Classes
{
    /// <summary>
    /// Provides a simple mechanism for getting the current GPS coordinates
    /// </summary>
    public class GeoProvider
    {
        static GeoProvider()
        {
            try
            {
                DeviceLocation = null;

                GeoCoordinateWatcher Provider = new GeoCoordinateWatcher();
                Provider.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(GeoProvider_PositionChanged);
                Provider.Start(true);
            }
            catch (Exception exx)
            {
            }
        }

        private static void GeoProvider_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            if (e.Position.Location.IsUnknown)
                DeviceLocation = null;
            else
                DeviceLocation = new GpsLocation
                {
                    Latitude = e.Position.Location.Longitude,
                    Longitude = e.Position.Location.Longitude,
                    Accuracy = e.Position.Location.HorizontalAccuracy
                };
        }

        /// <summary>
        /// Provides the current GPS coordinates, if available
        /// </summary>
        public static GpsLocation? DeviceLocation { get; private set; }
    }
}
