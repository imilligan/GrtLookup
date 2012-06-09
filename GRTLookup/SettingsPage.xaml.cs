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

namespace GRTLookup
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            LayoutRoot.DataContext = App.ViewModel.SettingViewModel;
        }

        private void urlInput_SelectionChanged(object sender, RoutedEventArgs e)
        {
            App.Settings.Url = urlInput.Text;
        }

        private void urlSelector_Unchecked(object sender, RoutedEventArgs e)
        {
            App.Settings.IsDevMode = false;
        }

        private void urlSelector_Checked(object sender, RoutedEventArgs e)
        {
            App.Settings.IsDevMode = true;
        }


    }
}