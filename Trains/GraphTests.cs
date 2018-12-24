using System;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Trains.Models;

namespace TrainsTests
{
    public class GraphTests
    {
        private Graph _graph; 

        [SetUp]
        public void Setup()
        {
            _graph = new Graph("AB5,BC4,CD8,DC8,DE6,AD5,CE2,EB3,AE7");
        }

        [Test]
        [TestCase("A", "B", 5)]
        [TestCase("B", "C", 4)]
        [TestCase("C", "D", 8)]
        [TestCase("D", "C", 8)]
        [TestCase("D", "E", 6)]
        [TestCase("A", "D", 5)]
        [TestCase("C", "E", 2)]
        [TestCase("E", "B", 3)]
        [TestCase("A", "E", 7)]
        public void GetArcListTests(string start, string end, decimal weight)
        {
            decimal? arcWeight = _graph.ArcList.FindLast(arc=>arc.Start.Equals(start) && arc.End.Equals(end))?.Weight;
            Assert.IsNotNull(arcWeight);
            Assert.AreEqual(weight, weight);
        }
    }
}
