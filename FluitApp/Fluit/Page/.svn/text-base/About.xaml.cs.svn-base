using System.Windows;
using Fluit.Classes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace Fluit.Page
{
    public partial class About : PhoneApplicationPage
    {
        public About()
        {
            InitializeComponent();
            AmountHolesPressed.Text = IsolatedStorage.ReadIntFromFile(IsolatedStorage.FileName.HolesPressed).ToString();
        }

        private void EmailClick(object sender, RoutedEventArgs e)
        {
            EmailComposeTask sendEmail = new EmailComposeTask()
            {
                To = "JensDennisBV@gmail.com",
                Subject = "Flute Instrument"
            };
            sendEmail.Show();
        }

        private void ReviewClick(object sender, RoutedEventArgs e)
        {
            new MarketplaceReviewTask().Show();
        }

        private void BuyAppClick(object sender, RoutedEventArgs e)
        {
            new MarketplaceDetailTask().Show();
        }
    }
}