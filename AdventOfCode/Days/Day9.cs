namespace AdventOfCode.Days
{
    /// <summary>
    /// | Method |     Mean |     Error |    StdDev |    Gen 0 |    Gen 1 | Allocated |
    /// |------- |---------:|----------:|----------:|---------:|---------:|----------:|
    /// |  Part1 | 1.522 ms | 0.0299 ms | 0.0378 ms | 333.9844 |  66.4063 |      2 MB |
    /// |  Part2 | 2.845 ms | 0.0531 ms | 0.0827 ms | 566.4063 | 238.2813 |      3 MB |
    /// </summary>
    [MemoryDiagnoser]
    public class Day9
    {
        private const string Filename = "Data/Day9/input.txt";

        public IEnumerable<string> Data;

        int recursiveCount = 0;

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
            var map = ParseFile();
            var lowPoints = GetLowPoints(map);
            return lowPoints.Sum(x => 1 + map[x.Row][x.Col]);
        }

        [Benchmark]
        public long Part2()
        {
            var map = ParseFile();
            var lowPoints = GetLowPoints(map);

            var basins = new List<HashSet<D9Point>>();

            // loop over lowpoints and get basins
            foreach (var lowPoint in lowPoints)
                basins.Add(GetBasin(map, new HashSet<D9Point>(), lowPoint));

            var result = basins
                .Select(x => x.Count)
                .OrderByDescending(x => x)
                .Take(3)
                .Aggregate((x, y) => x * y);

            return result;
        }

        private HashSet<D9Point> GetLowPoints(int[][] map)
        {
            var lowPoints = new HashSet<D9Point>();

            for (var i = 0; i < map.Length; i++)
            {
                for (var c = 0; c < map[i].Length; c++)
                {
                    var currentVal = map[i][c];
                    var pointsToCheck = new List<int>();

                    if (i != 0)
                        pointsToCheck.Add(map[i - 1][c]);
                    if (i != map.Length - 1)
                        pointsToCheck.Add(map[i + 1][c]);
                    if (c != 0)
                        pointsToCheck.Add(map[i][c - 1]);
                    if (c != map[i].Length - 1)
                        pointsToCheck.Add(map[i][c + 1]);

                    if (pointsToCheck.All(x => x > currentVal))
                        lowPoints.Add(new D9Point(i, c, currentVal));
                }
            }

            return lowPoints;
        }

        private HashSet<D9Point> GetBasin(int[][] map, HashSet<D9Point> basin, D9Point lowpoint)
        {
            recursiveCount++;

            if (lowpoint.Depth == 9)
                return basin;

            if (!basin.Add(lowpoint))
                return basin;

            if (lowpoint.Row != 0)
                basin = GetBasin(map, basin, new D9Point(lowpoint.Row - 1, lowpoint.Col, map[lowpoint.Row - 1][lowpoint.Col]));
            if (lowpoint.Row != map.Length - 1)
                basin = GetBasin(map, basin, new D9Point(lowpoint.Row + 1, lowpoint.Col, map[lowpoint.Row + 1][lowpoint.Col]));
            if (lowpoint.Col != 0)
                basin = GetBasin(map, basin, new D9Point(lowpoint.Row, lowpoint.Col - 1, map[lowpoint.Row][lowpoint.Col - 1]));
            if (lowpoint.Col != map[lowpoint.Row].Length - 1)
                basin = GetBasin(map, basin, new D9Point(lowpoint.Row, lowpoint.Col + 1, map[lowpoint.Row][lowpoint.Col + 1]));

            return basin;
        }        
    }
    public struct D9Point : IEquatable<D9Point>
    {
        public int Depth { get; }
        public int Row { get; }
        public int Col { get; }

        public D9Point(int row, int col, int depth)
        {
            Row = row;
            Col = col;
            Depth = depth;
        }

        public bool Equals(D9Point other)
        {
            return Equals(other, this);
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var objectToCompareWith = (D9Point)obj;

            return objectToCompareWith.Row == Row && objectToCompareWith.Col == Col;
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 31 + Row.GetHashCode();
                hash = hash * 31 + Col.GetHashCode();
                return hash;
            }
        }
    }
}
