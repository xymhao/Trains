using System;
using System.Collections.Generic;
using System.Text;

namespace Trains
{
    public class Vertex
    {
        public int ID { get; }

        public string Name { get; }

        public Vertex(int id, string name)
        {
            ID = id;
            Name = name;
        }
    }
}
