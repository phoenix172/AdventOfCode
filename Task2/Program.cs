using System.Collections.Immutable;
using System.Numerics;
using System.Reflection.Metadata;

namespace Task2;

static class Extensions
{
    public static ImmutableArray<T> SkipIndex2<T>(this ImmutableArray<T> source, int index)
        => [..source[..index], ..source[(index+1)..]];

    public static IEnumerable<T> SkipIndex<T>(this IReadOnlyList<T> source, int index)
        => source.Take(index).Concat(source.Skip(index + 1));
}

class Program
{
    static async Task Main(string[] args)
    {
        var input = await ReadInput();

        Console.WriteLine(input.Count(x => IsSafe(x)));
        Console.WriteLine(input.Count(IsSafe2));
    }

    private static bool IsSafe2(ImmutableArray<int> list) 
        => list.Select((_, i) => list.SkipIndex(i)).Any(IsSafe);

    private static bool IsSafe(IEnumerable<int> source)
    {
        var list = source.ToImmutableArray();
        var differenceMap = list.Zip(list.Skip(1))
            .Select(numbersPair => (
                Sign: numbersPair.First - numbersPair.Second > 0,
                Difference: Math.Abs(numbersPair.First - numbersPair.Second)
            )).ToImmutableArray();
        return differenceMap.All(item => item.Sign == differenceMap[0].Sign && item.Difference is >= 1 and <= 3);
    }

    private static async Task<ImmutableArray<ImmutableArray<int>>> ReadInput() =>
    [
        ..(await File.ReadAllLinesAsync("Input.txt"))
        .Where(line => !string.IsNullOrWhiteSpace(line))
        .Select(line => line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToImmutableArray())
    ];
}