using Trains;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;

namespace TrainTests
{
    public class TrainGraphTests
    {
        private TrainGraph trainGraph;

        [SetUp]
        public void Setup()
        {
            trainGraph = new TrainGraph(true);
        }

        [Test]
        public void Add_Station()
        {
            Assert.DoesNotThrow(() => { trainGraph.AddStation("A"); });
        }

        [Test]
        public void Get_Station_FindValue()
        {
            trainGraph.AddStation("A");
            Assert.AreEqual("A", trainGraph[0].GetStation());
        }

        [Test]
        public void Add_Routes()
        {
            trainGraph.AddStation("A");
            trainGraph.AddStation("B");
            trainGraph.AddStation("C");
            trainGraph.AddStation("D");
            trainGraph.AddStation("E");
            trainGraph.AddRoute("A", "B");
            trainGraph.AddRoute("A", "C");
            trainGraph.AddRoute("A", "D");
            var node = trainGraph[0].FirstRoute;
            int i = 0;
            do
            {
                i++;
                node = node.NextRoute;
            } while (node != null);
            Assert.AreEqual(3, i);
        }
    }
}
