using System;

namespace Trains
{
    public class TrainMatrix
    {
        private Graph graph;
        private decimal[,] matrix;

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

        public string GetDistanceOfRoutes(string route)
        {
            var stops = route.Split("-");
            decimal distance = 0;
            string result = string.Empty;
            for (var i = 0; i < stops.Length - 1; i++)
            {
                var value = GetDistance(stops[i], stops[i + 1]);
                if (value == 0 || value.Equals(Constant.INFINITE))
                {
                    return "NO SUCH ROUTE ";
                }
                distance += value;
            }
            return distance.ToString();
        }
    }
}
