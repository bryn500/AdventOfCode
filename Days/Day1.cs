namespace AdventOfCode.Days
{
    /// <summary>
    ///|                     Method |       Mean |      Error |    StdDev |   Gen 0 | Allocated |
    ///|--------------------------- |-----------:|-----------:|----------:|--------:|----------:|
    ///|         GetIncreasesSimple |   3.707 us |  0.0362 us | 0.0339 us |       - |         - |
    ///| GetWindowedIncreasesSimple | 198.255 us |  1.7352 us | 1.6231 us | 30.5176 | 191,712 B |
    ///|               GetIncreases | 188.817 us |  0.6213 us | 0.5812 us |       - |      40 B |
    ///|       GetWindowedIncreases | 907.066 us | 10.4398 us | 9.2546 us | 50.7813 | 319,560 B |
    /// </summary>
    [MemoryDiagnoser]
    public class Day1
    {
        private const int WindowSize = 3;

        public List<ReportData> ReportList { get; set; }

        private List<int> Depths;

        public Day1()
        {
            ReportList = new List<ReportData>();
            Depths = new List<int>();            
        }

        [GlobalSetup]
        public void SetData()
        {
            var lines = File.ReadLines("Data/Day1/input.txt");

            foreach (var depth in lines.Where(x => !string.IsNullOrWhiteSpace(x)))
                ReportList.Add(new ReportData(int.Parse(depth), ReportList, WindowSize));

            Depths = lines
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => int.Parse(x))
                .ToList();
        }

        [Benchmark]
        public int GetIncreasesSimple()
        {
            var count = 0;

            // loop over and increment if current is larger than previous
            for (var i = 1; i < Depths.Count; i++)
                if (Depths[i] > Depths[i - 1])
                    count++;

            return count;
        }

        [Benchmark]
        public int GetWindowedIncreasesSimple()
        {
            var count = 0;
            var prevSum = 0;

            // skip items without large enough window, loop over remaining
            for (var i = WindowSize; i < Depths.Count; i++)
            {
                // get the sum of the last 3 
                var windowSum = Depths
                    .Skip(i - WindowSize)
                    .Take(WindowSize)
                    .Sum();

                // increment if larger than previous
                if (windowSum > prevSum)
                    count++;

                // store for next run
                prevSum = windowSum;
            }

            return count;
        }

        [Benchmark]
        public int GetIncreases()
        {
            // Count items with an increase
            return ReportList.Count(x =>
                x.IncreasedSinceLast.HasValue &&
                x.IncreasedSinceLast.Value);
        }

        [Benchmark]
        public int GetWindowedIncreases()
        {
            // Count items with a windowed increase
            return ReportList.Count(x =>
                x.WindowIncreased.HasValue &&
                x.WindowIncreased.Value);
        }
    }

    public class ReportData
    {
        private readonly int _windowSize;
        private IEnumerable<ReportData> _window;

        public int Value { get; set; }

        /// <summary>
        /// gets the previous instance
        /// </summary>
        public ReportData? Prev => _window.Any() ? _window.Last() : null;

        /// <summary>
        /// gets the sum of previous values in the allotted window
        /// </summary>
        public int? SumOfWindow => _window?.Sum(x => x.Value);

        /// <summary>
        /// returns whether the Value has increased since previous
        /// </summary>
        public bool? IncreasedSinceLast => Value > Prev?.Value;

        /// <summary>
        /// returns if the sum of the allotted window has increased since the previous iteration
        /// will return null if previous window is lower than the default window size
        /// </summary>
        public bool? WindowIncreased => _window.Count() < _windowSize ? null : SumOfWindow > Prev?.SumOfWindow;

        public ReportData(int val, IEnumerable<ReportData> reportList, int windowSize)
        {
            _windowSize = windowSize;

            Value = val;

            // create window from last 3 items or fewer
            _window = reportList
                        .Skip(reportList.Count() - _windowSize)
                        .Take(_windowSize)
                        .ToList();
        }
    }
}
