using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Fluit.Classes;
using Color = System.Windows.Media.Color;

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

        public void AirHoleMouseLeave()
        {
            AirHolePressed = false;
            AirHoleEllipse.Fill = new SolidColorBrush(Colors.Transparent);

            int holesPressed = IsolatedStorage.ReadIntFromFile(IsolatedStorage.FileName.HolesPressed);
            if(MainPage.IsPlaying)
                IsolatedStorage.SaveIntToFile(IsolatedStorage.FileName.HolesPressed, holesPressed + 1);
        }

        public void AirHoleMouseEnter()
        {
            AirHolePressed = true;
            AirHoleEllipse.Fill = new SolidColorBrush(Color.FromArgb(0x88, 0x33, 0x33, 0x33));
        }
    }
}
