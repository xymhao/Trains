using System;
using System.Collections.Generic;
using System.Text;

namespace Trains
{
    public class AdjMatrix
    {
        private const int INFINITE = int.MaxValue;

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
                _matrix[i, j] = Convert.ToInt32(g.Weight);
            }
            for (var i = 0; i < graph.VertexList.Count; i++)
            {
                for (var j = 0; j < graph.VertexList.Count; j++)
                {
                    _matrix[i, j] = _matrix[i, j].Equals(0) ? INFINITE : _matrix[i, j];
                }
            }
        }

        public decimal GetDistance(string start, string end)
        {
            var i = _graph.VertexList.Find(p => p.Name.Equals(start)).ID;
            var j = _graph.VertexList.Find(p => p.Name.Equals(end)).ID;
            return _matrix[i, j];
        }

        public string GetRoutesDistance(string route)
        {
            var stops = route.Split("-");
            decimal distance = 0;
            for (var i = 0; i < stops.Length - 1; i++)
            {
                var value = GetDistance(stops[i], stops[i + 1]);
                if (value == 0)
                {
                    return "NO SUCH ROUTE";
                }
                distance += value;
            }
            return distance.ToString();
        }

        public void Dijkstra(int startIndex, decimal[] path, decimal[] cost, decimal max)
        {
            int nodeCount = _matrix.GetLength(0);
            bool[] v = new bool[nodeCount];
            //初始化 path，cost，V
            for (int i = 0; i < nodeCount; i++)
            {
                if (i == startIndex)//如果是出发点
                {
                    v[i] = true;//
                }
                else
                {
                    cost[i] = _matrix[startIndex, i];
                    if (cost[i] < max && cost[i] != 0)
                        path[i] = startIndex;
                    else
                        path[i] = -1;
                    v[i] = false;
                }
            }

            for (int i = 1; i < nodeCount; i++)//求解nodeCount-1个
            {
                decimal minCost = max;
                int curNode = -1;
                for (int w = 0; w < nodeCount; w++)
                {
                    if (!v[w])//未在V集合中
                    {
                        if (cost[w] < minCost)
                        {
                            minCost = cost[w];
                            curNode = w;
                        }
                    }
                }//for  获取最小权值的节点
                if (curNode == -1) break;//剩下都是不可通行的节点，跳出循环
                v[curNode] = true;
                for (int w = 0; w < nodeCount; w++)
                {
                    if (!v[w] && (_matrix[curNode, w] + cost[curNode] < cost[w]))
                    {
                        cost[w] = _matrix[curNode, w] + cost[curNode];//更新权值
                        path[w] = curNode;//更新路径
                    }
                }//for 更新其他节点的权值（距离）和路径
            }
        }
    }
}
