namespace AdventOfCode.Days
{
    /// <summary> 
    ///|  Method |      Mean |     Error |    StdDev |   Gen 0 |   Gen 1 | Allocated |
    ///|-------- |----------:|----------:|----------:|--------:|--------:|----------:|
    ///| SetData | 884.88 us | 13.024 us | 12.182 us | 46.8750 | 22.4609 |    293 KB |
    ///|  Part1  |  71.86 us | 0.380 us  | 0.337 us  | 10.4980 |      -  |     65 KB |
    ///|  Part2  | 190.19 us | 0.831 us  | 0.778 us  | 15.3809 | 0.2441  |     94 KB |
    /// Row improvement
    /// | Method |      Mean |    Error  |   StdDev  |   Gen 0 |  Gen 1  | Allocated |
    /// |------- |----------:|---------: |---------: |--------:|-------: |----------:|
    /// |  Part1 |  54.33 us | 0.550 us  | 0.515 us  |  7.9346 |      -  |     49 KB |
    /// |  Part2 | 172.58 us | 2.596 us  | 2.549 us  | 13.4277 | 0.2441  |     84 KB |
    /// Col improvement                                                  
    /// | Method |      Mean |    Error  |   StdDev  |   Gen 0 |  Gen 1  | Allocated |
    /// |------- |----------:|---------: |---------: |--------:|-------: |----------:|
    /// |  Part1 |  39.39 us | 0.103 us  | 0.097 us  |  2.6245 |      -  |     16 KB |
    /// |  Part2 | 160.89 us | 0.880 us  | 0.823 us  | 10.9863 | 0.2441  |     68 KB |
    /// Row+Col improvements                              
    /// | Method |      Mean |    Error  |   StdDev  |  Gen 0  | Allocated |
    /// |------- |----------:|---------: |---------: |-------: |----------:|
    /// |  Part1 |  31.01 us | 0.136 us  | 0.113 us  |      -  |    288  B |
    /// |  Part2 | 149.04 us | 2.176 us  | 1.929 us  | 9.2773  |  58.68 KB |
    /// </summary>
    [MemoryDiagnoser]
    public class Day4
    {
        private const string Filename = "Data/Day4/input.txt";

        public IEnumerable<string> Data;

        const int MatrixSize = 5;

        IEnumerable<int> CalledNumbers;
        List<int?[][]> ScoreSheets;

        public Day4(string filename = Filename)
        {
            Data = File.ReadLines(filename);
            SetData();
        }

        //[Benchmark]
        public void SetData()
        {
            CalledNumbers = GetCalledNumbers();
            ScoreSheets = PopulateScoreSheets(MatrixSize);
        }

        [Benchmark]
        public Result Part1()
        {
            foreach (var number in CalledNumbers)
            {
                // mark numbers
                foreach (var scoreSheet in ScoreSheets)
                    MarkNumber(number, scoreSheet);

                // check for winner
                foreach (var scoreSheet in ScoreSheets)
                {
                    if (RowWins(scoreSheet))
                        return GetResult(number, scoreSheet);

                    if (ColWins(scoreSheet))
                        return GetResult(number, scoreSheet);
                }
            }

            return new Result();
        }

        [Benchmark]
        public Result? Part2()
        {
            var result = new List<Result>();

            var winningSheets = new HashSet<int>();

            foreach (var number in CalledNumbers)
            {
                // mark numbers
                foreach (var scoreSheet in ScoreSheets)
                    MarkNumber(number, scoreSheet);

                // check for winners
                for (var sheet = 0; sheet < ScoreSheets.Count; sheet++)
                {
                    if (winningSheets.Contains(sheet))
                        continue;

                    var scoreSheet = ScoreSheets[sheet];

                    if (RowWins(scoreSheet))
                    {
                        winningSheets.Add(sheet);
                        result.Add(GetResult(number, scoreSheet));
                    }

                    // loop over cols
                    if (ColWins(scoreSheet))
                    {
                        winningSheets.Add(sheet);
                        result.Add(GetResult(number, scoreSheet));
                    }

                    // is last winning sheet
                    if (winningSheets.Count == ScoreSheets.Count)
                        return result.Last();
                }
            }

            return null;
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

        private void MarkNumber(int number, int?[][] scoreSheet)
        {
            foreach (var row in scoreSheet)
            {
                int pos = Array.IndexOf(row, number);

                if (pos > -1)
                    row[pos] = null;
            }
        }

        private bool RowWinsLinq(int?[][] scoreSheet)
        {
            return scoreSheet.Any(x => x.All(z => z == null));
        }

        private bool RowWins(int?[][] scoreSheet)
        {
            for (var row = 0; row < MatrixSize; row++)
            {
                int count = 0;
                for (var col = 0; col < MatrixSize; col++)
                {
                    if (scoreSheet[row][col] == null)
                        count++;
                }

                if (count == MatrixSize)
                    return true;
            }

            return false;
        }

        private bool ColWinsLinq(int?[][] scoreSheet)
        {
            for (var i = 0; i < MatrixSize; i++)
                if (scoreSheet.Select(row => row[i]).All(z => z == null))
                    return true;

            return false;
        }

        private bool ColWins(int?[][] scoreSheet)
        {
            for (int col = 0; col < MatrixSize; col++)
            {
                var count = 0;
                for (int row = 0; row < MatrixSize; row++)
                {
                    if (scoreSheet[row][col] == null)
                        count++;
                }

                if (count == MatrixSize)
                    return true;
            }

            return false;
        }

        private Result GetResult(int number, int?[][] scoreSheet)
        {
            var result = new Result();
            result.SumOfUnmarked = scoreSheet.SelectMany(x => x).Sum().Value;
            result.LastCalledNumber = number;

            return result;
        }

        public struct Result
        {
            public int SumOfUnmarked { get; set; }
            public int LastCalledNumber { get; set; }

            public int Total { get { return SumOfUnmarked * LastCalledNumber; } }
        }
    }
}
