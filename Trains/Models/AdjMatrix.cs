using System;
using System.Collections.Generic;
using System.Text;

namespace Trains
{
    public class AdjMatrix
    {
        public Graph _graph;
        public decimal[,] _matrix { get; set; }

        public AdjMatrix(Graph graph)
        {
            _graph = graph;
            _matrix = new decimal[graph.VertexList.Count, graph.VertexList.Count];
            foreach (var g in graph.ArcList)
            {
                var i = graph.VertexList.Find(p => p.Name.Equals(g.Start)).ID;
                var j = graph.VertexList.Find(p => p.Name.Equals(g.End)).ID;
                _matrix[i, j] = Convert.ToDecimal(g.Weight);
            }
            for (var i = 0; i < graph.VertexList.Count; i++)
            {
                for (var j = 0; j < graph.VertexList.Count; j++)
                {
                    _matrix[i, j] = _matrix[i, j].Equals(0) ? Constant.INFINITE : _matrix[i, j];
                }
            }
        }

        public decimal GetDistance(string start, string end)
        {
            var i = _graph.VertexList.Find(p => p.Name.Equals(start));
            var j = _graph.VertexList.Find(p => p.Name.Equals(end));
            if (i == null)
            {
                throw new ArgumentNullException(start, "输入站点不存在");
            }
            if (j == null)
            {
                throw new ArgumentNullException(end, "输入站点不存在");
            }
            return _matrix[i.ID, j.ID];
        }

        public bool GetRoutResult(string route, Action<string> doAction)
        {
            var stops = route.Split("-");
            decimal distance = 0;
            for (var i = 0; i < stops.Length - 1; i++)
            {
                var value = GetDistance(stops[i], stops[i + 1]);
                if (value == 0 || value.Equals(Constant.INFINITE))
                {
                    doAction("NO SUCH ROUTE ");
                    return false;
                }
                distance += value;
            }
            doAction(distance.ToString());
            return true;
        }
    }
}
