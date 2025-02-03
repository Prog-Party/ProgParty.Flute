using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Fluit.Classes;
using Fluit.Resources;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using com.mtiks.winmobile;

namespace Fluit
{
    public partial class MainPage : PhoneApplicationPage
    {
        Dictionary<AirHole, bool> airHoleCollisions = new Dictionary<AirHole, bool>();
        private const int AmountAirHoles = 4;
        private int SoundIndex = 0;
        public static bool IsPlaying = false;


        // Constructor
        public MainPage()
        {
            InitializeComponent();
            
            LittleWatson.CheckForPreviousException(true);

            InitializeAds();

            RateApplicationMessage();

            int holesToEndTrial = (Constants.MaxTrial - IsolatedStorage.ReadIntFromFile(IsolatedStorage.FileName.HolesPressed));
            if (holesToEndTrial <= 0)
                AmountHitsForTrial.Text = "0";
            else
                AmountHitsForTrial.Text = holesToEndTrial.ToString();
            Status.Text = AppResources.IsSilent;

            airHoleCollisions.Add(AirHole1, false);
            airHoleCollisions.Add(AirHole2, false);
            airHoleCollisions.Add(AirHole3, false);
            airHoleCollisions.Add(AirHole4, false);

            InitializeMicrophone();

            Touch.FrameReported += Touch_FrameReported;

            InitializeSoundEffects();

            SetSound();

            LittleWatson.CheckForPreviousException(true);
            FrameworkDispatcher.Update();
        }

        private void InitializeSoundEffects()
        {
            SoundEffect e1 = SoundEffect.FromStream(TitleContainer.OpenStream("Resources/Sounds/00_1.wav"));
            SoundEffectInstance ef1 = e1.CreateInstance();
            ef1.IsLooped = true;
            ef1.Volume = 1.0f;
            ef1.Play();
            ef1.Pause();
            _effects.Add(e1);
            _effects2.Add(ef1);
        }

        private void InitializeMicrophone()
        {
            Microphone m = Microphone.Instance;
            m.TurnSoundOn += TurnSoundOn;
            m.TurnSoundOff += TurnSoundOff;
            m.ChangeSound += ChangeSound;
            m.Record();
        }

        /// <summary>
        /// Check if the RateApplicationMessage needs to be shown
        /// </summary>
        private static void RateApplicationMessage()
        {
            int rateAppView = IsolatedStorage.ReadIntFromFile(IsolatedStorage.FileName.RateApp);

            if (rateAppView == 5)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show(AppResources.RateApplication, String.Empty, MessageBoxButton.OKCancel);
                if (messageBoxResult == MessageBoxResult.OK)
                    new MarketplaceReviewTask().Show();
            }
        }

        private void InitializeAds()
        {
            if (Constants.IsTrial)
            {
                String cultureName = Thread.CurrentThread.CurrentCulture.Name.ToLower();
                if(cultureName == "nl-nl" || cultureName == "nl")
                {
                    //activate addmob
                    Adds.Children.Add(Advertising.CreateAddMob());
                }
                else
                {
                    //Microsoft adds
                    Adds.Children.Add(Advertising.CreateAdd());
                }
            }
        }

        private List<SoundEffect> _effects = new List<SoundEffect>();
        private List<SoundEffectInstance> _effects2 = new List<SoundEffectInstance>();

        private void ChangeSound(double decibel)
        {
            decibel = Math.Round(decibel, 2);
            Status2.Text = decibel.ToString();
        }

        private void TurnSoundOff(object sender, EventArgs e)
        {
            Status.Text = AppResources.IsSilent;
            if(!Constants.IsDebug)
                _effects2[0].Pause();
            IsPlaying = false;
        }

        private void TurnSoundOn(object sender, EventArgs e)
        {
            Status.Text = AppResources.IsPlaying;
            _effects2[0].Resume();
            IsPlaying = true;
        }

