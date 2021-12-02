namespace AdventOfCode.Days
{
    /// <summary>
    /// w/ 20,000 lines

    /// string.split
    ///|                            Method |        Mean |     Error |    StdDev |    Gen 0 |    Gen 1 |    Gen 2 |   Allocated |
    ///|---------------------------------- | -----------:|----------:|----------:|---------:| --------:| --------:|------------:|
    ///|                           SetData |  7,508.7 us | 182.67 us | 529.96 us | 445.3125 |   7.8125 |        - | 2,795,870 B |
    ///|        CalculateChangeInDirection |    903.7 us |  18.05 us |  37.68 us |  82.0313 |  41.0156 |  41.0156 |   460,294 B |
    ///| CalculateChangeInDirectionWithAim |    271.0 us |   1.21 us |   1.13 us |        - |        - |        - |           - |
                                                                                                                                                                                                                 
    /// span<T>                                                                              
    ///|                            Method |        Mean |     Error |    StdDev |    Gen 0 |    Gen 1 |    Gen 2 |   Allocated |
    ///|---------------------------------- | -----------:|----------:|----------:|---------:| --------:| --------:|  ----------:|
    ///|                           SetData |  4,335.6 us |  69.57 us |  87.98 us | 125.0000 |  15.6250 |        - |   812,127 B |
    ///|        CalculateChangeInDirection |    831.3 us |   6.27 us |   5.87 us |  82.0313 |  41.0156 |  41.0156 |   460,294 B |
    ///| CalculateChangeInDirectionWithAim |    260.7 us |   0.34 us |   0.30 us |        - |        - |        - |           - |
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
            foreach (var input in test)
                Inputs.Add(new MovementInput(input));
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
            public int Left { get; set; }
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
