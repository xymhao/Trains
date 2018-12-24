using System;
using System.Collections.Generic;
using System.Linq;

namespace Trains.Models
{
    public class Graph
    {
        int _index = 0;
        public List<Vertex> VertexList { get; set; } = new List<Vertex>();
        public List<Arc> ArcList { get; set; } = new List<Arc>();

        public Graph(string graphStr)
        {
            try
            {
                var graphArr = graphStr.Split(@",");
                foreach (var g in graphArr)
                {
                    var start = g[0].ToString();
                    var end = g[1].ToString();
                    var weight = Convert.ToDecimal(g.Substring(2, g.Length - 2));
                    AddVertex(start);
                    AddVertex(end);
                    AddArc(start, end, weight);
                }
            }
            catch
            {
                throw new Exception(@"输入有误");
            }
        }


        private void AddArc(string start, string end, decimal weight)
        {
            ArcList.ForEach(arc =>
            {
                if (arc.Start.Equals(start) && arc.End.Equals(end))
                {
                    throw new ArgumentException("添加了重复的边！");
                }
            });
            ArcList.Add(new Arc(start, end, weight));
        }

        private void AddVertex(string vertex)
        {
            if (!VertexContains(vertex))
            {
                VertexList.Add(new Vertex(_index, vertex));
                _index++;
            }
        }

        public bool VertexContains(string name)
        {
            return VertexList.Select(p => p.Name).Contains(name);
        }

    }


    public class Vertex
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public Vertex(int id, string name)
        {
            ID = id;
            Name = name;
        }

    }

    public class Arc
    {
        public string Start { get; set; }

        public string End { get; set; }

        public decimal Weight { get; set; }

        public Arc(string start, string end, decimal weight)
        {
            Start = start;
            End = end;
            Weight = weight;
        }
    }
}