        private void Touch_FrameReported(object sender, TouchFrameEventArgs e)
        {
            for (int airHoleNumber = 0; airHoleNumber < AmountAirHoles; airHoleNumber++)
                airHoleCollisions[airHoleCollisions.Keys.Skip(airHoleNumber).First()] = false;

            TouchPointCollection touchPointCollection;
            try
            {
                touchPointCollection = e.GetTouchPoints(FluitFrame);
            }
            catch (Exception)
            {
                return;
            }
            foreach (TouchPoint tp in touchPointCollection)
            {
                if (tp.Action == TouchAction.Up)
                    continue;

                for (int airHoleNumber = 0; airHoleNumber < AmountAirHoles; airHoleNumber++)
                    if (CollidesWith(tp, airHoleNumber))
                    {
                        if (Constants.IsTrial)
                        {
                            int amountHolesPressed = IsolatedStorage.ReadIntFromFile(IsolatedStorage.FileName.HolesPressed);
                            if ((amountHolesPressed <= Constants.MaxTrial))
                            {
                                airHoleCollisions[airHoleCollisions.Keys.Skip(airHoleNumber).First()] = true;
                                AmountHitsForTrial.Text =
                                    (Constants.MaxTrial - amountHolesPressed).
                                        ToString();
                            }
                            else
                            {
                                AmountHitsForTrial.Text = "0";
                                MessageBoxResult trialEndedMessage = MessageBox.Show(AppResources.TrialEnded, String.Empty, MessageBoxButton.OKCancel);
                                if (trialEndedMessage == MessageBoxResult.OK)
                                    new MarketplaceDetailTask().Show();
                            }
                        }
                        else
                        {
                            airHoleCollisions[airHoleCollisions.Keys.Skip(airHoleNumber).First()] = true;
                        }
                    }
            }

            SetAirHoles();
            SoundIndex = CalculateSoundIndex();
            SetSound();

            FrameworkDispatcher.Update();
        }

        private void SetSound()
        {
            Status3.Text = SoundIndex.ToString();

            int newSound = SoundIndex - 7;

            //b = a * (2 ^ (1/12))
            double multiplyFactor = Math.Pow(2.0, 1.0/12.0);

            double newPitch = 1;
            for(int i = 0; i < Math.Abs(newSound); i++)
            {
                if(newSound > 0)
                    newPitch *= multiplyFactor;
                else
                    newPitch /= multiplyFactor;
            }
            newPitch -= 1;

            Status3.Text = SoundIndex.ToString() + " - " + newPitch;
            _effects2[0].Pitch = (float)newPitch;
        }

        private int CalculateSoundIndex()
        {
            int count = 0;
            foreach (KeyValuePair<AirHole, bool> airHoleCollision in airHoleCollisions)
            {
                if (airHoleCollision.Key.AirHolePressed)
                {
                    count += airHoleCollision.Key.SoundValue;
                }
            }
            return count;
        }

        private void SetAirHoles()
        {
            foreach (KeyValuePair<AirHole, bool> airHoleCollision in airHoleCollisions)
            {
                if(airHoleCollision.Key.AirHolePressed != airHoleCollision.Value)
                {
                    //change the airholepressed
                    if(airHoleCollision.Value)
                        airHoleCollision.Key.AirHoleMouseEnter();
                    else
                        airHoleCollision.Key.AirHoleMouseLeave();
                }
            }
        }

        private bool CollidesWith(TouchPoint tp, int airholeNr)
        {
            airholeNr++;
            double airholeHeight = 90;
            double left = (230/2) - (airholeHeight / 2);
            double top = 10 + ((70+airholeHeight)*(airholeNr - 1));

            if(     tp.Position.X > (left - (airholeHeight/2))
                &&  tp.Position.X < (left + (airholeHeight*1.5))
                &&  
                tp.Position.Y > top 
                &&  tp.Position.Y < (top + airholeHeight))
                return true;//collision
            return false;//no collision
        }

        private void HelpClick(object sender, EventArgs e)
        {
            Dictionary<String, String> attributes = new Dictionary<string, string>();
            attributes.Add("Amount holes pressed", IsolatedStorage.ReadIntFromFile(IsolatedStorage.FileName.HolesPressed).ToString());
            mtiks.Instance.postEventAttributes("Open Help page", attributes);

            NavigationService.Navigate(new Uri("/Page/Help.xaml", UriKind.Relative));
        }

        private void InformationClick(object sender, EventArgs e)
        {
            Dictionary<String, String> attributes = new Dictionary<string, string>();
            attributes.Add("Amount holes pressed", IsolatedStorage.ReadIntFromFile(IsolatedStorage.FileName.HolesPressed).ToString());
            mtiks.Instance.postEventAttributes("Open Information page", attributes);

            NavigationService.Navigate(new Uri("/Page/About.xaml", UriKind.Relative));
        }

//        Flute instrument

//With flute you can play a real flute instrument. It feels like a real flute instrument because you need to blow to your phone to get sound out of it. 
    }
}