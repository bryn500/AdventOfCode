namespace AdventOfCode.Days
{
    /// <summary>
    ///| Method | days |     Mean |   Error |   StdDev |  Gen 0 |  Gen 1 | Allocated |
    ///|------- |----- |---------:|--------:|---------:|-------:|-------:|----------:|
    ///|  Part1 |   18 | 353.1 us | 6.78 us | 12.90 us | 9.2773 | 4.3945 |     57 KB |
    ///|  Part2 |   18 | 282.3 us | 2.64 us |  2.46 us | 3.4180 | 1.4648 |     21 KB |
    /// </summary>
    [MemoryDiagnoser]
    public class Day6
    {
        private const string Filename = "Data/Day6/input.txt";

        public IEnumerable<string> Data;
        const int _defaultReproduceAge = 6;
        const int _newReproduceAge = 8;

        public Day6(string filename = Filename)
        {
            Data = File.ReadLines(filename);
        }

        public List<int> ParseFile()
        {
            return Data.First().Split(',').Select(x => int.Parse(x)).ToList();
        }

        [Benchmark]
        [Arguments(18)]
        public int Part1(int days = 18)
        {
            var fish = ParseFile();

            foreach (var _ in Enumerable.Range(1, days))
            {
                var fishToAdd = new List<int>();

                for (int i = 0; i < fish.Count; i++)
                {
                    if (fish[i] == 0)
                        fishToAdd.Add(_newReproduceAge);

                    fish[i]--;

                    if (fish[i] < 0)
                        fish[i] = _defaultReproduceAge;
                }

                fish.AddRange(fishToAdd);
            }

            return fish.Count;
        }

        [Benchmark]
        [Arguments(18)]
        public long Part2(int days = 18)
        {
            var data = ParseFile();

            // populate initial data
            var fishMap = new long[9];
            foreach (int fish in data)
                fishMap[fish]++;

            foreach (var _ in Enumerable.Range(1, days))
            {
                // new fish count
                long fishToAdd = fishMap[0];

                // decrement fish
                for (int i = 1; i < fishMap.Length; i++)
                    fishMap[i - 1] = fishMap[i];

                // add new fish
                fishMap[_newReproduceAge] = fishToAdd;
                fishMap[_defaultReproduceAge] += fishToAdd;
            }

            return fishMap.Sum();
        }
    }
}
