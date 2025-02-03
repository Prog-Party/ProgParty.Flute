using System;
using System.Windows;
using Microsoft.Phone.Marketplace;

namespace Fluit.Classes
{
    public class Constants
    {
        public const String AdUnitId = "10032122";//real adunit
        public const String AdApplicationId = "c806abcf-fa5b-4ecd-8e53-ac749770a9d5";//real app id
        //public const String AdUnitId = "Image480_80";
        //public const String AdApplicationId = "test_client";

        public const String AddMobUnitId = "a14f662e58ea716";

        public const int MaxTrial = 100;

        private static bool? _isTrial;
        public static bool IsTrial
        {
            get
            {
                if (!_isTrial.HasValue)
                    _isTrial = new LicenseInformation().IsTrial();
                return _isTrial.Value;
            }
        }

        public static Visibility IsTrialVisibility
        {
            get
            {
                if (IsTrial)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }

        public static bool IsDebug
        {
            get { return false; }
        }

        public static Visibility IsDebugVisibility
        {
            get
            {
                if (IsDebug)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
        }
    }
}
