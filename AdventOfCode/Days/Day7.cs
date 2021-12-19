namespace AdventOfCode.Days
{
    /// <summary>
    ///| Method |     Mean |     Error |    StdDev |   Gen 0 |  Gen 1 | Allocated |
    ///|------- |---------:|----------:|----------:|--------:|-------:|----------:|
    ///|  Part1 | 3.467 ms | 0.0145 ms | 0.0129 ms | 19.5313 | 3.9063 |     67 KB |
    ///|  Part2 | 4.675 ms | 0.0906 ms | 0.1007 ms | 15.6250 |      - |     67 KB |
    /// </summary>
    [MemoryDiagnoser]
    public class Day7
    {
        private const string Filename = "Data/Day7/input.txt";
        public IEnumerable<string> Data;

        public Day7(string filename = Filename)
        {
            Data = File.ReadLines(filename);
        }

        public List<int> ParseFile()
        {
            return Data.First().Split(',').Select(x => int.Parse(x)).ToList();
        }

        public int GetTotalFuelCost(List<int> data, int target)
        {
            var result = 0;

            foreach (var val in data)
                result += Math.Abs(val - target);

            return result;
        }

        public int GetTotalFuelCost2(List<int> data, int target)
        {
            // 1,3,6,10
            // triangular number
            // make a rectangle and then half it

            var result = 0;

            foreach (var val in data)
            {
                var diff = Math.Abs(val - target);                
                result += diff * (diff + 1) / 2;
            }

            return result;
        }

        public IEnumerable<int> GetPossibleRange(List<int> data)
        {
            var result = Enumerable.Range(data.Min(), Math.Abs(data.Min() - data.Max()) + 1);
            return result;
        }

        [Benchmark]
        public int Part1()
        {
            var inputs = ParseFile();

            var result = int.MaxValue;

            foreach (var test in GetPossibleRange(inputs))
                result = Math.Min(result, GetTotalFuelCost(inputs, test));

            return result;
        }

        [Benchmark]
        public int Part2()
        {
            var inputs = ParseFile();

            var result = int.MaxValue;

            foreach (var test in GetPossibleRange(inputs))
                result = Math.Min(result, GetTotalFuelCost2(inputs, test));

            return result;
        }
    }
}