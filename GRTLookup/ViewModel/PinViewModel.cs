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
using GRTLookup.Model;
using System.ComponentModel;
using System.Device.Location;
using System.Windows.Media.Imaging;

namespace GRTLookup.ViewModel
{
    public class PinViewModel : INotifyPropertyChanged
    {
        private bool isFav;
        private bool isCustom;
        private ImageSource source;
        private Stop pinStop;
        private GeoCoordinate location;
        private string content;

        public PinViewModel(Stop stop)
        {
            pinStop = stop;
        }
        public PinViewModel(string content)
        {
            this.content = content;
        }

        private Action<object, MouseButtonEventArgs> onClick;

        public Action<object, MouseButtonEventArgs> OnClick{
            get
            {
                if( onClick == null )
                {
                    return delegate(object ob, MouseButtonEventArgs args)
                    { return; }; 

                }
                return onClick;
            }
            set
            {
                onClick = value;
            }
        }
        public Visibility IsFavVisibility
        {
            get
            {
                if (isFav)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }
        public Visibility IsCustomVisibility
        {
            get
            {
                if (isCustom)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }

        public bool IsFav
        {
            get
            {
                return isFav;
            }
            set
            {
                isFav = value;
                NotifyPropertyChanged("IsFav");
                NotifyPropertyChanged("IsFavVisibility");
            }
        }
        public bool IsCustom
        {
            get
            {
                return isCustom;
            }
            set
            {
                isCustom = value;
                NotifyPropertyChanged("IsCustom");
                NotifyPropertyChanged("IsCustomVisibility");
            }
        }
        public string Content
        {
            get
            {
                if (isCustom)
                {
                    return content;
                }
                else
                {
                    return pinStop.stop_id.ToString();
                }
            }
        }
        public ImageSource Source
        {
            get
            {
                if (source == null)
                {
                    source = new BitmapImage(new Uri(""));
                }
                return source;
            }
            set
            {
                source = value;
            }
        }
        public GeoCoordinate Location
        {
            get
            {
                if (isCustom)
                {
                    return location;
                }
                else
                {
                    return new GeoCoordinate(
                        pinStop.stop_lat,
                        pinStop.stop_lon
                        );
                }
            }
            set
            {
                location = value;
                NotifyPropertyChanged("Location");
            }
        }

        public long StopId {
            get
            {
                return pinStop.stop_id;
            }
        }
        public string StopName
        {
            get
            {
                return pinStop.stop_name;
            }
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
