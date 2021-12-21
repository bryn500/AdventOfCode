namespace AdventOfCode.Days
{
    /// <summary>
    /// | Method |        Mean |    Error |   StdDev |     Gen 0 |   Gen 1 | Allocated |
    /// |------- |------------:|---------:|---------:|----------:|--------:|----------:|
    /// |  Part1 |    641.2 us | 12.57 us | 14.48 us |   38.0859 |  1.9531 |    238 KB |
    /// |  Part2 | 10,042.3 us | 77.54 us | 72.54 us | 1375.0000 | 46.8750 |  8,488 KB |
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
                var patterns = line.Item1;
                var outputs = line.Item2;

                var maps = new Dictionary<int, string>();

                // add map from length to number value
                // potentially skip this
                foreach (var code in patterns)
                    if (_mappings.ContainsKey(code.Length) && !maps.ContainsValue(code))
                        maps.Add(_mappings[code.Length], code);

                // figure out inputs of length 6
                foreach (var val in patterns.Where(x => x.Length == 6))
                    if (maps[1].Except(val).Count() == 1)
                        maps[6] = val;
                    else if (!maps[4].Except(val).Any())
                        maps[9] = val;
                    else
                        maps[0] = val;

                // figure out inputs of length 5
                foreach (var val in patterns.Where(x => x.Length == 5))
                    if (!maps[1].Except(val).Any())
                        maps[3] = val;
                    else if (maps[4].Except(val).Count() == 2)
                        maps[2] = val;
                    else
                        maps[5] = val;

                var no = "";

                foreach (var output in outputs)
                    foreach (var n in maps)
                        if (string.Concat(n.Value.OrderBy(c => c)) == string.Concat(output.OrderBy(c => c)))
                            no += n.Key.ToString();

                count += int.Parse(no);
            }

            return count;
        }
    }
}
