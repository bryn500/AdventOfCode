namespace Tests.Days
{
    [TestClass]
    public class Day3Tests
    {
        [TestMethod]
        public void Part1()
        {
            var day = new Day3(5);
            var result = day.Part1();
            Assert.AreEqual(198, result);
        }

        [TestMethod]
        public void Part2()
        {
            var day1 = new Day3(5);
            var result = day1.Part2();
            Assert.AreEqual(230, result);
        }
    }
}