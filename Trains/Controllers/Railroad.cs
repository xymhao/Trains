using System;
using System.Collections.Generic;

namespace Trains
{
    public class Railroad
    {
        //邻接表
        private readonly TrainGraph trainGraph;
        //邻接矩阵
        private readonly TrainMatrix trainMatrix;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="graph">图类</param>
        public Railroad(string graph)
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
            var distance = trainMatrix.GetDistanceOfRoutes(routes);
            return distance.Equals(Constant.INFINITE) ? "NO SUCH ROUTE" : distance.ToString();
        }

        public int GetNumberOfRoutes(string start, string end, int num, OPCondition op)
        {
            int result = 0;
            trainGraph.GetNumberWithCondition(start, (distance, routes) =>
            {
                var stopsNumber = routes.Count - 1;
                if (routes.Peek().Equals(end) && op.GetConditionResult(num, stopsNumber))
                {
                    result++;
                }
                return op.GetConditionResult(num, stopsNumber);
            });
            return result;
        }

        public string GetDuration(string routes)
        {
            var routeArr = routes.Split("-");
            var distance = trainMatrix.GetDistanceOfRoutes(routes);
            if(distance.Equals(Constant.INFINITE))
            {
                return "NO SUCH ROUTE";
            }
            var duration = (routeArr.Length - 2) * Constant.UNIT_STATION + Convert.ToDecimal(distance) * Constant.UNIT_DISTANCE;
            return duration.ToString();
        }

        public int GetNumberWithMaximumDuration(string start, string end, int maxinum)
        {
            int result = 0;
            trainGraph.GetNumberWithCondition(start, (distance, routes) =>
            {
                var stopsNumber = routes.Count - 1;
                var duration = (routes.Count - 2) * Constant.UNIT_STATION + distance * Constant.UNIT_DISTANCE;
                if (routes.Peek().Equals(end) && duration <= maxinum)
                {
                    result++;
                }
                return duration <= maxinum;
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
        public int GetNumberOfDistance(string start, string end, int num, OPCondition op)
        {
            int result = 0;
            trainGraph.GetNumberWithCondition(start, (distance, routes) =>
            {
                if (routes.Peek().Equals(end) && op.GetConditionResult(num, Convert.ToInt32(distance)))
                {
                    result++;
                }
                return op.GetConditionResult(num, Convert.ToInt32(distance));
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
            trainGraph.GetPath(start, (curStation, desStation, distance) =>
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

        public string GetLongtDistance(string start, string end)
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
            trainGraph.GetPath(start, (curStation, desStation, distance) =>
            {
                decimal shortestDistance = 0;
                //当前点是起点时 weight = 0
                if (!curStation.Equals(start))
                {
                    shortestDistance = shortestDict[curStation];
                }
                //存储最小路径
                if (shortestDict.TryGetValue(desStation, out decimal value)
                    && value < distance + shortestDistance)
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
            ;
            Console.WriteLine($"#6: { GetNumberOfRoutes("C", "C", 3, new LessThanAndEqualCondtion()) }");
            Console.WriteLine($"#6: { GetNumberOfRoutes("A", "C", 4, new EqualCondition()) }");
            //Console.WriteLine($"#6: { GetNumberWithMaximum("C", "C", 3) }");
            //Console.WriteLine($"#7: { GetNumberWithExactlyStops("A", "C", 4)}");
            Console.WriteLine($"#8: { GetShortestDistance("A", "C") }");
            Console.WriteLine($"#9: { GetShortestDistance("B", "B") }");
            Console.WriteLine($"#10:{ GetNumberOfDistance("C", "C", 30, new LessThanCondtion()) }");
        }
    }
}
