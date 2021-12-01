var day1 = new Day1();

Console.WriteLine(day1.GetIncreasesSimple());
Console.WriteLine(day1.GetWindowedIncreasesSimple());
Console.WriteLine(day1.GetIncreases());
Console.WriteLine(day1.GetWindowedIncreases());

// benchmark the methods
var summary = BenchmarkRunner.Run<Day1>();
