﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Trains
{
    public class Graph
    {
        private int index = 0;
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
                    var weight = Convert.ToInt32(g.Substring(2, g.Length - 2));
                    AddVertex(start);
                    AddVertex(end);
                    AddArc(start, end, weight);
                }
            }
            catch
            {
                throw new Exception(@"输入值有误！");
            }
        }

        private void AddArc(string start, string end, int weight)
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
                VertexList.Add(new Vertex(index, vertex));
                index++;
            }
        }

        public bool VertexContains(string name)
        {
            return VertexList.Select(p => p.Name).Contains(name);
        }

        public int FindVertexIDByStation(string name)
        {
            var vertex = VertexList.Find(p => p.Name.Equals(name));
            if (vertex == null)
            {
                throw new ArgumentNullException(name, "该站点不存在！");
            }
            return vertex.ID;
        }

        public string FindStationByID(int id)
        {
            var vertex = VertexList.Find(p => p.ID.Equals(id));
            if (vertex == null)
            {
                throw new ArgumentNullException(id.ToString(), "该节点不存在！");
            }
            return vertex.Name;
        }

        public Arc GetRouteByStartEnd(string start, string end)
        {
            return ArcList.Find(ar => ar.Start.Equals(start) && ar.End.Equals(end));
        }

        public decimal GetDistance(int start, int end)
        {
            var startStation = FindStationByID(start);
            var endStation = FindStationByID(end);
            var arc = GetRouteByStartEnd(startStation, endStation);
            return arc == null ? Constant.INFINITE : arc.Weight;
        }
    }
}
