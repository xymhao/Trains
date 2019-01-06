using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Trains;

namespace TrainTests
{
    public class RailroadUtilsTests
    {
        private RailroadUtils railroadUtils;
        [SetUp]
        public void Setup()
        {
            var graph = new Graph("AB5,BC4,CD8,DC8,DE6,AD5,CE2,EB3,AE7");
            railroadUtils = new RailroadUtils(graph);
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
            var distance = railroadUtils.GetDistanceOfRoutes(routes);
            Assert.AreEqual(result, distance.Trim());
        }

        [Test]
        public void GetNumberWithMaximumTests()
        {
            var result = railroadUtils.GetNumberWithMaximum("C", "C", 3);
            Assert.AreEqual(2, result);
        }

        [Test]
        public void GetNumberWithExactlyStopsTests()
        {
            var result = railroadUtils.GetNumberWithExactlyStops("A", "C", 4);
            Assert.AreEqual(3, result);

        }

        [Test]
        public void GetNumberLessThanDistanceTests()
        {
            var result = railroadUtils.GetNumberLessThanDistance("C", "C", 30);
            Assert.AreEqual(7, result);
        }

        [Test]
        public void GetShortestDistanceTests()
        {
            var result = railroadUtils.GetShortestDistance("A", "C");
            Assert.AreEqual("9", result);
        }

    }
}
