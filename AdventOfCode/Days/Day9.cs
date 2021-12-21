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

        public int[][] ParseFile()
        {
            var allLines = Data.ToList();

            int[][] map = new int[allLines.Count][];

            for (var i = 0; i < allLines.Count; i++)
            {
                map[i] = new int[allLines[i].Length];

                for (var c = 0; c < allLines[i].Length; c++)
                {
                    var val = allLines[i][c];
                    var num = char.GetNumericValue(val);
                    map[i][c] = (int)num;
                }
            }
                

            return map;
        }

        [Benchmark]
        public int Part1()
        {
            var data = ParseFile();
            var lowPoints = new List<int>();

            for (var i = 0; i < data.Length; i++)
            {
                for (var c = 0; c < data[i].Length; c++)
                {
                    var currentVal = data[i][c];
                    var pointsToCheck = new List<int>();

                    if (i != 0)
                        pointsToCheck.Add(data[i - 1][c]);
                    if (i != data.Length - 1)
                        pointsToCheck.Add(data[i + 1][c]);
                    if (c != 0)
                        pointsToCheck.Add(data[i][c - 1]);
                    if (c != data[i].Length - 1)
                        pointsToCheck.Add(data[i][c + 1]);

                    if(pointsToCheck.All(x => x > currentVal))
                        lowPoints.Add(currentVal);
                }
            }

            return lowPoints.Sum(x => 1 + x);
        }

        [Benchmark]
        public long Part2()
        {
            var data = ParseFile();

            return 0;
        }
    }
}
