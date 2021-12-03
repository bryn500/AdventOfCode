var day1 = new Day1();
Console.WriteLine(day1.GetIncreasesSimple()); //1266
Console.WriteLine(day1.GetWindowedIncreasesSimple()); //1217
Console.WriteLine(day1.GetIncreases()); //1266
Console.WriteLine(day1.GetWindowedIncreases()); //1217

var day2 = new Day2();
Console.WriteLine(day2.Part1().ProductOfPart1Movement); // 2150351
Console.WriteLine(day2.Part2().ProductOfPart2Movement); // 1842742223

var day3 = new Day3();
Console.WriteLine(day3.Part1()); // 3429254
Console.WriteLine(day3.Part2()); // 5410338

var summary = BenchmarkRunner.Run<Day3>();