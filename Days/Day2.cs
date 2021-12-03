namespace AdventOfCode.Days
{
    /// <summary>
    /// w/ 10,000 lines

    /// string.split
    ///|  Method |       Mean |    Error |   StdDev |    Gen 0 |   Gen 1 |   Allocated |
    ///|-------- |-----------:|---------:|---------:|---------:|--------:|------------:|
    ///| SetData | 3,160.1 us | 53.34 us | 61.42 us | 222.6563 | 15.6250 | 1,402,553 B |
    ///|   Part1 |   405.7 us |  5.77 us |  5.12 us |  36.1328 |  8.7891 |   230,832 B |
    ///|   Part2 |   132.7 us |  2.05 us |  1.81 us |        - |       - |           - |

    /// span<T>                                                                              
    ///|  Method |       Mean |    Error |   StdDev |    Gen 0 |   Gen 1 |   Allocated |
    ///|-------- |-----------:|---------:|---------:| --------:|--------:|  ----------:|
    ///| SetData | 2,267.1 us | 18.51 us | 16.41 us |  62.5000 | 11.7188 |   410,698 B |
    ///|   Part1 |   391.7 us |  0.68 us |  0.64 us |  36.6211 |  9.2773 |   230,832 B |
    ///|   Part2 |   127.8 us |  0.24 us |  0.23 us |        - |       - |           - |
    
    /// Reduced allocations in Part1 by not grouping by direction
    ///|  Method |       Mean |    Error |   StdDev |   Gen 0 |   Gen 1 | Allocated |
    ///|-------- |-----------:|---------:|---------:|--------:|--------:|----------:|
    ///| SetData | 2,201.4 us | 27.53 us | 21.49 us | 62.5000 | 11.7188 | 406,953 B |
    ///|   Part1 |   128.8 us |  0.24 us |  0.18 us |       - |       - |         - |
    ///|   Part2 |   130.0 us |  0.23 us |  0.20 us |       - |       - |         - |
    /// </summary>
    [MemoryDiagnoser]
    public class Day2
    {
        public List<MovementInput> Inputs { get; set; }
        public Day2()
        {
            Inputs = new List<MovementInput>();
        }

        [GlobalSetup]
        [Benchmark]
        public void SetData()
        {
            var test = File.ReadLines("Data/Day2/input.txt");
            foreach (var input in test)
                Inputs.Add(new MovementInput(input));
        }

        [Benchmark]
        public int Part1()
        {
            var amounts = new MovementAmounts();

            foreach (var input in Inputs)
            {
                switch (input.Direction)
                {
                    case Direction.Up:
                        amounts.Up += input.Amount;
                        break;
                    case Direction.Down:
                        amounts.Down += input.Amount;
                        break;
                    case Direction.Forward:
                        amounts.Forward += input.Amount;
                        break;
                    case Direction.Backward:
                        amounts.Backward += input.Amount;
                        break;
                }
            }

            return amounts.ProductOfPart1Movement;
        }

        [Benchmark]
        public int Part2()
        {
            var amounts = new MovementAmounts();

            foreach (var input in Inputs)
            {
                switch (input.Direction)
                {
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

            return amounts.ProductOfPart2Movement;
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
            public int Up { get; set; }
            public int Down { get; set; }
            public int Forward { get; set; }
            public int Backward { get; set; }
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
            Forward,
            Backward
        }
    }
}
