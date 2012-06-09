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
using System.IO.IsolatedStorage;
using System.Linq;
using System.Collections.Generic;

namespace GRTLookup
{
    public class AppSettings
    {
        // Our isolated storage settings
        IsolatedStorageSettings isolatedStore;

        // The isolated storage key names of our settings
        const string FavoritesKeyName = "Favorites";
        const string UrlKeyName = "Url";
        const string IsDevModeKeyName = "IsDevMode";
        const string ContactNumberKeyName = "ContactNumber";


        // The default value of our settings
        readonly string[] FavoritesDefault = new string[]{};
        const string UrlDefault = "http://107.21.124.170/grtlookup/index.php/";
        readonly bool IsDevModeDefault = false;
        readonly string ContactNumberDefault = "57555";

        /// <summary>
        ///  Constructor that gets the application settings.
        /// </summary>
        public AppSettings()
        {

                // Get the settings for this application.
                isolatedStore = IsolatedStorageSettings.ApplicationSettings;

        }

      
        public bool AddOrUpdateValue(string Key, Object value)
        {
            bool valueChanged = false;

            // If the key exists
            if (isolatedStore.Contains(Key))
            {
                // If the value has changed
                if (isolatedStore[Key] != value)
                {
                    // Store the new value
                    isolatedStore[Key] = value;
                    valueChanged = true;
                }
            }
            // Otherwise create the key.
            else
            {
                isolatedStore.Add(Key, value);
                valueChanged = true;
            }

            return valueChanged;
        }

        public Object GetValueOrDefault(string Key, Object defaultValue)
        {
            Object value;

            // If the key exists, retrieve the value.
            if (isolatedStore.Contains(Key))
            {
                value = isolatedStore[Key];
            }
            // Otherwise, use the default value.
            else
            {
                value = defaultValue;
            }

            return value;
        }

        public void Save()
        {
            isolatedStore.Save();
        }


        public string[] Favourites
        {
            get
            {
                return (string[])GetValueOrDefault(FavoritesKeyName, FavoritesDefault);
            }
            set
            {
                AddOrUpdateValue(FavoritesKeyName, value);
                Save();
            }
        }

        public string Url
        {
            get
            {
                return (string)GetValueOrDefault(UrlKeyName, UrlDefault);
            }
            set
            {
                AddOrUpdateValue(UrlKeyName, value);
                Save();
            }
        }

        public bool IsDevMode
        {
            get
            {
                return (bool)GetValueOrDefault(IsDevModeKeyName, IsDevModeDefault);
            }
            set
            {
                AddOrUpdateValue(IsDevModeKeyName, value);
                Save();
            }
        }

        public string ContactNumber
        {
            get
            {
                return (string)GetValueOrDefault(ContactNumberKeyName, ContactNumberDefault);
            }
            set
            {
                AddOrUpdateValue(ContactNumberKeyName, value);
                Save();
            }
        }
    }
}
