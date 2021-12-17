namespace AdventOfCode.Days
{
    /// <summary> 
    /// Inefficient, needs improving.
    ///| Method |     Mean |     Error |    StdDev |     Gen 0 |    Gen 1 | Allocated |
    ///|------- |---------:|----------:|----------:|----------:|---------:|----------:|
    ///|  Part1 | 3.632 ms | 0.0700 ms | 0.0833 ms |  488.2813 |  78.1250 |      3 MB |
    ///|  Part2 | 9.342 ms | 0.1822 ms | 0.1789 ms | 1218.7500 | 156.2500 |      7 MB |
    /// </summary>
    [MemoryDiagnoser]
    public class Day4
    {
        private const string Filename = "Data/Day4/input.txt";

        public IEnumerable<string> Data;

        public Day4(string filename = Filename)
        {
            Data = File.ReadLines(filename);
        }

        [Benchmark]
        public Result Part1()
        {
            int rowSize = 5;
            IEnumerable<int> calledNumbers = GetCalledNumbers();
            List<int?[][]> scoreSheets = PopulateScoreSheets(rowSize);

            foreach (var number in calledNumbers)
            {
                foreach (var scoreSheet in scoreSheets)
                {
                    foreach (var line in scoreSheet)
                    {
                        MarkNumber(number, line);
                    }
                }

                foreach (var scoreSheet in scoreSheets)
                {
                    for (var i = 0; i < rowSize; i++)
                    {
                        var col = scoreSheet.Select(row => row[i]).ToArray();
                        if (col.All(z => z == null))
                            return GetResult(number, scoreSheet);
                    }

                    var nullRows = scoreSheet.Where(x => x.All(z => z == null));

                    if (nullRows.Any())
                        return GetResult(number, scoreSheet);
                }
            }

            return new Result();
        }

        [Benchmark]
        public Result Part2()
        {
            int rowSize = 5;
            IEnumerable<int> calledNumbers = GetCalledNumbers();
            List<int?[][]> scoreSheets = PopulateScoreSheets(rowSize);

            var result = new List<Result>();

            var winningSheets = new HashSet<int>();

            foreach (var number in calledNumbers)
            {
                var t = 1;

                foreach (var scoreSheet in scoreSheets)
                {
                    foreach (var line in scoreSheet)
                    {
                        MarkNumber(number, line);
                    }
                }

                for (var sheet = 0; sheet < scoreSheets.Count; sheet++)
                {
                    if (winningSheets.Contains(sheet))
                        continue;

                    var scoreSheet = scoreSheets[sheet];

                    for (var i = 0; i < rowSize; i++)
                    {
                        var col = scoreSheet.Select(row => row[i]).ToArray();
                        if (col.All(z => z == null))
                        {
                            winningSheets.Add(sheet);
                            result.Add(GetResult(number, scoreSheet));
                        }
                    }

                    var nullRows = scoreSheet.Where(x => x.All(z => z == null));

                    if (nullRows.Any())
                    {
                        winningSheets.Add(sheet);
                        result.Add(GetResult(number, scoreSheet));
                    }

                    if (winningSheets.Count == scoreSheets.Count)
                        return result.Last();
                }
            }

            return new Result();
        }

        private void MarkNumber(int number, int?[] line)
        {
            int pos = Array.IndexOf(line, number);

            if (pos > -1)
                line[pos] = null;
        }

        private Result GetResult(int number, int?[][] scoreSheet)
        {
            var result = new Result();
            result.SumOfUnmarked = scoreSheet.SelectMany(x => x).Sum().Value;
            result.LastCalledNumber = number;

            return result;
        }

        private IEnumerable<int> GetCalledNumbers()
        {
            return Data.First().Split(',').Select(x => int.Parse(x)).ToArray();
        }

        private List<int?[][]> PopulateScoreSheets(int rowSize)
        {
            var scoreSheets = new List<int?[][]>();

            int sheetindex = 0;
            int lineindex = 0;
            var currentSheet = new int?[rowSize][];

            foreach (var line in Data.Skip(1))
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    sheetindex++;
                    lineindex = 0;
                    currentSheet = new int?[rowSize][];
                    continue;
                }

                currentSheet[lineindex] = line.Split(' ')
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => (int?)int.Parse(x))
                    .ToArray();

                if (lineindex == 4)
                    scoreSheets.Add(currentSheet);

                lineindex++;
            }

            return scoreSheets;
        }

        public struct Result
        {
            public int SumOfUnmarked { get; set; }
            public int LastCalledNumber { get; set; }

            public int Total { get { return SumOfUnmarked * LastCalledNumber; } }
        }
    }
}
