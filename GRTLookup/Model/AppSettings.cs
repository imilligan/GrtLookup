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

namespace GRTLookup.Model
{
    public class AppSettings
    {
        public AppSettings()
        {
        }

        private string url = App.APP_URL;
        private bool isDevMode = false;

        public string Url
        {
            get
            {
                return url;
            }
            set
            {
                url = value;
            }
        }
        public bool IsDevMode
        {
            get
            {
                return isDevMode;
            }
            set
            {
                isDevMode = value;
            }
        }
    }
}
