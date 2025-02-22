namespace Task1;

class Program
{
    static async Task Main(string[] args)
    {
        (int, int)[] inputLines =
            await ReadInput();

        Console.WriteLine(GetPart1Answer(inputLines));
        Console.WriteLine(GetPart2Answer(inputLines));
    }

    private static int GetPart1Answer((int, int)[] inputLines)
    {
        var leftOrdered = inputLines.OrderBy(x => x.Item1).Select(x => x.Item1);
        var rightOrdered = inputLines.OrderBy(x => x.Item2).Select(x => x.Item2);
        var zipped = leftOrdered.Zip(rightOrdered).Sum(x => Math.Abs(x.First - x.Second));
        return zipped;
    }

    private static long GetPart2Answer((int, int)[] inputLines)
    {
        var rightOccurrences = inputLines.GroupBy(x => x.Item2).ToDictionary(x => x.Key, x => x.Count());

        long leftScore = inputLines
            .GroupBy(x => x.Item1)
            .ToDictionary(x => x.Key, x => x.Count())
            .Sum(left => left.Key * left.Value * rightOccurrences.GetValueOrDefault(left.Key, 0));
        return leftScore;
    }

    private static async Task<(int, int)[]> ReadInput()
    {
        return (await File.ReadAllLinesAsync("Input.txt"))
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(line => line.Split(" ", StringSplitOptions.RemoveEmptyEntries))
            .Select(numbersInLine => (int.Parse(numbersInLine[0]), int.Parse(numbersInLine[1])))
            .ToArray();
    }
}