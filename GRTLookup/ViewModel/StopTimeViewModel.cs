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

namespace GRTLookup.ViewModel
{
    public class StopTimeViewModel
    {
        private StopTime element;
        private Trip trip;

        public StopTimeViewModel(StopTime element, Trip trip)
        {

            this.element = element;
            this.trip = trip;
        }

        public String ArrivalTime
        {
            get
            {
                return element.arrivalTimeExact(DateTime.Today).ToShortTimeString() + " - " + element.departureTimeExact(DateTime.Today).ToShortTimeString();
            }
        }
        public string RouteId
        {
            get
            {
                return trip.routeId;
            }
        }
        public string Headsign
        {
            get
            {
                return trip.tripHeadsign;
            }
        }

    }
}
