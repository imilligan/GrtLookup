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

namespace GRTLookup.Model
{
    public class Response<T> where T : new()
    {
        public List<T> content { get; set; }
        public string error { get; set; }
        public bool hasError { get; set; }
        public bool hasMore { get; set; }
        public int pageSize { get; set; }
    }
}
