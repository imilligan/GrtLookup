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
using System.IO;
using System.Collections.Generic;
using System.Windows.Resources;
using System.Collections.ObjectModel;

namespace GRTLookup.Model
{
    public class CsvReader
    {
        public static Stop readStopLine(string line)
        {
            var split = ((String)line).Split(',');
            if (split.Length != 6)
            {
                return null;
            }
            Stop retStop = new Stop()
            {
                stop_lat = double.Parse(split[0]),
                zone_id = tryParseLong(split[1]),
                stop_lon = double.Parse(split[2]),
                stop_id = tryParseLong(split[3]),
                stop_name = split[4],
                location_type = tryParseInt(split[5])

            };
            return retStop;
            
        }
        public static ObservableCollection<Stop> readStopFile()
        {
            Stream reader = getFileStream("stops.txt");
            ObservableCollection<Stop> retStops = new ObservableCollection<Stop>();
            StreamReader streamReader = new StreamReader(reader);
            while (!streamReader.EndOfStream)
            {
                Stop stop = readStopLine(streamReader.ReadLine());
                if (stop != null)
                {
                    retStops.Add(stop);
                }
            }
            return retStops;
        }

        private static long tryParseLong(string inString){
            if (inString == null || inString == (""))
            {
                return 0;
            }
            else if (inString.Contains("_merged_"))
            {
                return long.Parse(inString.Substring(0, inString.IndexOf('_')));
            }
            else return long.Parse(inString);
        }
        private static int tryParseInt(string inString)
        {
            if (inString == null || inString == (""))
            {
                return 0;
            }
            else return int.Parse(inString);
        }

        private static System.IO.Stream getFileStream(string fileName)
        {
            try
            {
                ///<summary>
                ///get the user Store and then open the file in the store
                ///finally read the content to the file and return it
                ///</summary>
                return Application.GetResourceStream(new Uri(fileName, UriKind.Relative)).Stream;

            }
            catch (Exception e)
            {
                //IsolatedStorageException catch if File cant be opened
                MessageBox.Show("File Read Error");
                return null;
            }

        }

        internal static void readStopFile(out KDTree.KDTree rootNode)
        {
            rootNode = new KDTree.KDTree(2);
            Stream reader = getFileStream("stops.txt");
            StreamReader streamReader = new StreamReader(reader);
            while (!streamReader.EndOfStream)
            {
                Stop stop = readStopLine(streamReader.ReadLine());
                if (stop != null)
                {
                    rootNode.insert(new double[] { stop.stop_lat, stop.stop_lon }, stop);
                }
            }
        }
    }
}
