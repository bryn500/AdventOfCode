namespace AdventOfCode.Days
{
    /// <summary>
    /// </summary>
    [MemoryDiagnoser]
    public class Day9
    {
        private const string Filename = "Data/Day9/input.txt";

        public IEnumerable<string> Data;

        public Day9(string filename = Filename)
        {
            Data = File.ReadLines(filename);
        }

        public List<Tuple<string[], string[]>> ParseFile()
        {
            var result = new List<Tuple<string[], string[]>>();

            foreach (var line in Data)
            {
                var split = line.Split(" | ");
                var inputs = split[0].Trim().Split(' ');
                var outputs = split[1].Trim().Split(' ');
                result.Add(new Tuple<string[], string[]>(inputs, outputs));
            }

            return result;
        }

        [Benchmark]
        public int Part1()
        {
            var data = ParseFile();
            
            return 0;
        }

        [Benchmark]
        public long Part2()
        {
            var data = ParseFile();            

            return 0;
        }
    }
}
