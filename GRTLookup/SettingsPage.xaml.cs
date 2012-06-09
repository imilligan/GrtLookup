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
            if (MessageBox.Show("Alert", "Are you sure you wish to delete your favourites?", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                App.ViewModel.Favourites.Clear();
                App.ViewModel.SaveFavourites();
                App.ViewModel.Pins.Clear();
                App.ViewModel.StopsOnMap.Clear();
            }
        }


    }
}