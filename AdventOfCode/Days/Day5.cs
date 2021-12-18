namespace AdventOfCode.Days
{
    /// <summary>
    /// Storing grid for each chart
    ///| Method |         Mean |      Error |     StdDev |      Gen 0 |      Gen 1 |      Gen 2 | Allocated |
    ///|------- |-------------:|-----------:|-----------:|-----------:|-----------:|-----------:|----------:|
    ///|  Part1 |     5.768 ms |  0.0638 ms |  0.0597 ms |   992.1875 |   992.1875 |   992.1875 |      4 MB |
    ///|  Part2 | 1,306.613 ms | 26.0929 ms | 60.4742 ms | 86000.0000 | 86000.0000 | 86000.0000 |    795 MB |
    /// 
    /// Don't store each grid, calculate result only
    ///| Method |     Mean     |     Error  |    StdDev  |    Gen 0   |    Gen 1   |    Gen 2   | Allocated |
    ///|------- |---------:    |----------: |----------: |---------:  |---------:  |---------:  |----------:|
    ///|  Part1 |     5.779 ms | 0.0705 ms  | 0.0659 ms  | 992.1875   | 992.1875   | 992.1875   |      4 MB |
    ///|  Part2 |     6.010 ms | 0.1162 ms  | 0.1291 ms  | 992.1875   | 992.1875   | 992.1875   |      4 MB |
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
            {
                var done = false;
                var x = chartData.StartX;
                int y = chartData.StartY;
                while (!done)
                {
                    chart[x, y]++;
                    if (chartData.StartY < chartData.EndY)
                    {
                        y++;
                        if (y > chartData.EndY)
                            done = true;
                    }
                    else if (chartData.StartY > chartData.EndY)
                    {
                        y--;
                        if (y < chartData.EndY)
                            done = true;
                    }
                    if (chartData.StartX < chartData.EndX)
                    {
                        x++;
                        if (x > chartData.EndX)
                            done = true;
                    }
                    else if (chartData.StartX > chartData.EndX)
                    {
                        x--;
                        if (x < chartData.EndX)
                            done = true;
                    }
                }
            }                

            return chart;
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

            public bool IsHorizontalLine => StartY == EndY;
            public bool IsVerticalLine => StartX == EndX;

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
