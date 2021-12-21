namespace Tests.Days
{
    [TestClass]
    public class Day9Tests
    {
        [TestMethod]
        public void Part1()
        {
            var day9 = new Day9();
            var result = day9.Part1();
            Assert.AreEqual(15, result);
        }

        [TestMethod]
        public void Part2()
        {
            var day9 = new Day9();
            var result = day9.Part2();
            Assert.AreEqual(1134, result);
        }

        [TestMethod]
        public void TestComparer()
        {
            var point = new D9Point(1, 2, 3);
            var point2 = new D9Point(1, 2, 4);

            var point3 = new D9Point(2, 2, 3);
            var point4 = new D9Point(1, 1, 3);

            Assert.AreEqual(point, point2);
            Assert.IsTrue(point.Equals(point2));
            Assert.IsFalse(point.Equals(point3));
            Assert.IsFalse(point.Equals(point4));
        }
    }
}