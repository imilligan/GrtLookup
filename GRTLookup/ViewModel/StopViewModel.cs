using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections.ObjectModel;
using GRTLookup.Model;
using RestSharp;
using System.Collections.Generic;
using GRTLookup.Caching;
using System.Linq;
using System.Windows.Threading;
using System.Windows.Navigation;

namespace GRTLookup.ViewModel
{
    public class StopViewModel : INotifyPropertyChanged
    {
        private static readonly string Today = "Viewing: ";
        private int page = 1;
        private DateTime dateContext = DateTime.Now;
        private bool isCurDay = true;
        private ObservableCollection<StopTimeViewModel> stopTimes;
        private PinViewModel currentStop;
        private bool showDatePicker = false;
        private bool isLoading = false;

        public DateTime DateContext
        {
            get
            {
                return dateContext;
            }
            set
            {
                isCurDay = false;
                dateContext = value;
                NotifyPropertyChanged("DateDisplay");
            }
        }
        public string DateDisplay
        {
            get
            {
                if (isCurDay)
                {
                    return Today + "today";
                }
                else
                {
                    return Today + dateContext.ToShortDateString();
                }
            }
        }

        public ObservableCollection<StopTimeViewModel> StopTimes
        {
            get
            {
                if (stopTimes == null)
                {
                    stopTimes = new ObservableCollection<StopTimeViewModel>();
                }
                return stopTimes;
            }
        }


        public PinViewModel CurrentStop
        {
            get
            {
                return currentStop;
            }
            set
            {
                currentStop = value;
                NotifyPropertyChanged("CurrentStop");
            }
        }

        public bool ShowDatePicker
        {
            get
            {
                return showDatePicker;
            }
            set
            {
                showDatePicker = value;
                NotifyPropertyChanged("ShowDatePicker");
            }
        }
       
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
        private bool showMoreButton = true;
        public bool ShowMoreButton
        {
            get
            {
                return showMoreButton;
            }
            set
            {
                showMoreButton = value;
                NotifyPropertyChanged("ShowMoreButton");
            }
        }
        private bool hasMore = true;
        public bool HasMore
        {
            get
            {
                return hasMore;
            }
            set
            {
                hasMore = value;
                NotifyPropertyChanged("HasMore");
                NotifyPropertyChanged("HasMoreVisibility");
            }
        }
        public Visibility HasMoreVisibility
        {
            get
            {
                if (hasMore)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }
        #region Requests
        public void StartScheduleRequest( )
        {

            hasMore = true;
            IsLoading = true;
            RestRequest request = new RestRequest();
            request.Resource = "stoptimes/get";
            request.AddParameter( "stopId", currentStop.StopId );
            request.AddParameter("pageNum", page);
            if (!isCurDay)
            {
                request.AddParameter("date", dateContext.ToString("Y-m-d"));
            }
            //ShowMoreButton = false;
            App.GrtApiClient.Execute<Response<StopTime>>(
                request,
                (callbackData) =>
                {
                    Deployment.Current.Dispatcher.BeginInvoke( () => {
                    if (callbackData.Data != null)
                    {
                        PopulateData(callbackData.Data);
                        IsLoading = false;
                    }
                    else
                    {
                        ShowMoreButton = true;
                    }
                    } );
                }
            );


        }

        private void PopulateData(Response<StopTime> callbackData)
        {

                List<StopTime> content = callbackData.content;
                HasMore = callbackData.hasMore;
                TripsCache.Instance.GetTrips(
                    content.Select((stopTime) => { return stopTime.tripId; }),
                    () =>
                    {
                        foreach (StopTime stopTime in content)
                        {
                            StopTimes.Add(
                                new StopTimeViewModel(stopTime, TripsCache.Instance.Get(stopTime.tripId))
                            );
                        }
                        ShowMoreButton = true;
                        page++;
                    }
                );

        }


        internal void LoadMore()
        {
            throw new NotImplementedException();
        }
        #endregion

        //{Binding IsNotLoading}" Visibility="{Binding HasMoreVisibility}

        #region INotifyPropertyChanged Members


        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
