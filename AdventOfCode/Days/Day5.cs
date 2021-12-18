namespace AdventOfCode.Days
{
    /// <summary>
    ///| Method |         Mean |      Error |     StdDev |      Gen 0 |      Gen 1 |      Gen 2 | Allocated |
    ///|------- |-------------:|-----------:|-----------:|-----------:|-----------:|-----------:|----------:|
    ///|  Part1 |     5.768 ms |  0.0638 ms |  0.0597 ms |   992.1875 |   992.1875 |   992.1875 |      4 MB |
    ///|  Part2 | 1,306.613 ms | 26.0929 ms | 60.4742 ms | 86000.0000 | 86000.0000 | 86000.0000 |    795 MB |
    /// </summary>
    [MemoryDiagnoser]
    public class Day5
    {
        private const string Filename = "Data/Day5/input.txt";

        public IEnumerable<string> Data;

        public Day5(string filename = Filename)
        {
            Data = File.ReadLines(filename);
        }

        [Benchmark]
        public int Part1()
        {
            var data = GetChartData();
            var chart = GetChart(data);
            return GetCountOfDangerPoints(chart, 2);
        }

        [Benchmark]
        public int Part2()
        {
            var data = GetChartData();
            var chart = GetChartPart2(data);
            return GetCountOfDangerPoints(chart, 2);
        }

        private List<ChartData> GetChartData()
        {
            var data = new List<ChartData>();

            // Get chart data
            foreach (var row in Data)
            {
                var numbers = row.Split("->");

                var start = numbers[0].Trim().Split(',');
                var end = numbers[1].Trim().Split(',');

                data.Add(new ChartData(
                    int.Parse(start[0]),
                    int.Parse(end[0]),
                    int.Parse(start[1]),
                    int.Parse(end[1]))
                );
            }

            return data;
        }

        private int[,] GetChart(List<ChartData> data)
        {
            var maxX = data.Max(x => x.MaxX) + 1;
            var maxY = data.Max(x => x.MaxY) + 1;

            var chart = new int[maxX, maxY];

            foreach (var chartData in data)
            {
                if (chartData.IsVerticalLine)
                    foreach (var i in chartData.VerticalLine)
                        chart[i, chartData.StartX]++;
                else if (chartData.IsHorizontalLine)
                    foreach (var i in chartData.HorizontalLine)
                        chart[chartData.StartY, i]++;
            }

            return chart;
        }

        private int[,] GetChartPart2(List<ChartData> data)
        {
            var maxX = data.Max(x => x.MaxX);
            var maxY = data.Max(x => x.MaxY);

            var chart = new int[maxX + 1, maxY + 1];

            foreach (var chartData in data)
                chart = AddMatrix(chart, chartData.GetHits());

            return chart;
        }

        private int[,] AddMatrix(int[,] a, int[,] b)
        {
            int bBoundsX = b.GetUpperBound(0);
            int bBoundsY = b.GetUpperBound(1);

            for (var i = 0; i <= bBoundsX; i++)
                for (var j = 0; j <= bBoundsY; j++)
                    a[i, j] = a[i, j] + b[i, j];

            return a;
        }

        private int GetCountOfDangerPoints(int[,] chart, int dangerValue)
        {
            int uBound0 = chart.GetUpperBound(0);
            int uBound1 = chart.GetUpperBound(1);
            int count = 0;

            for (int i = 0; i <= uBound0; i++)
                for (int j = 0; j <= uBound1; j++)
                    if (chart[i, j] >= dangerValue)
                        count++;

            return count;
        }

        public struct ChartData
        {
            public int StartX { get; set; }
            public int EndX { get; set; }
            public int StartY { get; set; }
            public int EndY { get; set; }

            public int MinX => Math.Min(StartX, EndX);
            public int MaxX => Math.Max(StartX, EndX);
            public int MinY => Math.Min(StartY, EndY);
            public int MaxY => Math.Max(StartY, EndY);

            public IEnumerable<int> HorizontalLine => Enumerable.Range(MinX, MaxX - MinX + 1);
            public IEnumerable<int> VerticalLine => Enumerable.Range(MinY, MaxY - MinY + 1);

            public int[,] GetHits()
            {
                var hits = new int[MaxX + 1, MaxY + 1];

                var done = false;
                var x = StartX;
                int y = StartY;
                while (!done)
                {
                    hits[x, y]++;
                    if (StartY < EndY)
                    {
                        y++;
                        if (y > EndY)
                            done = true;
                    }
                    else if (StartY > EndY)
                    {
                        y--;
                        if (y < EndY)
                            done = true;
                    }
                    if (StartX < EndX)
                    {
                        x++;
                        if (x > EndX)
                            done = true;
                    }
                    else if (StartX > EndX)
                    {
                        x--;
                        if (x < EndX)
                            done = true;
                    }
                }

                return hits;
            }

            public bool IsHorizontalLine => StartY == EndY;
            public bool IsVerticalLine => StartX == EndX;
            public bool IsDiagonalLine => IsHorizontalLine && IsVerticalLine;

            public ChartData(int x0, int x1, int y0, int y1)
            {
                StartX = x0;
                EndX = x1;
                StartY = y0;
                EndY = y1;
            }
        }
    }
}
