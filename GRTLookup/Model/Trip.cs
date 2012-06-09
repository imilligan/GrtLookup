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
    public class Trip
    {
        public string tripId { get; set; }
        public string blockId { get; set; }
        public string routeId { get; set; }
        public string tripHeadsign { get; set; }
        public string serviceId { get; set; }
        public string shapeId { get; set; }
    }
}
