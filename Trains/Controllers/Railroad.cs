using System;
using System.Collections.Generic;

namespace Trains
{
    public class RailroadUtils
    {
        //邻接表
        private readonly TrainGraph trainGraph;
        //邻接矩阵
        private readonly TrainMatrix trainMatrix;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="graph">图类</param>
        public RailroadUtils(string graph)
        {
            //初始化邻接表
            trainGraph = new TrainGraph(graph);

            trainMatrix = new TrainMatrix(graph);
        }

        /// <summary>
        /// 获取两站之间的距离（Test 1-5）
        /// </summary>
        /// <param name="routes"></param>
        /// <returns></returns>
        public string GetDistanceOfRoutes(string routes)
        {
            return trainMatrix.GetDistanceOfRoutes(routes);
        }

        /// <summary>
        /// 经过站点数最大值为num(Test 6)
        /// </summary>
        /// <param name="start">始发站</param>
        /// <param name="end">终点站</param>
        /// <param name="num">最大站数</param>
        /// <returns></returns>
        public int GetNumberWithMaximum(string start, string end, int num)
        {
            int result = 0;
            trainGraph.GetNumberWithCondition(start, (distance, routes) =>
            {
                var stopsNumber = routes.Count - 1;
                if (routes.Peek().Equals(end) && stopsNumber <= num)
                {
                    result++;
                }
                return stopsNumber <= num;
            });
            return result;
        }

        /// <summary>
        /// 刚好到达终点的站点数为num（Test 7）
        /// </summary>
        /// <param name="start">始发站</param>
        /// <param name="end">终点站</param>
        /// <param name="num">站点数</param>
        /// <returns></returns>
        public int GetNumberWithExactlyStops(string start, string end, int num)
        {
            int result = 0;
            string stations = start;
            trainGraph.GetNumberWithCondition(start, (weight, routes) =>
            {
                var stopsNumber = routes.Count - 1;
                if (routes.Peek().Equals(end) && stopsNumber == num)
                {
                    result++;
                }
                return stopsNumber <= num;
            });
            return result;
        }

        /// <summary>
        /// 获取两站之间站点数小于num的方案总数 (Test 10)
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public int GetNumberLessThanDistance(string start, string end, int num)
        {
            int result = 0;
            trainGraph.GetNumberWithCondition(start, (distance, routes) =>
            {
                if (routes.Peek().Equals(end) && distance < num)
                {
                    result++;
                }
                return distance < num;
            });
            return result;
        }

        public string GetShortestDistance(string start, string end)
        {
            VerifyNode(start, end);
            //shortestDict起点到各个边的最短距离
            Dictionary<string, decimal> shortestDict = new Dictionary<string, decimal>();
            foreach (var vex in trainMatrix.VertexList)
            {
                var value = trainMatrix.GetDistance(start, vex.Name);
                shortestDict.TryAdd(vex.Name, value);
            }
            //curStation 当前站点
            trainGraph.GetShortestPath(start, (curStation, desStation, distance) =>
            {
                decimal shortestDistance = 0;
                //当前点是起点时 weight = 0
                if (!curStation.Equals(start))
                {
                    shortestDistance = shortestDict[curStation];
                }
                //存储最小路径
                if (shortestDict.TryGetValue(desStation, out decimal value)
                    && value > distance + shortestDistance)
                {
                    shortestDict[desStation] = distance + shortestDistance;
                }
                Console.Write(string.Format(@"{0}-{1}:{2}  ", start, desStation, shortestDistance + distance));
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
            Console.WriteLine($"#1: { GetDistanceOfRoutes("A-B-C") }");
            Console.WriteLine($"#2: { GetDistanceOfRoutes("A-D") }");
            Console.WriteLine($"#3: { GetDistanceOfRoutes("A-D-C") }");
            Console.WriteLine($"#4: { GetDistanceOfRoutes("A-E-B-C-D") }");
            Console.WriteLine($"#5: { GetDistanceOfRoutes("A-E-D") }");
            Console.WriteLine($"#6: { GetNumberWithMaximum("C", "C", 3) }");
            Console.WriteLine($"#7: { GetNumberWithExactlyStops("A", "C", 4)}");
            Console.WriteLine($"#8: { GetShortestDistance("A", "C") }");
            Console.WriteLine($"#9: { GetShortestDistance("B", "B") }");
            Console.WriteLine($"#10:{ GetNumberLessThanDistance("C", "C", 30) }");
        }
    }
}
