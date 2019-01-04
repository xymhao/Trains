using System;
using System.Collections.Generic;
using System.Text;

namespace Trains
{
    public class Arc
    {
        public string Start { get; }

        public string End { get; }

        public decimal Weight { get; }

        public Arc(string start, string end, decimal weight)
        {
            Start = start;
            End = end;
            Weight = weight;
        }
    }
}
