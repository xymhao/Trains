using System;
using System.Collections.Generic;
using System.Text;

namespace Trains
{
    //表结点（表示图的边）
    public class Route
    {
        public Route NextRoute { get; set; }

        public int Distance { get; }

        public Station Station { get; }

        public Route(Station station, int value)
        {
            Station = station;
            Distance = value;
        }

        public string GetDestinationStation()
        {
            return Station.Name;
        }

        public void SetNextRoute(Station vertex, int weight)
        {
            NextRoute = new Route(vertex, weight);
        }
    }
}
