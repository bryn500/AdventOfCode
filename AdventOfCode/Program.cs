var day1 = new Day1();
Console.WriteLine(day1.Part1Simple()); //1266
Console.WriteLine(day1.Part2Simple()); //1217
Console.WriteLine(day1.Part1()); //1266
Console.WriteLine(day1.Part2()); //1217

var day2 = new Day2();
Console.WriteLine(day2.Part1()); // 2150351
Console.WriteLine(day2.Part2()); // 1842742223

var day3 = new Day3();
Console.WriteLine(day3.Part1()); // 3429254
Console.WriteLine(day3.Part2()); // 5410338

var summary = BenchmarkRunner.Run<Day3>();
