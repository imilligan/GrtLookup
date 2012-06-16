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
using GRTLookup.Model;
using System.IO.IsolatedStorage;
using System.IO;
using Microsoft.Phone.Controls.Maps;
using System.Device.Location;
using System.Windows.Media.Imaging;
using GRTLookup.ViewModel;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Shell;

namespace GRTLookup
{
    public partial class MainPage : PhoneApplicationPage
    {

        private GeoCoordinateWatcher geoWatch;
        private PinViewModel userPushpin;
       
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(MainPage_Loaded);
            stopMap.Loaded += new RoutedEventHandler(stopMap_Loaded);
            App.ViewModel.ConstructTree(); 
            DataContext = App.ViewModel;

            stopMap.ZoomLevel = 15;
            GeoCoordinate kwCenter = new GeoCoordinate();
            kwCenter.Latitude = 43.47273;
            kwCenter.Longitude = -80.541218;
            stopMap.Center = kwCenter;

        }
        void MainPage_Loaded(object sender, RoutedEventArgs e) {
            
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (NavigationContext.QueryString.ContainsKey("StopId"))
            {
                SmsComposeTask smsComposeTask = new SmsComposeTask();

                smsComposeTask.To = App.Settings.ContactNumber;
                smsComposeTask.Body = NavigationContext.QueryString["StopId"];

                smsComposeTask.Show();
            }
            if (App.Settings.IsFirstLaunch)
            {
                if (!(MessageBox.Show(
                    @"This app uses your phone's location services to help find local stops. At no time is any location data sent to a remote server.  Would you like to enable location services for GRT Lookup?
GRT Lookup's privacy policy can be found in the app settings",
  "Location Services", MessageBoxButton.OKCancel) == MessageBoxResult.OK))
                {
                    App.Settings.UseLocationServices = false;
                }
                App.Settings.IsFirstLaunch = false;
            }
            if (App.Settings.UseLocationServices)
            {
                ((ApplicationBarIconButton)ApplicationBar.Buttons[0]).IsEnabled = true;
                if (geoWatch == null)
                {
                    geoWatch = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
                    geoWatch.MovementThreshold = 20;
                    geoWatch.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(geoWatch_StatusChanged);
                    App.ViewModel.IsLoading = true;
                    geoWatch.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(geoWatch_PositionChanged);
                }
                geoWatch.Start();
                AddUserPushpin();
            }
            else if( !App.Settings.UseLocationServices )
            {
                ((ApplicationBarIconButton)ApplicationBar.Buttons[0]).IsEnabled = false;
                if (geoWatch != null)
                {
                    geoWatch.Stop();
                }
                RemoveUserPushPin();
            }

        }
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (geoWatch != null)
            {
                geoWatch.PositionChanged -= new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(geoWatch_PositionChanged);
                geoWatch.StatusChanged -= new EventHandler<GeoPositionStatusChangedEventArgs>(geoWatch_StatusChanged);
                geoWatch.Stop();
            }


        }

        void stopMap_Loaded(object sender, RoutedEventArgs e)
        {
            stopMap.Hold += new EventHandler<System.Windows.Input.GestureEventArgs>(stopMap_Hold);
        }

        #region mapEvents
        void stopMap_Hold(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Point p = e.GetPosition(stopMap);
            GeoCoordinate geo = new GeoCoordinate();
            geo = stopMap.ViewportPointToLocation(p);
            PinViewModel pin = new PinViewModel("")
            {
                IsCustom = true,
                Location = geo,
                Source = new BitmapImage(new Uri("/Images/poi.rest.png", UriKind.Relative))
            };
            App.ViewModel.SearchPins.Add(pin);
        }

        void pin_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Pushpin pin = sender as Pushpin;
            long outId;
            if (long.TryParse(pin.Tag.ToString(), out outId))
            {
                App.ViewModel.SetCurrentStop(outId.ToString());
                NavigationService.Navigate(new Uri("/StopView.xaml", UriKind.Relative));
            }
        }
        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(stopMap);
            GeoCoordinate geo = new GeoCoordinate();
            geo = stopMap.ViewportPointToLocation(p);
            AddNearPoints(geo);
        }


        #endregion

        #region geoWatch
        void geoWatch_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            if (geoWatch.Status == GeoPositionStatus.Ready)
            {
                AddUserPushpin();
            }

        }

        void geoWatch_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            if (geoWatch.Status == GeoPositionStatus.Disabled)
            {
                MessageBox.Show("Location services on your device are not functioning.");
                geoWatch.StatusChanged -= new EventHandler<GeoPositionStatusChangedEventArgs>(geoWatch_StatusChanged);
                geoWatch.Stop();
            }
            else if (geoWatch.Status == GeoPositionStatus.Initializing)
            {
                return;
            }
            else if (geoWatch.Status == GeoPositionStatus.NoData)
            {
                MessageBox.Show("Location data is not available");
            }
            else if (geoWatch.Status == GeoPositionStatus.Ready)
            {
                App.ViewModel.IsLoading = false;
                AddUserPushpin();
            }
        }
        #endregion

        #region appBarButtons


        private void searchNearby_Click(object sender, EventArgs e)
        {
            if (geoWatch == null || geoWatch.Status != GeoPositionStatus.Ready)
            {
                MessageBox.Show("No location data");
                return;
            }
            AddNearPoints(geoWatch.Position.Location);
        }

        private void removePoints_Click(object sender, EventArgs e)
        {
            App.ViewModel.Pins.Clear();
            App.ViewModel.SearchPins.Clear();
            App.ViewModel.StopsOnMap.Clear();
            if ( App.Settings.UseLocationServices && geoWatch.Status == GeoPositionStatus.Ready )
            {
                AddUserPushpin();
            }
        }

        private void settingsMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }

        #endregion

        #region utils
        private void AddUserPushpin()
        {
            if (userPushpin == null)
            {
                userPushpin = new PinViewModel( string.Empty )
                {
                    IsCustom = true,
                    Source = new BitmapImage(new Uri("/Images/appbar.compas.rest.png", UriKind.Relative ))
                };
                stopMap.Center = geoWatch.Position.Location;
            }
            userPushpin.Location = geoWatch.Position.Location;
            if (!App.ViewModel.SearchPins.Contains(userPushpin))
            {
                App.ViewModel.SearchPins.Add(userPushpin);
            }
            
        }
        private void RemoveUserPushPin()
        {
            if (userPushpin != null)
            {
                App.ViewModel.SearchPins.Remove(userPushpin);
            }
        }

        private void AddNearPoints(GeoCoordinate location)
        {

            IEnumerable<Stop> nearStops = App.ViewModel.NearbyStops(location, 10);
            foreach (Stop stop in nearStops)
            {
                if (!App.ViewModel.StopsOnMap.ContainsKey(stop.stop_id))
                {
                    AddStopToMap(stop);
                }
            }
        }

        private void AddStopToMap(Stop stop)
        {
            PinViewModel pin = new PinViewModel(stop)
            {
                IsFav = App.ViewModel.Favourites.Contains(stop.stop_id)
            };
            App.ViewModel.Pins.Add(pin);
            if (App.ViewModel.StopsOnMap.ContainsKey(stop.stop_id))
            {
                App.ViewModel.StopsOnMap[stop.stop_id] = stop;
            }
            else
            {
                App.ViewModel.StopsOnMap.Add(stop.stop_id, stop);
            }
        }
        #endregion        

        private void info_Click(object sender, EventArgs e)
        {
			NavigationService.Navigate(new Uri("/InfoPage.xaml", UriKind.Relative));
        }

        
    }
}