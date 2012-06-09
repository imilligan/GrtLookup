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
using RestSharp;
using RestSharp.Deserializers;

namespace GRTLookup.WebClient
{
    public class GrtApi
    {

        public void Execute<T>(RestRequest request, Action<RestResponse<T>> callback ) where T : new()
        {
            var client = new RestClient();
            client.BaseUrl = App.Settings.Url;
            client.AddHandler("application/json", new JsonDeserializer());
            var response = client.ExecuteAsync<T>(request, callback);
            
        }
    }
}
