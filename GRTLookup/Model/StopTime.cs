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
using RestSharp.Deserializers;

namespace GRTLookup.Model
{
    public class StopTime
    {
        public StopTime()
        {
        }
        public string tripId { get; set; }
        public string arrivalTime { get; set; }
        public string departureTime { get; set; }
        public string stopId { get; set; }
        public string stopSequence { get; set; }
        public string stopHeadsign { get; set; }
        public string pickupType { get; set; }
        public string dropOffType { get; set; }
        public string shapeDistTraveled { get; set; }
        public DateTime arrivalTimeExact( DateTime day ){
            TimeSpan span;
            string[] tryParseString;
            if (TimeSpan.TryParse(arrivalTime, out span))
            {
                day = day.Add(span); 
            }
            else
            {
                tryParseString = arrivalTime.Split(':');
                if (tryParseString.Length == 3)
                {
                    int hours = int.Parse(tryParseString[0]);
                    if (hours > 23)
                    {
                        day = day.AddDays(1);
                        day = day.AddHours(hours - 24);
                    }
                    day = day.AddMinutes(int.Parse(tryParseString[1]));
                    day = day.AddSeconds(int.Parse(tryParseString[2]));
                }
            }
            return day;
        }
        public DateTime departureTimeExact(DateTime day)
        {
            TimeSpan span;
            string [] tryParseString;
            if (TimeSpan.TryParse(departureTime, out span))
            {
                day = day.Add(span);
            }
            else 
            {
                tryParseString = departureTime.Split(':');
                if (tryParseString.Length == 3)
                {
                    int hours = int.Parse(tryParseString[0]);
                    if (hours > 23)
                    {
                        day = day.AddDays(1);
                        day = day.AddHours(hours - 24);
                    }
                    day = day.AddMinutes(int.Parse(tryParseString[1]));
                    day = day.AddSeconds(int.Parse(tryParseString[2]));
                }
            }
            return day;
        }
    }
}
