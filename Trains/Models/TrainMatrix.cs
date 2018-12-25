using System;
using System.Collections.Generic;
using System.Text;

namespace Trains
{
    public class TrainMatrix
    {
        public Graph graph;
        public decimal[,] matrix;

        public TrainMatrix(Graph graph)
        {
            this.graph = graph;
            matrix = new decimal[graph.VertexList.Count, graph.VertexList.Count];
            for (var i = 0; i < graph.VertexList.Count; i++)
            {
                for (var j = 0; j < graph.VertexList.Count; j++)
                {
                    matrix[i, j] = graph.GetDistance(i, j);
                }
            }
        }

        public decimal GetDistance(string start, string end)
        {
            var startVertex = graph.FindVertexIDByStation(start);
            var endVertex = graph.FindVertexIDByStation(end);
            return matrix[startVertex, endVertex];
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
