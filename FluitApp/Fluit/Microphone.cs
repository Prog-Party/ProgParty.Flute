using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Fluit
{
    public class Microphone
    {
        public const int MinDecibelToPlay = -23;

        static readonly Microphone instance=new Microphone();

        Microsoft.Xna.Framework.Audio.Microphone microphone = Microsoft.Xna.Framework.Audio.Microphone.Default;
        byte[] buffer;
        MemoryStream stream = new MemoryStream();
        SoundEffect sound;

        public delegate void ChangeEventHandler(double counter);
        public event EventHandler TurnSoundOn;
        public event EventHandler TurnSoundOff;
        public event ChangeEventHandler ChangeSound;

        private bool _isOn;

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Microphone()
        {
        }

        private int decibelIsSame = 0;
        private double prevDecibel = 0;
        
        Microphone()
        {
            // Timer to simulate the XNA Game Studio game loop (Microphone is from XNA Game Studio)
            DispatcherTimer dt3 = new DispatcherTimer();
            dt3.Interval = TimeSpan.FromMilliseconds(33);
            dt3.Tick += delegate { try { FrameworkDispatcher.Update(); } catch { } };
            dt3.Start();
            DispatcherTimer dt2 = new DispatcherTimer();
            dt2.Interval = TimeSpan.FromMilliseconds(1000);
            dt2.Tick += delegate
            {
                try
                {
                    double decibel = GetVolume();

                    RestartMicrophoneIfNotStarted(decibel);

                    stream.Position = 0;

                    bool newIsOn = decibel > MinDecibelToPlay;
                    if(_isOn != newIsOn)
                    {
                        if (newIsOn)
                            TurnSoundOn(this, null);
                        else
                            TurnSoundOff(this, null);
                        _isOn = newIsOn;
                    }
                }
                catch (Exception e)
                {
                }
            };
            dt2.Start();

            DispatcherTimer dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromMilliseconds(120);
            dt.Tick += delegate
            {
                try
                {
                    double decibel = GetVolume();

                    //bool newIsOn = decibel > MinDecibelToPlay;
                    //if (_isOn != newIsOn)
                    //{
                    //    if (newIsOn)
                    //        TurnSoundOn(this, null);
                    //    _isOn = newIsOn;
                    //}

                    ChangeSound(decibel);
                }
                catch (Exception e)
                {
                }
            };
            dt.Start();
        }

        private double GetVolume()
        {
            List<byte> bytes = stream.ToArray().ToList();
            byte[] bbytes = bytes.ToArray();
            double sum = 0;
            for (var i = 0; i < bbytes.Length; i = i + 2)
            {
                double sample = BitConverter.ToInt16(bbytes, i) / 32768.0;
                sum += (sample * sample);
            }
            double rms = Math.Sqrt(sum / bbytes.Length);
            return 20 * Math.Log10(rms);
        }

        private void RestartMicrophoneIfNotStarted(double decibel)
        {
            if (decibel == prevDecibel)
            {
                decibelIsSame++;

                if(decibelIsSame > 5)
                {
                    microphone.Stop();
                    microphone = Microsoft.Xna.Framework.Audio.Microphone.Default;
                    microphone.BufferReady += new EventHandler<EventArgs>(BufferReady);
                    microphone.Start();
                }
            }
            else
            {
                decibelIsSame = 0;
            }
            prevDecibel = decibel;
        }

        public static Microphone Instance
        {
            get
            {
                return instance;
            }
        }

        private void BufferReady(object sender, EventArgs e)
        {
            microphone.GetData(buffer);

            stream.Write(buffer, 0, buffer.Length);
        }

        public void Record()
        {
            microphone.BufferDuration = TimeSpan.FromMilliseconds(100);
            buffer = new byte[microphone.GetSampleSizeInBytes(microphone.BufferDuration)];
            microphone.BufferReady += new EventHandler<EventArgs>(BufferReady);
            microphone.Start();
        }

        public void Stop()
        {
            if (microphone.State == MicrophoneState.Started)
            {
                microphone.Stop();
            }
        }

        public void Play()
        {
            sound = new SoundEffect(stream.ToArray(), microphone.SampleRate, AudioChannels.Mono);
            sound.Play();
        }
    }
}
