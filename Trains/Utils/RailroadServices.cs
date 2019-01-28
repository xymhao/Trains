using System;
using System.Collections.Generic;

namespace Trains
{
    public class RailroadServices
    {
        private const decimal INFINITE = decimal.MaxValue;

        private AdjMatrix _adjMatrix;
        private AdjacencyList _adjList;
        private Graph _graph;

        public RailroadServices(Graph graph)
        {
            _graph = graph;
            //初始化邻接矩阵
            _adjMatrix = new AdjMatrix(graph);
            //初始化邻接表
            _adjList = new AdjacencyList(true, graph);
        }

        public string GetDistanceOfRoutes(string routes)
        {
            string result = _adjMatrix.GetRoutesDistance(routes);
            return result;
        }

        public int GetNumberOfMaximum(string start, string end, int num)
        {
            int result = 0;
            _adjList.GetResultNumber(start, (count, curStop, weight) =>
            {
                if (curStop.Equals(end) && count <= num)
                {
                    result++;
                }
                return count <= num;
            });
            Console.WriteLine($"#6:{result}");
            return result;
        }

        public int GetNumberWithExactlyStops(string start, string end, int num)
        {
            int result = 0;
            _adjList.GetResultNumber(start, (count, curStop, weight) =>
            {
                if (curStop.Equals(end) && count.Equals(num))
                {
                    result++;
                }
                return count <= num;
            });
            Console.WriteLine($"#6:{result}");
            return result;
        }

        public int GetNumberLessThanDistance(string start, string end, int num)
        {
            int result = 0;
            _adjList.GetResultNumber(start, (count, curStop, distance) =>
            {
                if (curStop.Equals(end) && distance < num)
                {
                    result++;
                }
                return count <= num;
            });
            Console.WriteLine($"#6:{result}");
            return result;
        }

        public string GetShortestDistance(string start, string end)
        {
            VerifyNode(start, end);
            Dictionary<string, decimal> dict = new Dictionary<string, decimal>();
            //初始化路程
            foreach (var vex in _graph.VertexList)
            {
                var value = _adjMatrix.GetDistance(start, vex.Name);
                dict.TryAdd(vex.Name, value);
            }

            _adjList.ShortestPath(start, (vertex, arcNode) =>
            {
                decimal weight = 0;
                if (!vertex._data.Equals(start))
                {
                    weight = dict[vertex._data];
                }
                if (dict.TryGetValue(arcNode.GetVertexNodeData(), out decimal value)
                    && value > arcNode._weight + weight)
                {
                    dict[arcNode.GetVertexNodeData()] = arcNode._weight + weight;
                }
                Console.Write(string.Format(@"{0}-{1}:{2}  ", start, arcNode.GetVertexNodeData(), weight + arcNode._weight));
            });
            return dict[end].ToString();
        }

        private void VerifyNode(string start, string end)
        {
            if (_adjList.Find(new VertexNode(start)).Equals(null))
            {
                throw new ArgumentNullException(nameof(start), "输入站点不存在");
            }
            if (_adjList.Find(new VertexNode(end)).Equals(null))
            {
                throw new ArgumentNullException(nameof(end), "输入站点不存在");
            }
        }

        public void TestOutput()
        {

        }
    }
}
