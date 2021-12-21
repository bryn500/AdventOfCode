namespace AdventOfCode.Days
{
    /// <summary>
    /// | Method |     Mean |     Error |    StdDev |    Gen 0 |    Gen 1 | Allocated |
    /// |------- |---------:|----------:|----------:|---------:|---------:|----------:|
    /// |  Part1 | 1.522 ms | 0.0299 ms | 0.0378 ms | 333.9844 |  66.4063 |      2 MB |
    /// |  Part2 | 2.845 ms | 0.0531 ms | 0.0827 ms | 566.4063 | 238.2813 |      3 MB |
    /// Removed allocations from List creation in GetLowPoints and using bool isLower
    /// |    Method |       Mean |    Error |   StdDev |    Gen 0 |    Gen 1 | Allocated |
    /// |---------- |-----------:|---------:|---------:|---------:|---------:|----------:|
    /// | ParseFile |   379.3 us |  3.49 us |  2.91 us |  12.2070 |   5.8594 |     77 KB |
    /// |     Part1 |   521.3 us |  2.81 us |  2.63 us |  15.6250 |   7.8125 |     96 KB |
    /// |     Part2 | 1,730.8 us | 21.97 us | 20.55 us | 248.0469 | 117.1875 |  1,529 KB |
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

        [Benchmark]
        public List<int[]> ParseFile()
        {
            var map = new List<int[]>();

            foreach (var line in Data)
            {
                var arr = new int[line.Length];

                for (var c = 0; c < line.Length; c++)
                    arr[c] = (int)char.GetNumericValue(line[c]);

                map.Add(arr);
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

        private HashSet<D9Point> GetLowPoints(List<int[]> map)
        {
            var lowPoints = new HashSet<D9Point>();

            for (var i = 0; i < map.Count; i++)
            {
                for (var c = 0; c < map[i].Length; c++)
                {
                    var currentVal = map[i][c];
                    bool isLower = true;

                    if (i != 0)
                        isLower = isLower && map[i - 1][c] > currentVal;
                    if (i != map.Count - 1)
                        isLower = isLower && map[i + 1][c] > currentVal;
                    if (c != 0)
                        isLower = isLower && map[i][c - 1] > currentVal;
                    if (c != map[i].Length - 1)
                        isLower = isLower && map[i][c + 1] > currentVal;

                    if (isLower)
                        lowPoints.Add(new D9Point(i, c, currentVal));
                }
            }

            return lowPoints;
        }

        private HashSet<D9Point> GetBasin(List<int[]> map, HashSet<D9Point> basin, D9Point pt)
        {
            if (pt.Depth == 9)
                return basin;

            if (!basin.Add(pt))
                return basin;

            if (pt.Row != 0)
                basin = GetBasin(map, basin,
                    new D9Point(pt.Row - 1, pt.Col, map[pt.Row - 1][pt.Col]));
            if (pt.Row != map.Count - 1)
                basin = GetBasin(map, basin,
                    new D9Point(pt.Row + 1, pt.Col, map[pt.Row + 1][pt.Col]));
            if (pt.Col != 0)
                basin = GetBasin(map, basin,
                    new D9Point(pt.Row, pt.Col - 1, map[pt.Row][pt.Col - 1]));
            if (pt.Col != map[pt.Row].Length - 1)
                basin = GetBasin(map, basin,
                    new D9Point(pt.Row, pt.Col + 1, map[pt.Row][pt.Col + 1]));

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

        public static bool operator ==(D9Point left, D9Point right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(D9Point left, D9Point right)
        {
            return !(left == right);
        }
    }
}
