using System;
using System.Collections;
using System.Collections.Generic;

namespace Trains
{
    public class AdjacencyList : IList<VertexNode>
    {
        List<VertexNode> _list;
        //是否是有向图
        bool IsDirecte { get; set; } = true;

        public AdjacencyList(bool isDirecte)
        {
            _list = new List<VertexNode>();
            IsDirecte = isDirecte;
        }

        public AdjacencyList(bool isDirecte, Graph graph)
        {
            _list = new List<VertexNode>();
            foreach (var st in graph.VertexList)
            {
                Add(st.Name);
            }
            foreach (var ds in graph.ArcList)
            {
                AddEdge(ds.Start, ds.End, Convert.ToDecimal(ds.Weight));
            }
            IsDirecte = isDirecte;
        }

        public void Add(VertexNode item)
        {
            //不允许插入重复值
            if (Contains(item))
            {
                throw new ArgumentException("插入了重复顶点！");
            }
            _list.Add(item);
        }

        public void Add(string item)
        {
            Add(new VertexNode(item));
        }

        /// <summary>
        /// 添加边
        /// </summary>
        /// <param name="from">始点</param>
        /// <param name="to">终点</param>
        /// <param name="weight">权值</param>
        public void AddEdge(VertexNode from, VertexNode to, decimal weight)
        {
            VertexNode fromVer = Find(from); //找到起始顶点
            if (fromVer == null)
            {
                throw new ArgumentException("头顶点并不存在！");
            }
            VertexNode toVer = Find(to); //找到结束顶点
            if (toVer == null)
            {
                throw new ArgumentException("尾顶点并不存在！");
            }
            //无向边的两个顶点都需记录边信息
            AddDirectedEdge(fromVer, toVer, weight);
            if (!IsDirecte)
                AddDirectedEdge(toVer, fromVer, weight);
        }

        public void AddEdge(string from, string to, decimal weight = 0)
        {
            VertexNode fromVer = Find(new VertexNode(from));
            if (fromVer == null)
            {
                throw new ArgumentException("头顶点并不存在！");
            }
            VertexNode toVer = Find(new VertexNode(to));
            if (toVer == null)
            {
                throw new ArgumentException("尾顶点并不存在！");
            }
            //无向边的两个顶点都需记录边信息
            AddDirectedEdge(fromVer, toVer, weight);
            if (!IsDirecte)
                AddDirectedEdge(toVer, fromVer, weight);
        }

        //查找指定项并返回
        public VertexNode Find(VertexNode item)
        {
            foreach (VertexNode v in _list)
            {
                if (v.Equals(item))
                {
                    return v;
                }
            }
            return null;
        }

        //查找指定项并返回
        public VertexNode Find(string itemStr)
        {
            return Find(new VertexNode(itemStr));
        }

        public bool Contains(VertexNode item)
        {
            return _list.Contains(item);
        }

        //添加有向边
        private void AddDirectedEdge(VertexNode fromVer, VertexNode toVer, decimal weight)
        {
            if (fromVer._firstArc == null) //无邻接点时
            {
                fromVer._firstArc = new ArcNode(toVer, weight);
            }
            else
            {
                ArcNode tmp, arcNode = fromVer._firstArc;
                do
                {   //检查是否添加了重复边
                    if (arcNode.GetNextNodeData().Equals(toVer._data))
                    {
                        throw new ArgumentException("添加了重复的边！");
                    }
                    tmp = arcNode;
                    arcNode = arcNode._nextArc;
                } while (arcNode != null);
                tmp._nextArc = new ArcNode(toVer, weight);
            }
        }

        public int Count => _list.Count;

        public bool IsReadOnly => false;

        public VertexNode this[int index]
        {
            get
            {
                if (index >= _list.Count)
                    throw new IndexOutOfRangeException("索引超出了界限");
                return _list[index];
            }
            set
            {
                if (index >= _list.Count)
                    throw new IndexOutOfRangeException("索引超出了界限");

                //不允许插入重复值
                if (Contains(value))
                {
                    throw new ArgumentException("插入了重复顶点！");
                }
                _list[index] = value;
            }
        }

