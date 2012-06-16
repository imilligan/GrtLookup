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

namespace GRTLookup.ViewModel
{
    public class SettingsViewModel
    {
        public bool IsDevMode
        {
            get
            {
                return App.Settings.IsDevMode;
            }
            set
            {
                App.Settings.IsDevMode = value;
                NotifyPropertyChanged("IsDevMode");
            }
        }
        public string Url
        {
            get
            {
                return App.Settings.Url;
            }
            set
            {
                App.Settings.Url = value;
                NotifyPropertyChanged("Url");
            }
        }
        public string ContactNumber
        {
            get
            {
                return App.Settings.ContactNumber;
            }
            set
            {
                App.Settings.ContactNumber = value;
                NotifyPropertyChanged("ContactNumber");
            }
        }
        public bool UseLocationServices
        {
            get
            {
                return App.Settings.UseLocationServices;
            }
            set
            {
                App.Settings.UseLocationServices = value;
                NotifyPropertyChanged("UseLocationServices");
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
