using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Task3;

class Program
{
    static void Main(string[] args)
    {
        string input = File.ReadAllText("Input.txt");
        Console.WriteLine(Part2Sum(input));
    }

    private static long Part2Sum(string input)
    {
        Regex commandsRegex = new Regex(@"(mul\((?<left>[0-9]+),(?<right>[0-9]+)\))|(?<dont>don't\(\))|(?<do>do\(\))");
        long sum = 0;
        bool enabled = true;
        foreach (Match match in commandsRegex.Matches(input))
        {
            string? Get(string group) =>
                match.Groups.TryGetValue(group, out var value) && value is { Success: true }
                    ? value.Value
                    : null;

            if (enabled && Get("left") is { } left && Get("right") is { } right)
                sum += int.Parse(left) * int.Parse(right);
            if (Get("dont") is not null)
                enabled = false;
            if (Get("do") is not null)
                enabled = true;
        }

        return sum;
    }
}