        public int IndexOf(VertexNode item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, VertexNode item)
        {
            _list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        public void Clear()
        {
            _list = new List<VertexNode>();
        }

        public void CopyTo(VertexNode[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public bool Remove(VertexNode item)
        {
            return _list.Remove(item);
        }

        public bool Remove(string item)
        {
            return Remove(new VertexNode(item));
        }

        public IEnumerator<VertexNode> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        #region DFS
        public string GetDistanceOfRoutes(List<string> stops)
        {
            decimal result = 0;
            for (var i = 0; i < stops.Count - 1; i++)
            {
                var start = Find(new VertexNode(stops[i]));
                if (!start.ContainsNode(stops[i + 1]))
                {
                    return "NO SUCH ROUTE";
                }
                var arcNode = start._firstArc;
                do
                {
                    if (arcNode.GetNextNodeData().Equals(stops[i + 1]))
                    {
                        result += arcNode._weight;
                        break;
                    }
                    arcNode = arcNode._nextArc;
                } while (arcNode != null);
            }
            return result.ToString();
        }
        public void DFSTraverse(VertexNode vrtex, Func<int, ArcNode, decimal, bool> func = null, int curStops = 0, int weight = 0) //深度优先遍历
        {
            InitVisited(); //将visited标志全部置为false
            DFS(vrtex, func, curStops); //从第一个顶点开始遍历
        }

        public void DFS(VertexNode vrtexNode, Func<int, ArcNode, decimal, bool> func = null, int curStops = 0, decimal weight = 0) //使用递归进行深度优先遍历
        {
            vrtexNode._visited = true;
            ArcNode node = vrtexNode._firstArc;
            while (node != null)
            {
                curStops = curStops == 0 ? 1 : curStops;
                weight += node._weight;
                //外置条件是否继续遍历
                var isdfs = func?.Invoke(curStops, node, weight) ?? false;
                var nextVertex = node.GetNextVertexNode();
                //如果邻接点未被访问，则递归访问它的边
                if (!nextVertex._visited || isdfs)
                {
                    DFS(nextVertex, func, curStops + 1, weight);
                }
                weight -= node._weight;
                node = node._nextArc;
            }
        }
        public void ShortestPath(VertexNode vertexNode, Action<VertexNode, ArcNode> func = null)
        {
            Queue<VertexNode> queue = new Queue<VertexNode>();
            InitVisited();
            vertexNode._visited = true;
            queue.Enqueue(vertexNode);

            while (queue.Count > 0)
            {
                VertexNode curruntNode = queue.Dequeue();
                ArcNode arcNode = curruntNode._firstArc;
                while (arcNode != null) //访问此顶点的所有邻接点
                {
                    var nextVertex = arcNode.GetNextVertexNode();
                    func?.Invoke(curruntNode, arcNode);
                    if (!nextVertex._visited)
                    {
                        queue.Enqueue(nextVertex); //进队
                    }
                    nextVertex._visited = true; //设置访问标志
                    arcNode = arcNode._nextArc; //访问下一个邻接点
                }
            }
        }

        public void BFS(VertexNode v, Action<VertexNode, ArcNode, decimal> func = null) //使用递归进行深度优先遍历
        {
            Queue<VertexNode> queue = new Queue<VertexNode>();
            decimal weight = 0;
            v._visited = true; //设置访问标志
            queue.Enqueue(v); //进队
            while (queue.Count > 0) //只要队不为空就循环
            {
                VertexNode w = queue.Dequeue();
                ArcNode node = w._firstArc;
                weight += node._weight;
                while (node != null) //访问此顶点的所有邻接点
                {
                    VertexNode nextVertex = node.GetNextVertexNode();
                    if (!nextVertex._visited)
                    {
                        func?.Invoke(v, node, weight);
                        nextVertex._visited = true; //设置访问标志
                        queue.Enqueue(nextVertex); //进队
                    }
                    node = node._nextArc; //访问下一个邻接点
                }
            }
        }

        private void InitVisited() //初始化visited标志
        {
            foreach (VertexNode v in _list)
            {
                v._visited = false; //全部置为false
            }
        }
        #endregion

    }
    //表头结点（表示图的顶点）
    public class VertexNode
    {
        public readonly string _data;
        public ArcNode _firstArc;
        public Boolean _visited;
        public VertexNode(string value)
        {
            _data = value;
        }

        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                VertexNode v = (VertexNode)obj;
                return (_data == v._data);
            }
        }

        public bool ContainsNode(string nodeData)
        {
            var curruntNode = _firstArc;
            do
            {
                if (curruntNode.GetNextNodeData().Equals(nodeData))
                {
                    return true;
                }
                curruntNode = curruntNode._nextArc;
            } while (curruntNode != null);
            return false;
        }

        public override int GetHashCode() => _data.GetHashCode();
    }

    //表结点（表示图的边）
    public class ArcNode
    {
        private VertexNode _vertexNode;
        public ArcNode _nextArc;
        public decimal _weight;
        public ArcNode(VertexNode vertex, decimal value)
        {
            _vertexNode = vertex;
            _weight = value;
        }

        public string GetNextNodeData()
        {
            return _vertexNode._data;
        }

        public VertexNode GetNextVertexNode()
        {
            return _vertexNode;
        }
    }
}
