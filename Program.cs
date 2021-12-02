var day1 = new Day1();
Console.WriteLine(day1.GetIncreasesSimple()); //1266
Console.WriteLine(day1.GetWindowedIncreasesSimple()); //1217
Console.WriteLine(day1.GetIncreases()); //1266
Console.WriteLine(day1.GetWindowedIncreases()); //1217
//var summary = BenchmarkRunner.Run<Day1>();

var day2 = new Day2();
Console.WriteLine(day2.Part1().ProductOfPart1Movement); // 2150351
Console.WriteLine(day2.Part2().ProductOfPart2Movement); // 1842742223
var summary2 = BenchmarkRunner.Run<Day2>();
