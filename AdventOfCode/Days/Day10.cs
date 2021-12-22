namespace AdventOfCode.Days
{
    /// <summary>
    /// | Method |     Mean |    Error |   StdDev |   Gen 0 |  Gen 1 | Allocated |
    /// |------- |---------:|---------:|---------:|--------:|-------:|----------:|
    /// |  Part1 | 642.9 us | 12.46 us | 14.83 us | 25.3906 | 7.8125 |    158 KB |
    /// |  Part2 | 691.5 us | 13.29 us | 17.28 us | 25.3906 | 7.8125 |    161 KB |
    /// </summary>
    [MemoryDiagnoser]
    public class Day10
    {
        private const string Filename = "Data/Day10/input.txt";

        public IEnumerable<string> Data;

        private readonly Dictionary<char, char> _brackets = new()
        {
            { '(', ')' },
            { '[', ']' },
            { '{', '}' },
            { '<', '>' }
        };

        private readonly Dictionary<char, int> _illegal_points = new()
        {
            { ')', 3 },
            { ']', 57 },
            { '}', 1197 },
            { '>', 25137 }
        };

        private readonly Dictionary<char, int> _completionPoints = new()
        {
            { ')', 1 },
            { ']', 2 },
            { '}', 3 },
            { '>', 4 }
        };

        public Day10(string filename = Filename)
        {
            Data = File.ReadLines(filename);
        }

        [Benchmark]
        public int Part1()
        {
            var illegal_chars = new List<char>();

            foreach (var line in Data)
            {
                var stack = new Stack<char>();

                foreach (var c in line)
                {
                    if (IsValidCloser(stack, c))
                    {
                        stack.Pop();
                    }
                    else if (_brackets.ContainsKey(c))
                    {
                        stack.Push(c);
                    }
                    else
                    {
                        illegal_chars.Add(c);
                        break;
                    }
                }
            }

            return illegal_chars
                .Sum(x => _illegal_points[x]);
        }

        [Benchmark]
        public long Part2()
        {
            var leftovers = new List<Stack<char>>();

            foreach (var line in Data)
            {
                var lft = GetLeftovers(line);

                if (lft != null)
                    leftovers.Add(lft);
            }

            var counts = leftovers.Select(x => GetCompletionScore(x));

            return GetMiddleScore(counts.ToList());
        }

        private Stack<char>? GetLeftovers(string line)
        {
            var stack = new Stack<char>();
            bool corrupt = false;

            foreach (var c in line)
            {
                if (IsValidCloser(stack, c))
                    stack.Pop();
                else if (_brackets.ContainsKey(c))
                    stack.Push(c);
                else
                {
                    corrupt = true;
                    break;
                }
            }

            if (corrupt)
                return null;
            else
                return stack;
        }

        private bool IsValidCloser(Stack<char> stack, char c)
        {
            var closesRound = c == ')' && stack.First() == '(';
            var closesSquare = c == ']' && stack.First() == '[';
            var closesCurly = c == '}' && stack.First() == '{';
            var closesAngle = c == '>' && stack.First() == '<';

            return closesAngle || closesSquare || closesCurly || closesRound;
        }

        public long GetCompletionScore(Stack<char> stack)
        {
            long count = 0; // pain

            foreach (var c in stack)
                count = count * 5 + _completionPoints[_brackets[c]];

            return count;
        }

        private long GetMiddleScore(List<long> counts)
        {
            return counts.OrderBy(x => x)
                .Skip((counts.Count / 2))
                .First();
        }
    }
}
