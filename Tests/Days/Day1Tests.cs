namespace Tests.Days
{
    [TestClass]
    public class Day1Tests
    {
        [TestMethod]
        public void Part1Simple()
        {
            var day = new Day1();
            var result = day.Part1Simple();
            Assert.AreEqual(7, result);
        }

        [TestMethod]
        public void Part2Simple()
        {
            var day = new Day1();
            var result = day.Part2Simple();
            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void Part1()
        {
            var day = new Day1();
            var result = day.Part1();
            Assert.AreEqual(7, result);
        }

        [TestMethod]
        public void Part2()
        {
            var day = new Day1();
            var result = day.Part2();
            Assert.AreEqual(5, result);
        }
    }
}