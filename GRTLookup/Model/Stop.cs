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
    public class Stop
    {
        public double stop_lat;
        public long zone_id;
        public double stop_lon;
        public long stop_id;
        public string stop_name;
        public int location_type;

        public double StopLat
        {
            get
            {
                return stop_lat;
            }
        }
        public double ZoneId
        {
            get
            {
                return zone_id;
            }
        }
        public double StopLon
        {
            get
            {
                return StopLon;
            }
        }
        public long StopId
        {
            get
            {
                return stop_id;
            }
        }
        public string StopName
        {
            get
            {
                return stop_name;
            }
        }
        public int LocationType
        {
            get
            {
                return location_type;
            }
        }


     }
}
