﻿using System;
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

namespace GRTLookup
{
    public partial class MainPage : PhoneApplicationPage
    {

        private GeoCoordinateWatcher geoWatch;
        private PinViewModel userPushpin;
        private Dictionary<long, Stop> stopsOnMap;
       
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(MainPage_Loaded);
            stopMap.Loaded += new RoutedEventHandler(stopMap_Loaded);
            App.ViewModel.ConstructTree(); // AllStops = CsvReader.readStopFile(); //getFileStream("stops.txt"));
            stopsOnMap = new Dictionary<long, Stop>();
            DataContext = App.ViewModel;

            stopMap.ZoomLevel = 15;
            GeoCoordinate kwCenter = new GeoCoordinate();
            kwCenter.Latitude = 43.47273;
            kwCenter.Longitude = -80.541218;
            stopMap.Center = kwCenter;
        }

        void stopMap_Loaded(object sender, RoutedEventArgs e)
        {
            stopMap.Hold += new EventHandler<System.Windows.Input.GestureEventArgs>(stopMap_Hold);
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (geoWatch == null)
            {
                geoWatch = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
                geoWatch.MovementThreshold = 20;
                geoWatch.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(geoWatch_StatusChanged);
                App.ViewModel.IsLoading = true;
                geoWatch.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(geoWatch_PositionChanged);
            }
            geoWatch.Start();

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
                Source = new BitmapImage(new Uri("/Images/appbar.compas.rest.png", UriKind.Relative))
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
                if (geoWatch.Permission == GeoPositionPermission.Denied)
                {
                    MessageBox.Show("Location services on your device are disabled.");
                }
                else
                {
                    MessageBox.Show("Location services on your device are not functioning.");
                }
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
            stopsOnMap.Clear();
            if (geoWatch.Status == GeoPositionStatus.Ready)
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
                userPushpin = new PinViewModel("me")
                {
                    IsCustom = true
                };
                stopMap.Center = geoWatch.Position.Location;
            }
            userPushpin.Location = geoWatch.Position.Location;
            App.ViewModel.SearchPins.Add(userPushpin);
        }

        private void AddNearPoints(GeoCoordinate location)
        {

            IEnumerable<Stop> nearStops = App.ViewModel.NearbyStops(location, 10);
            foreach (Stop stop in nearStops)
            {
                if (!stopsOnMap.ContainsKey(stop.stop_id))
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
            if (stopsOnMap.ContainsKey(stop.stop_id))
            {
                stopsOnMap[stop.stop_id] = stop;
            }
            else
            {
                stopsOnMap.Add(stop.stop_id, stop);
            }
        }
        #endregion        

        
    }
}