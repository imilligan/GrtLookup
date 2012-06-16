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
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace GRTLookup
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            LayoutRoot.DataContext = App.ViewModel.SettingViewModel;
        }
        
        private void contactInput_SelectionChanged(object sender, RoutedEventArgs e)
        {
            App.Settings.ContactNumber = ((TextBox)sender).Text;
        }

        private void addToContacts_Click(object sender, RoutedEventArgs e)
        {
            SavePhoneNumberTask saveNumber = new SavePhoneNumberTask();
            saveNumber.PhoneNumber = App.Settings.ContactNumber;

            saveNumber.Show();
        }

        private void clearFavourites_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you wish to delete your favourites?", "Alert", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                App.ViewModel.Favourites.Clear();
                App.ViewModel.SaveFavourites();
                App.ViewModel.Pins.Clear();
                App.ViewModel.StopsOnMap.Clear();
            }
        }

        private void useLocation_Unchecked(object sender, RoutedEventArgs e)
        {
            App.Settings.UseLocationServices = false;
        }

        private void useLocation_Checked(object sender, RoutedEventArgs e)
        {
            App.Settings.UseLocationServices = true;
        }

        private void locationServicesLink_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show( 
                @"This app uses a locally stored file of Grand River Transit stop data to retrieve and display relevant information.  GRT Lookup also uses Windows Phone Location Services to find an approximation of your location, so that it may find the closest bus stops and relevant stop numbers.  Location data is only used locally, and no information is sent to any remote server.
You may also enable or disable location services for your device in the Settings tool.",
                  "Privacy Statement", MessageBoxButton.OK);
        }


    }
}