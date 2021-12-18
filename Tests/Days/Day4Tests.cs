namespace Tests.Days
{
    [TestClass]
    public class Day4Tests
    {
        [TestMethod]
        public void Part1_GetsRow()
        {
            var day = new Day4();
            var result = day.Part1();
            Assert.AreEqual(24, result.Value.LastCalledNumber);
            Assert.AreEqual(188, result.Value.SumOfUnmarked);
            Assert.AreEqual(4512, result.Value.Total);
        }

        [TestMethod]
        public void Part1_Gets_Column()
        {
            var array = new int[] {
                 13, 17,  1,  0,
                  2, 23,  4, 24,
                 21, 14, 16, 22,
                 10,  3, 18,  6,
                 12, 20, 15, 19
            };

            var day = new Day4("Data/Day4/input-column.txt");
            var result = day.Part1();
            Assert.AreEqual(11, result.Value.LastCalledNumber);
            Assert.AreEqual(array.Sum(), result.Value.SumOfUnmarked);
            Assert.AreEqual(array.Sum() * 11, result.Value.Total);
        }

        [TestMethod]
        public void Part2()
        {
            var day = new Day4();
            var result = day.Part2();
            Assert.AreEqual(13, result.Value.LastCalledNumber);
            Assert.AreEqual(148, result.Value.SumOfUnmarked);
            Assert.AreEqual(1924, result.Value.Total);
        }
    }
}