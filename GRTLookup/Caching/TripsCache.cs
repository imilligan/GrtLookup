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
using System.Collections.Generic;
using GRTLookup.Model;
using System.Linq;
using RestSharp;

namespace GRTLookup.Caching
{
    public class TripsCache
    {


        private TripsCache()
        {
        }
        private static TripsCache instance;
        public static TripsCache Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TripsCache();
                }
                return instance;
            }
        }

        private Dictionary<string, Trip> _cache = new Dictionary<string,Trip>();
        public void GetTrips(IEnumerable<string> tripIds, Action callback)
        {
            List<string> notFoundTripIds = new List<string>();
            foreach (string tripId in tripIds)
            {
                if( !_cache.ContainsKey( tripId ) )
                {
                    notFoundTripIds.Add(tripId);
                }
            }
            if (notFoundTripIds.Count() > 0)
            {
                RestRequest request = new RestRequest();
                request.Resource = "trips";
                request.AddParameter("tripIds", ToCSV(notFoundTripIds));
                request.DateFormat = "HH:mm:ss";
                App.GrtApiClient.Execute<List<Trip>>(
                    request,
                    (callbackData) =>
                    {
                        foreach (var element in callbackData.Data)
                        {
                            if (_cache.ContainsKey(element.tripId))
                            {
                                _cache[element.tripId] = element;
                            }
                            else
                            {
                                _cache.Add(element.tripId, element);
                            }
                        }
                        callback.Invoke();
                    }
                );
            }
            else
            {
                callback.Invoke();
            }
         }

        internal Trip Get(string p)
        {
            Trip trip;
            if (_cache.TryGetValue(p, out trip))
            {
                return trip;
            }
            else
            {
                return new Trip();
            }
        }

        private string ToCSV(List<string> elements)
        {
            string elementsString = "";
            if (elements.Count() > 0)
            {
                elementsString = elements[0].ToString();
                for (int i = 1; i < elements.Count(); i++)
                {
                    elementsString += "," + elements[i].ToString();
                }
            }
            return elementsString;
        }
       
    }
}
