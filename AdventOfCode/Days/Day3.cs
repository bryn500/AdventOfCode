namespace AdventOfCode.Days
{
    /// <summary>
    /// FilterListByBitCount w/o ToList
    ///| Method |        Mean |     Error |    StdDev |    Gen 0 |    Gen 1 | Allocated |
    ///|------- |------------:|----------:|----------:|---------:|---------:|----------:|
    ///|  Part1 |    468.5 us |   8.31 us |   7.37 us |   9.2773 |   4.3945 |     58 KB |
    ///|  Part2 | 17,979.2 us | 212.64 us | 166.02 us | 375.0000 | 187.5000 |  2,438 KB |
    /// FilterListByBitCount w/ .ToList
    ///| Method |       Mean |    Error |   StdDev |   Gen 0 |   Gen 1 | Allocated |
    ///|------- |-----------:|---------:|---------:|--------:|--------:|----------:|
    ///|  Part1 |   503.5 us | 10.05 us | 21.85 us |  8.7891 |  3.9063 |     58 KB |
    ///|  Part2 | 2,476.5 us | 21.79 us | 20.38 us | 62.5000 | 31.2500 |    393 KB |
    /// </summary>
    [MemoryDiagnoser]
    public class Day3
    {
        private const string Filename = "Data/Day3/input.txt";

        public int BitLength { get; set; }
        public IEnumerable<string> Data;

        public Day3(int bitLength = 12, string filename = Filename)
        {
            BitLength = bitLength;
            Data = File.ReadLines(filename);
        }

        [Benchmark]
        public int Part1()
        {
            var counts = new int[BitLength];

            //for (int i = 0; i <= BitLength - 1; i++)
            //    counts[i] = GetCountOfBitAtPosition(test, i);
            foreach (string input in Data)
            {
                for (int i = 0; i < input.Length; i++)
                {
                    if (input[i] == '0')
                        counts[i]--;
                    else
                        counts[i]++;
                }
            }

            char[] gammaBits = new char[BitLength];
            char[] epsilonBits = new char[BitLength];
            for (var i = 0; i <= BitLength - 1; i++)
            {
                if (counts[i] > 0)
                {
                    gammaBits[i] = '1';
                    epsilonBits[i] = '0';
                }
                else
                {
                    gammaBits[i] = '0';
                    epsilonBits[i] = '1';
                }
            }

            var gamma = Convert.ToInt32(new string(gammaBits), 2);
            var epsilon = Convert.ToInt32(new string(epsilonBits), 2);

            return gamma * epsilon;
        }

        [Benchmark]
        public int Part2()
        {
            IEnumerable<string> oxygenResult = Data;
            for (var i = 0; i <= BitLength - 1; i++)
            {   
                if (oxygenResult.Count() <= 1)
                    continue;

                oxygenResult = FilterListByBitCount(oxygenResult, i);
            }

            IEnumerable<string> co2Result = Data;
            for (var i = 0; i <= BitLength - 1; i++)
            {
                if (co2Result.Count() <= 1)
                    continue;

                co2Result = FilterListByBitCount(co2Result, i, true);
            }

            var o2 = Convert.ToInt32(oxygenResult.First(), 2);
            var co2 = Convert.ToInt32(co2Result.First(), 2);

            return o2 * co2;
        }

        private IEnumerable<string> FilterListByBitCount(IEnumerable<string> list, int position, bool invert = false)
        {
            int count = GetCountOfBitAtPosition(list, position);

            bool bit = count >= 0;
            if (invert)
                bit = !bit;

            list = list
                .Where(x => CharAtPostionMatchesBool(x, position, bit))
                .ToList(); // test performance of ToList

            return list;
        }

        private int GetCountOfBitAtPosition(IEnumerable<string> list, int position)
        {
            int result = 0;
            foreach (string input in list)
            {
                // increment count in postion
                if (input[position] == '0')
                    result--;
                else
                    result++;
            }

            return result;
        }

        private bool CharAtPostionMatchesBool(string x, int position, bool bit)
        {
            return Convert.ToBoolean(char.GetNumericValue(x[position])) == bit;
        }
    }
}
