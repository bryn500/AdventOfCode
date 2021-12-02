namespace AdventOfCode.Days
{
    /// <summary>
    /// Initial
    ///|                            Method |      Mean |     Error |    StdDev |   Gen 0 |  Gen 1 | Allocated |
    ///|---------------------------------- |----------:| ---------:| ---------:|--------:|-------:|----------:|
    ///|                           SetData | 613.62 us |  8.825 us |  7.823 us | 28.3203 | 0.9766 | 180,045 B |
    ///|        CalculateChangeInDirection |  40.93 us |  0.249 us |  0.233 us |  3.4180 | 0.1221 |  21,696 B |
    ///| CalculateChangeInDirectionWithAim |  12.89 us |  0.046 us |  0.038 us |       - |      - |         - |
    /// Span<T> slice from string.split
    ///|                            Method |      Mean |     Error |    StdDev |   Gen 0 |  Gen 1 | Allocated |
    ///|---------------------------------- |----------:|----------:|----------:|--------:|-------:|----------:|
    ///|                           SetData | 519.94 us | 10.352 us | 23.366 us | 12.6953 | 5.8594 |  80,858 B |
    ///|        CalculateChangeInDirection |  42.15 us |  0.407 us |  0.340 us |  3.4180 | 0.1221 |  21,696 B |
    ///| CalculateChangeInDirectionWithAim |  13.34 us |  0.171 us |  0.160 us |       - |      - |         - |
    /// </summary>
    [MemoryDiagnoser]
    public class Day2
    {
        public List<MovementInput> Inputs { get; set; }
        public Day2()
        {
            Inputs = new List<MovementInput>();
            SetData();
        }

        [Benchmark]
        public void SetData()
        {
            var test = File.ReadLines("Data/Day2/input.txt");
            Inputs = test.Select(x => new MovementInput(x)).ToList();
        }

        [Benchmark]
        public MovementAmounts CalculateChangeInDirection()
        {
            var inputDirections = Inputs.GroupBy(x => x.Direction);

            var amounts = new MovementAmounts();

            foreach (var direction in inputDirections)
            {
                switch (direction.Key)
                {
                    case Direction.Left:
                        amounts.Left = direction.Sum(x => x.Amount);
                        break;
                    case Direction.Right:
                        amounts.Right = direction.Sum(x => x.Amount);
                        break;
                    case Direction.Up:
                        amounts.Up = direction.Sum(x => x.Amount);
                        break;
                    case Direction.Down:
                        amounts.Down = direction.Sum(x => x.Amount);
                        break;
                    case Direction.Forward:
                        amounts.Forward = direction.Sum(x => x.Amount);
                        break;
                    case Direction.Backward:
                        amounts.Backward = direction.Sum(x => x.Amount);
                        break;
                }
            }

            return amounts;
        }

        [Benchmark]
        public MovementAmounts CalculateChangeInDirectionWithAim()
        {
            var amounts = new MovementAmounts();

            foreach (var input in Inputs)
            {
                switch (input.Direction)
                {
                    case Direction.Left:
                        amounts.Horizontal += input.Amount;
                        break;
                    case Direction.Right:
                        amounts.Horizontal -= input.Amount;
                        break;
                    case Direction.Up:
                        amounts.Aim -= input.Amount;
                        break;
                    case Direction.Down:
                        amounts.Aim += input.Amount;
                        break;
                    case Direction.Forward:
                        amounts.Traversal += input.Amount;
                        amounts.Depth += input.Amount * amounts.Aim;
                        break;
                    case Direction.Backward:
                        amounts.Traversal -= input.Amount;
                        amounts.Depth -= input.Amount * amounts.Aim;
                        break;
                }
            }

            return amounts;
        }

        public struct MovementAmounts
        {
            // Part 2
            public int Aim { get; set; }
            public int Depth { get; set; }
            public int Horizontal { get; set; }
            public int Traversal { get; set; }
            public int ProductOfPart2Movement => Depth * Traversal;

            // Part 1
            public int Left {get; set;}
            public int Right { get; set; }
            public int Up { get; set; }
            public int Down { get; set; }
            public int Forward { get; set; }
            public int Backward { get; set; }
            public int HorizontalTotal => Left - Right;
            public int DepthTotal => Down - Up;
            public int ForwardTotal => Forward - Backward;
            public int ProductOfPart1Movement => DepthTotal * ForwardTotal;
        }

        public struct MovementInput
        {
            public Direction Direction { get; set; }
            public int Amount { get; set; }

            public MovementInput(ReadOnlySpan<char> input)
            {
                //var split = input.Split(' ');
                // span<t>slice instead of string.split

                var spaceindex = input.IndexOf(' ');
                var split = input.Slice(0, spaceindex);
                var split2 = input.Slice(spaceindex + 1, input.Length - split.Length - 1);

                if (!Enum.TryParse(split, true, out Direction result))
                    throw new ArgumentException("Could not parse direction", nameof(input));

                Direction = result;

                if (!int.TryParse(split2, out int amount))
                    throw new ArgumentException("Could not parse amount", nameof(input));

                Amount = amount;
            }
        }

        public enum Direction
        {
            Up,
            Down,
            Left,
            Right,
            Forward,
            Backward
        }
    }
}
