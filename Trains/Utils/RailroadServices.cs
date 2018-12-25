using System;
using System.Collections.Generic;

namespace Trains
{
    public class RailroadServices
    {
        private readonly TrainGraph trainGraph;
        private readonly Graph graph;
        private readonly AdjMatrix adjMatrix;


        public RailroadServices(Graph graph)
        {
            this.graph = graph;
            //初始化邻接表
            trainGraph = new TrainGraph(true, graph);

            adjMatrix = new AdjMatrix(graph);
        }

        public string GetDistanceOfRoutes(string routes)
        {
            return trainGraph.GetDistanceOfRoutes(routes);
        }

        public int GetNumberWithMaximum(string start, string end, int num)
        {
            int result = 0;
            trainGraph.GetNumberWithCondition(start, (count, station, weight) =>
            {
                if (station.Equals(end) && count <= num)
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
            trainGraph.GetNumberWithCondition(start, (cur, station, weight) =>
            {
                if (station.Equals(end) && cur == num)
                {
                    result++;
                }
                return cur <= num;
            });
            Console.WriteLine($"#7:{result}");
            return result;
        }

        public int GetNumberLessThanDistance(string start, string end, int num)
        {
            int result = 0;
            trainGraph.GetNumberWithCondition(start, (count, station, weight) =>
            {
                if (station.Equals(end) && weight < num)
                {
                    result++;
                }
                return count < num;
            });
            Console.WriteLine($"#10:{result}");
            return result;
        }

        public string GetShortestDistance(string start, string end)
        {
            VerifyNode(start, end);
            //构造起点start 到各个边的距离
            Dictionary<string, decimal> shortestDict = new Dictionary<string, decimal>();
            foreach (var vex in graph.VertexList)
            {
                var value = adjMatrix.GetDistance(start, vex.Name);
                shortestDict.TryAdd(vex.Name, value);
            }
            //curStation 当前站点
            trainGraph.ShortestPath(start, (curStation, route) =>
            {
                decimal weight = 0;
                //当前点是起点时 weight = 0
                if (!curStation.Equals(start))
                {
                    weight = shortestDict[curStation];
                }
                //如果
                if (shortestDict.TryGetValue(route.GetDestinationStation(), out decimal value)
                    && value > route.Weight + weight)
                {
                    shortestDict[route.GetDestinationStation()] = route.Weight + weight;
                }
                Console.Write(string.Format(@"{0}-{1}:{2}  ", start, route.GetDestinationStation(), weight + route.Weight));
            });
            return shortestDict[end].ToString();
        }

        private void VerifyNode(string start, string end)
        {
            if (trainGraph.Find(start).Equals(null))
            {
                throw new ArgumentNullException(nameof(start), "输入站点不存在");
            }
            if (trainGraph.Find(end).Equals(null))
            {
                throw new ArgumentNullException(nameof(end), "输入站点不存在");
            }
        }

        public void TestOutput()
        {
            Console.WriteLine($"#1: { GetDistanceOfRoutes("A-B-C")}");
            Console.WriteLine($"#2: { GetDistanceOfRoutes("A-D")}");
            Console.WriteLine($"#3: { GetDistanceOfRoutes("A-D-C")}");
            Console.WriteLine($"#4: { GetDistanceOfRoutes("A-E-B-C-D")}");
            Console.WriteLine($"#5: { GetDistanceOfRoutes("A-E-D")}");
            GetNumberWithMaximum("C", "C", 3);
            GetNumberWithExactlyStops("A", "C", 4);
            Console.WriteLine($"#8:{GetShortestDistance("A", "C")}");
            Console.WriteLine($"#9:{GetShortestDistance("B", "B")}");
            GetNumberLessThanDistance("C", "C", 30);
        }
    }
}
