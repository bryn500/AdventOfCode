namespace Tests.Days
{
    [TestClass]
    public class Day5Tests
    {
        [TestMethod]
        public void Part1()
        {
            var day5 = new Day5();
            var result = day5.Part1();
            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void Part2()
        {
            var day5 = new Day5();
            var result = day5.Part2();
            Assert.AreEqual(12, result);
        }
    }
}