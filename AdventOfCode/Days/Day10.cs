namespace AdventOfCode.Days
{
    /// <summary>
    /// </summary>
    [MemoryDiagnoser]
    public class Day10
    {
        private const string Filename = "Data/Day10/input.txt";

        public IEnumerable<string> Data;

        public Day10(string filename = Filename)
        {
            Data = File.ReadLines(filename);
        }

        [Benchmark]
        public int Part1()
        {
            return 0;
        }

        [Benchmark]
        public long Part2()
        {
            return 0;
        }
    }    
}
