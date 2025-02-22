using System.Collections.Immutable;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Task5.Tests")]

namespace Task5;

record Input(ILookup<int, int> RequirementsForNumbers, IEnumerable<IEnumerable<int>> Updates); 

internal class Program
{
    static void Main(string[] args)
    {
        var input = ReadRequirementsAndUpdates("Input.txt");

        var part1Result = GetPart1Result(input);
        part1Result.ValidUpdates.ToList()
            .ForEach(x => Console.WriteLine(string.Join(", ", x)));
        
        var part2Result = GetPart2Result(input);
        part2Result.CorrectedUpdates.ToList()
            .ForEach(x => Console.WriteLine(string.Join(", ", x)));
    }

    public static (int ResultNumber, IReadOnlyList<IReadOnlyList<int>> ValidUpdates) GetPart1Result(Input input)
    {
        var validUpdates = input.Updates
            .Where(x => CheckPart1(x.ToList(), input.RequirementsForNumbers))
            .Select(x => x.ToList()).ToList();
        var resultNumber = validUpdates.Sum(update => update.Skip(update.Count / 2).First());
        return (resultNumber, validUpdates);
    }
    
    public static (int ResultNumber, IReadOnlyList<IReadOnlyList<int>> CorrectedUpdates) GetPart2Result(Input input)
    {
        var edges = input.RequirementsForNumbers.SelectMany(x=>x.Select(y=>(y,x.Key))).ToList();
        var correctedUpdates = input.Updates.Select(x => (Initial: x, Corrected: CorrectPart2(x.ToList(), edges)))
            .Where(x => !x.Initial.SequenceEqual(x.Corrected)).Select(x=>x.Corrected).ToList();
        var resultNumber = correctedUpdates.Sum(update => update.Skip(update.Count() / 2).First());
        
        correctedUpdates.ForEach(x => Console.WriteLine(string.Join(", ", x)));
        
        return (resultNumber, correctedUpdates);
    }
    
    public static Input ReadRequirementsAndUpdates(string fileName)
    {
        string input = File.ReadAllText(fileName);
        var inputParts = input.Split("\r\n\r\n");
        var requirementsForNumbers = inputParts[0].Split("\n", StringSplitOptions.RemoveEmptyEntries)
            .Select(requirement => requirement.Split('|', StringSplitOptions.TrimEntries))
            .ToLookup(x => int.Parse(x.Last()), x => int.Parse(x.First()));
        var updates = inputParts[1].Split("\n", StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Split(',', StringSplitOptions.TrimEntries).Select(int.Parse));
        return new Input(requirementsForNumbers, updates);
    }

    private static bool CheckPart1(IReadOnlyList<int> update, ILookup<int, int> requirements)
    {
        ImmutableHashSet<int> updateNumbers = update.ToImmutableHashSet();
        HashSet<int> pastNumbers = [];
        foreach (var number in update)
        {
            if (requirements[number].All(MeetsRequirement))
                pastNumbers.Add(number);
            else
                return false;
        }

        return true;

        bool MeetsRequirement(int requirement)
        {
            return (updateNumbers.Contains(requirement) && pastNumbers.Contains(requirement)) ||
                   !updateNumbers.Contains(requirement);
        }
    }

    private static IReadOnlyList<int> CorrectPart2(IReadOnlyList<int> update, IList<(int, int)> requirements)
    {
        var numbersInUpdate = update.ToImmutableHashSet();
        var inducedGraphEdges = requirements.Where(x => numbersInUpdate.Contains(x.Item1) && numbersInUpdate.Contains(x.Item2));
        var inducedGraph = new DirectedGraph<int>(inducedGraphEdges);
        var result = inducedGraph.TopologicalSort().ToList();
        return result;
    }
}