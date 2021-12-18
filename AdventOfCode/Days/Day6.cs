namespace AdventOfCode.Days
{
    /// <summary>
    /// </summary>
    [MemoryDiagnoser]
    public class Day6
    {
        private const string Filename = "Data/Day6/input.txt";

        public IEnumerable<string> Data;

        public Day6(string filename = Filename)
        {
            Data = File.ReadLines(filename);
        }

        [Benchmark]
        [Arguments(18)]
        public int Part1(int days = 80)
        {
            int defaultReproduceAge = 6;
            int newReproduceAge = 8;
            var fish = Data.First().Split(',').Select(x => int.Parse(x)).ToList();

            Console.WriteLine($"Initial state: {string.Join(",", fish)}");

            foreach (var day in Enumerable.Range(1, days))
            {
                var fishToAdd = new List<int>();
                
                for(int i = 0; i < fish.Count; i++)
                {
                    if (fish[i] == 0)                    
                        fishToAdd.Add(newReproduceAge);                    

                    fish[i]--;

                    if (fish[i] < 0)                    
                        fish[i] = defaultReproduceAge;                    
                }

                fish.AddRange(fishToAdd);

                Console.WriteLine($"After {day} days:  {string.Join(",", fish)}");
            }

            return fish.Count;
        }

        //[Benchmark]
        //public int Part2()
        //{
        //}
    }
}
