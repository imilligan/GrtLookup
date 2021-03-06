﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using GRTLookup.Model;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;
using System.Collections;
using System.Device.Location;
using Microsoft.Phone.Controls.Maps;
using System.IO.IsolatedStorage;
using System.IO;
using GRTLookup.Model.KDTree;

namespace GRTLookup.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {

        private static readonly int getNearestNum = 10;
        private bool isLoading;
        public bool IsLoading
        {
            get
            {
                return isLoading;
            }
            set
            {
                isLoading = value;
                NotifyPropertyChanged("IsLoading");
            }
        }
        private Dictionary<long, Stop> stopsOnMap;
        public Dictionary<long, Stop> StopsOnMap
        {
           get
            {
                if (stopsOnMap == null)
                {
                    stopsOnMap = new Dictionary<long, Stop>();
                }
                return stopsOnMap;
            } 
        }

        private SettingsViewModel settingViewModel;
        public SettingsViewModel SettingViewModel
        {
            get
            {
                if (settingViewModel == null)
                {
                    settingViewModel = new SettingsViewModel();
                }
                return settingViewModel;
            }
        }

        private StopViewModel stopPageModel;
        public StopViewModel StopPageModel
        {
            get
            {
                if (stopPageModel == null)
                {
                    stopPageModel = new StopViewModel();
                }
                return stopPageModel;
            }
        }
        private ObservableCollection<PinViewModel> pins;
        public ObservableCollection<PinViewModel> Pins
        {
            get
            {
                if (pins == null)
                {
                    pins = new ObservableCollection<PinViewModel>();
                }
                return pins;
            }
        }
        private ObservableCollection<PinViewModel> searchPins;

        public ObservableCollection<PinViewModel> SearchPins
        {
            get
            {
                if (searchPins == null)
                {
                    searchPins = new ObservableCollection<PinViewModel>();
                }
                return searchPins;
            }
        }
        private KDTree rootNode;
        public KDTree RootNode
        {
            get
            {
                return rootNode;
            }
        }

        private List<long> favourites;

        public List<long> Favourites
        {
            get
            {
                if (favourites == null)
                {
                    favourites = new List<long>();

                    foreach (string stopString in App.Settings.Favourites)
                    {
                        long stopId;
                        if( long.TryParse( stopString, out stopId ) ){
                            favourites.Add( stopId );
                        }
                    }                   
                }
                return favourites;
            }            
        }

        public void SetCurrentStop(string id)
        {
            stopPageModel = new StopViewModel()
            {
                CurrentStop = (from stop in App.ViewModel.Pins where stop.StopId.ToString() == id select stop).FirstOrDefault()
            };
        }

        public void SaveFavourites()
        {
            if (favourites == null)
            {
                return;
            }
            App.Settings.Favourites = favourites.Select((id) => id.ToString()).ToArray();
            
        }
        internal IEnumerable<Stop> NearbyStops(GeoCoordinate geoCoordinate, int k)
        {
            return rootNode.nearest(new double[] { geoCoordinate.Latitude, geoCoordinate.Longitude }, k).Cast<Stop>();
        }


        internal void ConstructTree()
        {
            CsvReader.readStopFile(out rootNode);
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        

    }
}
