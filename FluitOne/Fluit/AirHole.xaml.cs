using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Fluit
{
    public partial class AirHole : UserControl
    {
        public static readonly DependencyProperty SoundValueProperty = DependencyProperty.Register("SoundValue", typeof(int), typeof(AirHole), new PropertyMetadata(1));
        public int SoundValue
        {
            get { return int.Parse(this.GetValue(SoundValueProperty).ToString()); }
            set { this.SetValue(SoundValueProperty, value); }
        }

        public delegate void SoundChangedHandler(bool isReleased, int soundValue);
        public event SoundChangedHandler SoundChanged;

        public bool AirHolePressed { get; set; }

        public AirHole()
        {
            InitializeComponent();
        }

        private void AirHoleMouseLeave(object sender, MouseEventArgs e)
        {
            AirHolePressed = false;
            OnSoundChanged(true);
        }

        private void AirHoleMouseEnter(object sender, MouseEventArgs e)
        {
            AirHolePressed = true;
            OnSoundChanged(false);
        }

        private void OnSoundChanged(bool isReleased)
        {
            if (SoundChanged != null)
                SoundChanged(isReleased, SoundValue);
        }
    }
}
