namespace Tests.Days
{
    [TestClass]
    public class Day8Tests
    {
        [TestMethod]
        public void Part1()
        {
            var day8 = new Day8();
            var result = day8.Part1();
            Assert.AreEqual(26, result);
        }

        [TestMethod]
        public void Part2()
        {
            var day8 = new Day8();
            var result = day8.Part2();
            Assert.AreEqual(61229, result);
        }
    }
}