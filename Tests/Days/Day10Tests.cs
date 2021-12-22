using System.Collections.Generic;

namespace Tests.Days
{
    [TestClass]
    public class Day10Tests
    {
        [TestMethod]
        public void Part1()
        {
            var day10 = new Day10();
            var result = day10.Part1();
            Assert.AreEqual(26397, result);
        }

        [TestMethod]
        public void Part2()
        {
            var day10 = new Day10();
            var result = day10.Part2();
            Assert.AreEqual(288957, result);
        }
    }
}