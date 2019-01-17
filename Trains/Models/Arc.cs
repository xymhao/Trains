using System;
using System.Collections.Generic;
using System.Text;

namespace Trains
{
    public class Arc
    {
        public string Start { get; }

        public string End { get; }

        public int Weight { get; }

        public Arc(string start, string end, int weight)
        {
            Start = start;
            End = end;
            Weight = weight;
        }
    }
}
