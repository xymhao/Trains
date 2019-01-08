using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Trains;

namespace TrainTests
{
    public class RailroadUtilsTests
    {
        private Railroad railroad;
        [SetUp]
        public void Setup()
        {
            railroad = new Railroad("AB5,BC4,CD8,DC8,DE6,AD5,CE2,EB3,AE7");
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
            var distance = railroad.GetDistanceOfRoutes(routes);
            Assert.AreEqual(result, distance.Trim());
        }

        [Test]
        public void GetNumberWithMaximumTests()
        {
            var result = railroad.GetNumberWithMaximum("C", "C", 3);
            Assert.AreEqual(2, result);
        }

        [Test]
        public void GetNumberWithExactlyStopsTests()
        {
            var result = railroad.GetNumberWithExactlyStops("A", "C", 4);
            Assert.AreEqual(3, result);

        }

        [Test]
        public void GetNumberLessThanDistanceTests()
        {
            var result = railroad.GetNumberLessThanDistance("C", "C", 30);
            Assert.AreEqual(7, result);
        }

        [Test]
        public void GetShortestDistanceTests()
        {
            var result = railroad.GetShortestDistance("A", "C");
            Assert.AreEqual("9", result);
        }

        [Test]
        public void DurationOfRouteABC_Return11()
        {
            string duration = railroad.GetDuration("A-B-C");
            Assert.AreEqual("11", duration);
        }

        [Test]
        public void DurationOfRouteAD_Return5()
        {
            string duration = railroad.GetDuration("A-D");
            Assert.AreEqual("5", duration);
        }

        [Test]
        public void DurationOfRouteAED_Return_NOSUCHROUTE()
        {
            string duration = railroad.GetDuration("A-E-D");
            Assert.AreEqual("NO SUCH ROUTE", duration);
        }

        [Test]
        public void Maxinum30OfRouteCToC_Return_4()
        {
            int result = railroad.GetNumberWithMaximumDuration("C", "C", 30);
            Assert.AreEqual(4,result);
        }
    }
}
