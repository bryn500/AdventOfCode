namespace Tests.Days
{
    [TestClass]
    public class Day2Tests
    {
        [TestMethod]
        public void GetIncreasesSimple()
        {
            var day = new Day2();
            var result = day.Part1();
            Assert.AreEqual(150, result);
        }

        [TestMethod]
        public void GetWindowedIncreasesSimple()
        {
            var day = new Day2();
            var result = day.Part2();
            Assert.AreEqual(900, result);
        }
    }
}