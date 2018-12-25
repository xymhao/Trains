using System;
using System.Collections;
using System.Collections.Generic;

namespace Trains
{
    public class TrainGraph : IList<Station>
    {
        List<Station> _stationList;
        //是否是有向图
        private bool IsDirecte { get; set; } = true;

        public TrainGraph(bool isDirecte)
        {
            _stationList = new List<Station>();
            IsDirecte = isDirecte;
        }

        public TrainGraph(bool isDirecte, Graph graph) : this(isDirecte)
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
                Route tmp, arcNode = startStation.FirstRoute;
                //找末尾边节点添加边
                do
                {   //检查是否添加了重复边
                    VerifyArcNode(endStation, arcNode);
                    tmp = arcNode;
                    arcNode = arcNode.NextRoute;
                } while (arcNode != null);
                //设置边节点
                tmp.SetNextRoute(endStation, weight);
            }
        }

        private static void VerifyArcNode(Station endVertex, Route arcNode)
        {
            if (arcNode.GetDestinationStation().Equals(endVertex.GetStation()))
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
            _stationList.Add(item);
        }

        //查找指定项并返回
        public Station Find(Station item)
        {
            foreach (Station vertex in _stationList)
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
            return _stationList.Contains(item);
        }

        public int Count => _stationList.Count;

        public bool IsReadOnly => false;

        public Station this[int index]
        {
            get
            {
                if (index >= _stationList.Count)
                    throw new IndexOutOfRangeException("索引超出了界限");
                return _stationList[index];
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
            return _stationList.IndexOf(item);
        }

        public void Insert(int index, Station item)
        {
            _stationList.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _stationList.RemoveAt(index);
        }

        public void Clear()
        {
            _stationList = new List<Station>();
        }

        public void CopyTo(Station[] array, int arrayIndex)
        {
            _stationList.CopyTo(array, arrayIndex);
        }

        public bool Remove(Station item)
        {
            return _stationList.Remove(item);
        }

        public bool Remove(string item)
        {
            return Remove(new Station(item));
        }

        public IEnumerator<Station> GetEnumerator()
        {
            return _stationList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _stationList.GetEnumerator();
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
                        result += arcNode.Weight;
                        break;
                    }
                    arcNode = arcNode.NextRoute;
                } while (arcNode != null);
            }
            return result.ToString();
        }

        //深度优先遍历
        public void GetNumberWithCondition(string start, Func<int, string, decimal, bool> func = null, int curStops = 0, int weight = 0) 
        {
            var startStation = Find(start);
            //将visited标志全部置为false
            InitVisited();
            //从第一个顶点开始遍历
            DFS(startStation, func, curStops);
        }

        //使用递归进行深度优先遍历
        private void DFS(Station startStation, Func<int, string, decimal, bool> func = null, int curStops = 0, decimal weight = 0)
        {
            startStation.Visited = true;
            Route node = startStation.FirstRoute;
            while (node != null)
            {
                curStops = curStops == 0 ? 1 : curStops;
                weight += node.Weight;
                //外置条件是否继续遍历
                var isdfs = func?.Invoke(curStops, node.GetDestinationStation(), weight) ?? false;
                var nextVertex = node.Station;
                //如果邻接点未被访问，则递归访问它的边
                if (!nextVertex.Visited || isdfs)
                {
                    DFS(nextVertex, func, curStops + 1, weight);
                }
                weight -= node.Weight;
                node = node.NextRoute;
            }
        }

        public void ShortestPath(string start, Action<string, Route> func = null)
        {
            Station vertexNode = Find(start);
            Queue<Station> queue = new Queue<Station>();
            InitVisited();
            vertexNode.Visited = true;
            queue.Enqueue(vertexNode);

            while (queue.Count > 0)
            {
                Station curruntStation = queue.Dequeue();
                Route arcNode = curruntStation.FirstRoute;
                //访问此顶点的所有邻接点
                while (arcNode != null)
                {
                    var nextVertex = arcNode.Station;
                    func?.Invoke(curruntStation.GetStation(), arcNode);
                    if (!nextVertex.Visited)
                    {
                        queue.Enqueue(nextVertex);
                    }
                    nextVertex.Visited = true;
                    //访问下一个邻接点
                    arcNode = arcNode.NextRoute;
                }
            }
        }

        //初始化visited标志
        private void InitVisited()
        {
            foreach (Station v in _stationList)
            {
                v.Visited = false;
            }
        }
    }

    //表头结点（表示图的顶点）
    public class Station
    {
        private readonly string _data;

        public Boolean Visited { get; set; }

        public Route FirstRoute { get; set; }

        public Station(string value)
        {
            _data = value;
        }

        public string GetStation()
        {
            return _data;
        }

        //重写equals方法
        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Station vertex = obj as Station;
                return (_data == vertex._data);
            }
        }

        public bool ContainsNode(string nodeData)
        {
            var curruntNode = FirstRoute;
            do
            {
                if (curruntNode.GetDestinationStation().Equals(nodeData))
                {
                    return true;
                }
                curruntNode = curruntNode.NextRoute;
            } while (curruntNode != null);
            return false;
        }

        public override int GetHashCode() => _data.GetHashCode();
    }

    //表结点（表示图的边）
    public class Route
    {
        public Route NextRoute { get; set; }

        public decimal Weight { get; }

        public Station Station { get; }

        public Route(Station station, decimal value)
        {
            Station = station;
            Weight = value;
        }

        public string GetDestinationStation()
        {
            return Station.GetStation();
        }

        public void SetNextRoute(Station vertex, decimal weight)
        {
            NextRoute = new Route(vertex, weight);
        }
    }
}
