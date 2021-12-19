namespace Tests.Days
{
    [TestClass]
    public class Day7Tests
    {
        [TestMethod]
        public void Part1()
        {
            var day7 = new Day7();
            var result = day7.Part1();
            Assert.AreEqual(37, result);
        }

        [TestMethod]
        public void Part2()
        {
            var day7 = new Day7();
            var result = day7.Part2();
            Assert.AreEqual(168, result);
        }
    }
}