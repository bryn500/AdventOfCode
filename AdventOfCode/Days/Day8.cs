namespace AdventOfCode.Days
{
    /// <summary>
    /// | Method |        Mean |    Error |   StdDev |     Gen 0 |   Gen 1 | Allocated |
    /// |------- |------------:|---------:|---------:|----------:|--------:|----------:|
    /// |  Part1 |    641.2 us | 12.57 us | 14.48 us |   38.0859 |  1.9531 |    238 KB |
    /// |  Part2 | 10,042.3 us | 77.54 us | 72.54 us | 1375.0000 | 46.8750 |  8,488 KB |
    /// Order chars when added to map and invert map
    /// |  Part1 |   621.5 us  |  7.06 us |  6.60 us |  38.0859 |   1.9531 |    238 KB |
    /// |  Part2 | 3,888.9 us  | 34.83 us | 30.88 us | 500.0000 | 125.0000 |  3,093 KB |
    /// </summary>
    [MemoryDiagnoser]
    public class Day8
    {
        private const string Filename = "Data/Day8/input.txt";

        public IEnumerable<string> Data;

        private Dictionary<int, int> _mappings = new Dictionary<int, int>()
        {
            //1, 4, 7, and 8 can be gathered from number of chars
            {2, 1},
            {4, 4},
            {3, 7},
            {7, 8}
        };

        public Day8(string filename = Filename)
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

            int count = 0;
            foreach (var line in data)
                foreach (var output in line.Item2)
                    if (_mappings.ContainsKey(output.Length))
                        count++;

            return count;
        }

        [Benchmark]
        public long Part2()
        {
            var data = ParseFile();

            int count = 0;
            foreach (var line in data)
            {
                var maps = new Dictionary<int, string>();
                var mapInverted = new Dictionary<string, int>();

                // add map from length to number value
                foreach (var code in line.Item1)
                {
                    if (_mappings.ContainsKey(code.Length) && !maps.ContainsValue(code))
                    {
                        maps.Add(_mappings[code.Length], OrderStringByChar(code));
                        mapInverted.Add(OrderStringByChar(code), _mappings[code.Length]);
                    }
                }

                // figure out inputs of length 6
                foreach (var val in line.Item1.Where(x => x.Length == 6))
                    if (maps[1].Except(val).Count() == 1)
                        mapInverted.Add(OrderStringByChar(val), 6);
                    else if (!maps[4].Except(val).Any())
                        mapInverted.Add(OrderStringByChar(val), 9);
                    else
                        mapInverted.Add(OrderStringByChar(val), 0);

                // figure out inputs of length 5
                foreach (var val in line.Item1.Where(x => x.Length == 5))
                    if (!maps[1].Except(val).Any())
                        mapInverted.Add(OrderStringByChar(val), 3);
                    else if (maps[4].Except(val).Count() == 2)
                        mapInverted.Add(OrderStringByChar(val), 2);
                    else
                        mapInverted.Add(OrderStringByChar(val), 5);

                var num = "";

                foreach (var output in line.Item2)
                {
                    var ordered = OrderStringByChar(output);

                    if (mapInverted.ContainsKey(ordered))
                        num += mapInverted[ordered].ToString();
                }

                count += int.Parse(num);
            }

            return count;
        }

        private static string OrderStringByChar(string val)
        {
            return string.Concat(val.OrderBy(c => c));
        }
    }
}
