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
using System.Device.Location;
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Shell;
using System.Windows.Automation.Peers;

namespace GRTLookup
{
    public partial class StopView : PhoneApplicationPage
    {
        
        public StopView()
        {
            InitializeComponent();
            DataContext = App.ViewModel.StopPageModel;

            LayoutRoot.BindingValidationError += new EventHandler<ValidationErrorEventArgs>(LayoutRoot_BindingValidationError);
            this.Loaded += new RoutedEventHandler(StopView_Loaded);
            if (App.Settings.IsDevMode)
            {
                App.ViewModel.StopPageModel.StartScheduleRequest();
            }
            locationMap.Loaded += new RoutedEventHandler(locationMap_Loaded);
            favsManager();

            locationMap.ZoomLevel = 15;
            GeoCoordinate kwCenter = new GeoCoordinate();
            kwCenter.Latitude = 43.47273;
            kwCenter.Longitude = -80.541218;
            locationMap.Center = kwCenter;
        }

        void StopView_Loaded(object sender, RoutedEventArgs e)
        {
            timeBox.BindingValidationError +=new EventHandler<ValidationErrorEventArgs>(LayoutRoot_BindingValidationError);
        }

        void LayoutRoot_BindingValidationError(object sender, ValidationErrorEventArgs e)
        {
            MessageBox.Show(e.Error.ErrorContent.ToString());
        }

        void locationMap_Loaded(object sender, RoutedEventArgs e)
        {
            locationMap.Center = App.ViewModel.StopPageModel.CurrentStop.Location;
        }

        private void textStopAppbarButton_Click(object sender, EventArgs e)
        {
            SmsComposeTask smsComposeTask = new SmsComposeTask();

            smsComposeTask.To = App.GRT_NUM;
            smsComposeTask.Body = App.ViewModel.StopPageModel.CurrentStop.StopId.ToString();

            smsComposeTask.Show();
        }

        private void favsManager()
        {
            if (!App.ViewModel.StopPageModel.CurrentStop.IsFav)
            {
                ((ApplicationBarIconButton)ApplicationBar.Buttons[1]).IconUri = new Uri("/Images/appbar.favs.addto.rest.png", UriKind.Relative);
                ((ApplicationBarIconButton)ApplicationBar.Buttons[1]).Text = "add";
            }
            else
            {
                ((ApplicationBarIconButton)ApplicationBar.Buttons[1]).IconUri = new Uri("/Images/appbar.minus.rest.png", UriKind.Relative);
                ((ApplicationBarIconButton)ApplicationBar.Buttons[1]).Text = "remove";
            }
        }

        private void setFav()
        {
            if (App.ViewModel.StopPageModel.CurrentStop.IsFav)
            {
                App.ViewModel.Favourites.Remove(App.ViewModel.StopPageModel.CurrentStop.StopId);
                App.ViewModel.StopPageModel.CurrentStop.IsFav = false;
            }
            else
            {
                App.ViewModel.Favourites.Add(App.ViewModel.StopPageModel.CurrentStop.StopId);
                App.ViewModel.StopPageModel.CurrentStop.IsFav = true;
            }
        }

        private void addToFavsAppbarButton_Click(object sender, EventArgs e)
        {
            setFav();
            favsManager();

        }

        private void loadMoreButton_Click(object sender, RoutedEventArgs e)
        {
            App.ViewModel.StopPageModel.StartScheduleRequest();
        }


        private void chooseDate_Click(object sender, EventArgs e)
        {
            FlipDatePickerVisiblility();
 
        }

       

        private void datePicker_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            if (e.NewDateTime.HasValue)
            {
                App.ViewModel.StopPageModel.DateContext = e.NewDateTime.Value;
                App.ViewModel.StopPageModel.StopTimes.Clear();
            }
        }

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            FlipDatePickerVisiblility();
        }
        private void FlipDatePickerVisiblility()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                if (App.ViewModel.StopPageModel.ShowDatePicker)
                {
                    App.ViewModel.StopPageModel.ShowDatePicker = false;
                }
                else
                {
                    App.ViewModel.StopPageModel.ShowDatePicker = true;
                }
            });
        }
   }
}