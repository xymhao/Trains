﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace Trains
{
    public class TrainGraph : IList<Station>
    {
        List<Station> stationList;
        //是否是有向图
        private bool IsDirecte { get; set; } = true;

        public TrainGraph(bool isDirecte)
        {
            stationList = new List<Station>();
            IsDirecte = isDirecte;
        }

        public TrainGraph(Graph graph, bool isDirecte = true) : this(isDirecte)
        {
            foreach (var st in graph.VertexList)
            {
                AddStation(st.Name);
            }
            foreach (var ds in graph.ArcList)
            {
                AddRoute(ds.Start, ds.End, Convert.ToDecimal(ds.Weight));
            }
        }

        public void AddStation(string item)
        {
            Add(new Station(item));
        }

        /// <summary>
        /// 添加边
        /// </summary>
        /// <param name="start">始点</param>
        /// <param name="end">终点</param>
        /// <param name="weight">权值</param>
        public void AddStation(Station start, Station end, decimal weight)
        {
            //找到起始顶点
            Station startVertex = Find(start);
            if (startVertex == null)
            {
                throw new ArgumentException("头顶点并不存在！");
            }
            //找到结束顶点
            Station endVertex = Find(end);
            if (endVertex == null)
            {
                throw new ArgumentException("尾顶点并不存在！");
            }
            //无向边的两个顶点都需记录边信息
            AddDirectedEdge(startVertex, endVertex, weight);
            if (!IsDirecte)
            {
                AddDirectedEdge(endVertex, startVertex, weight);
            }
        }

        public void AddRoute(string start, string end, decimal weight = 0)
        {
            Station startVertex = Find(new Station(start));
            if (startVertex == null)
            {
                throw new ArgumentException("头顶点并不存在！");
            }
            Station endVertex = Find(new Station(end));
            if (endVertex == null)
            {
                throw new ArgumentException("尾顶点并不存在！");
            }
            //无向边的两个顶点都需记录边信息
            AddDirectedEdge(startVertex, endVertex, weight);
            if (!IsDirecte)
            {
                AddDirectedEdge(endVertex, startVertex, weight);
            }
        }

        //添加有向边
        private void AddDirectedEdge(Station startStation, Station endStation, decimal weight)
        {
            //无邻接点时
            if (startStation.FirstRoute == null)
            {
                startStation.FirstRoute = new Route(endStation, weight);
            }
            else
            {
                Route tmp, route = startStation.FirstRoute;
                //找末尾边节点添加边
                do
                {   //检查是否添加了重复边
                    VerifyRoute(endStation, route);
                    tmp = route;
                    route = route.NextRoute;
                } while (route != null);
                //设置边节点
                tmp.SetNextRoute(endStation, weight);
            }
        }

        private static void VerifyRoute(Station endStation, Route route)
        {
            if (route.GetDestinationStation().Equals(endStation.Name))
            {
                throw new ArgumentException("添加了重复的边！");
            }
        }

        #region IList Interface
        public void Add(Station item)
        {
            //不允许插入重复值
            if (Contains(item))
            {
                throw new ArgumentException("插入了重复顶点！");
            }
            stationList.Add(item);
        }

        //查找指定项并返回
        public Station Find(Station item)
        {
            foreach (Station vertex in stationList)
            {
                if (vertex.Equals(item))
                {
                    return vertex;
                }
            }
            return null;
        }

        //查找指定项并返回
        public Station Find(string itemStr)
        {
            return Find(new Station(itemStr));
        }

        public bool Contains(Station item)
        {
            return stationList.Contains(item);
        }

        public int Count => stationList.Count;

        public bool IsReadOnly => false;

        public Station this[int index]
        {
            get
            {
                if (index >= stationList.Count)
                    throw new IndexOutOfRangeException("索引超出了界限");
                return stationList[index];
            }
            set
            {
                //if (index >= _stationList.Count)
                //    throw new IndexOutOfRangeException("索引超出了界限");

                ////不允许插入重复值
                //if (Contains(value))
                //{
                //    throw new ArgumentException("插入了重复顶点！");
                //}
                //_stationList[index] = value;
            }
        }

        public int IndexOf(Station item)
        {
            return stationList.IndexOf(item);
        }

        public void Insert(int index, Station item)
        {
            stationList.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            stationList.RemoveAt(index);
        }

        public void Clear()
        {
            stationList = new List<Station>();
        }

        public void CopyTo(Station[] array, int arrayIndex)
        {
            stationList.CopyTo(array, arrayIndex);
        }

        public bool Remove(Station item)
        {
            return stationList.Remove(item);
        }

        public bool Remove(string item)
        {
            return Remove(new Station(item));
        }

        public IEnumerator<Station> GetEnumerator()
        {
            return stationList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return stationList.GetEnumerator();
        }
        #endregion

        /// <summary>
        /// 获取路程距离
        /// </summary>
        /// <param name="routes"></param>
        /// <returns></returns>
        public string GetDistanceOfRoutes(string routes)
        {
            var stops = routes.Split("-");
            decimal result = 0;
            for (var i = 0; i < stops.Length - 1; i++)
            {
                var start = Find(new Station(stops[i]));
                if (!start.ContainsNode(stops[i + 1]))
                {
                    return "NO SUCH ROUTE";
                }
                var arcNode = start.FirstRoute;
                do
                {
                    if (arcNode.GetDestinationStation().Equals(stops[i + 1]))
                    {
                        result += arcNode.Distance;
                        break;
                    }
                    arcNode = arcNode.NextRoute;
                } while (arcNode != null);
            }
            return result.ToString();
        }

        //深度优先遍历
        public void GetNumberWithCondition(string start, Func<decimal, Stack<string>, bool> func = null, int weight = 0)
        {
            var startStation = Find(start);
            //将visited标志全部置为false
            InitVisited();
            //从第一个顶点开始遍历
            Stack<string> stack = new Stack<string>();
            stack.Push(startStation.Name);
            DFS(startStation, func, stack:stack);
        }

        //使用递归进行深度优先遍历
        private void DFS(Station startStation, Func<decimal, Stack<string>,bool> func = null, decimal weight = 0, Stack<string> stack = null)
        {
            startStation.Visited = true;
            Route route = startStation.FirstRoute;
            while (route != null)
            {
                weight += route.Distance;
                stack?.Push(route.GetDestinationStation());
                //外置条件是否继续遍历
                var isdfs = func?.Invoke(weight, stack) ?? false;
                var nextVertex = route.Station;
                //如果邻接点未被访问，则递归访问它的边
                if (!nextVertex.Visited || isdfs)
                {
                    DFS(nextVertex, func, weight, stack);
                }
                stack.Pop();
                weight -= route.Distance;
                route = route.NextRoute;
            }
        }

        public void GetShortestPath(string start, Action<string, string, decimal> func = null)
        {
            Station vertexNode = Find(start);
            Queue<Station> queue = new Queue<Station>();
            InitVisited();
            vertexNode.Visited = true;
            queue.Enqueue(vertexNode);

            while (queue.Count > 0)
            {
                Station curruntStation = queue.Dequeue();
                Route route = curruntStation.FirstRoute;
                //访问此顶点的所有邻接点
                while (route != null)
                {
                    var nextVertex = route.Station;
                    func?.Invoke(curruntStation.Name, route.GetDestinationStation(), route.Distance);
                    if (!nextVertex.Visited)
                    {
                        queue.Enqueue(nextVertex);
                    }
                    nextVertex.Visited = true;
                    //访问下一个邻接点
                    route = route.NextRoute;
                }
            }
        }

        //初始化visited标志
        private void InitVisited()
        {
            foreach (Station v in stationList)
            {
                v.Visited = false;
            }
        }
    } 
}
