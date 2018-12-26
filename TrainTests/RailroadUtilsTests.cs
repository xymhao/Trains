using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Trains;

namespace TrainTests
{
    public class RailroadUtilsTests
    {
        private TrainGraph trainMaps;
        [SetUp]
        public void Setup()
        {
            var graph = new Graph("AB5,BC4,CD8,DC8,DE6,AD5,CE2,EB3,AE7");
            trainMaps = new TrainGraph(graph);
        }

        [Test]
        [TestCase("A-B", "5")]
        [TestCase("A-D", "5")]
        [TestCase("A-B-C", "9")]
        [TestCase("A-D-C", "13")]
        [TestCase("A-B-C-D", "17")]
        [TestCase("A-E-B-C-D", "22")]
        [TestCase("A-E-D", "NO SUCH ROUTE")]
        public void GetDistanceOfRoutes(string routes, string result)
        {
            var distance = trainMaps.GetDistanceOfRoutes(routes);
            Assert.AreEqual(result, distance);
        }
    }
}
