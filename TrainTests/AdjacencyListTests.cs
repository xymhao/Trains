using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Trains.Utils;

namespace TrainTests
{
    class AdjacencyListTests
    {

        private AdjacencyList adjList;

        [SetUp]
        public void Setup()
        {
            adjList = new AdjacencyList(true);
        }

        [Test]
        public void Add_Vertex_NoThrow()
        {
            Assert.DoesNotThrow(() => { adjList.Add(new VertexNode("A")); });
        }

        [Test]
        public void Add_Vertex_FindValue()
        {
            Assert.DoesNotThrow(() => { adjList.Add(new VertexNode("A")); });
            Assert.AreEqual("A", adjList[0]._data);
        }

        [Test]
        public void Set_VertexNode()
        {
            adjList.Add(new VertexNode("A"));
            adjList[0] = new VertexNode("B");
            Assert.AreEqual("B", adjList[0]._data);
        }

        [Test]
        public void Set_VertexNode_ReturnException()
        {
            adjList.Add(new VertexNode("A"));
            adjList.Add(new VertexNode("B"));
            Assert.Throws<ArgumentException>(() => {
                adjList[0] = new VertexNode("B");
            }, "插入了重复顶点！");
        }

        [Test]
        public void Set_IndexOutOfRange_ThrowException()
        {
            Assert.Throws<IndexOutOfRangeException>(() => {
                adjList[0] = new VertexNode("A");
            }, "超出索引界限");
        }

        [Test]
        public void Add_RepitionVertex_ThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() => {
                adjList.Add(new VertexNode("A"));
                adjList.Add(new VertexNode("A"));
            }, "插入了重复顶点！");
        }

        [Test]
        public void Add_Edgs()
        {
            adjList.Add("A");
            adjList.Add("B");
            adjList.Add("C");
            adjList.Add("D");
            adjList.Add("E");
            adjList.AddEdge("A", "B");
            adjList.AddEdge("A", "C");
            adjList.AddEdge("A", "D");
            var node = adjList[0]._firstArc;
            int i = 0;
            do
            {
                i++;
                node = node.nextArc;
            } while (node != null);
            Assert.AreEqual(3, i);
        }

        [Test]
        public void Add_VertexNode_ReturnBool()
        {
            adjList.Add("A");
            adjList.Add("B");
            adjList.Add("C");
            adjList.Add("D");
            adjList.Add("E");

            Assert.AreEqual(true, adjList.Contains(adjList[0]));
            Assert.AreEqual(true, adjList.Contains(adjList.Find("A")));
        }

        [Test]
        public void Remove_Vertex_ReturnTrue()
        {
            adjList.Add("A");
            Assert.AreEqual(true, adjList.Remove(new VertexNode("A")));
            adjList.Add("A");
            Assert.AreEqual(true, adjList.Remove("A"));
        }

        [Test]
        public void Remove_Vertex_ReturnFalse()
        {
            adjList.Add("A");
            Assert.AreEqual(false, adjList.Remove(new VertexNode("B")));
            Assert.AreEqual(false, adjList.Remove("B"));
        }

        [Test]
        public void Verify_Enumerator()
        {
            adjList.Add("A");
            adjList.Add("B");
            adjList.Add("C");
            adjList.Add("D");
            adjList.Add("E");
            adjList.AddEdge("A", "B");
            adjList.AddEdge("A", "C");
            adjList.AddEdge("A", "D");
            List<string> ls = new List<string>();

            var items = adjList.GetEnumerator();
            foreach (var ad in adjList)
            {
                Assert.AreEqual(true, adjList.Contains(ad));
            }
        }
    }
}
