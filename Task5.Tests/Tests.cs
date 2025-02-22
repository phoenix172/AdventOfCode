
using System.Collections;
using Microsoft.VisualStudio.TestPlatform.Common.DataCollection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Legacy;

namespace Task5.Tests;

[TestFixture]
public class Tests
{
    [Test]
    public void Test_Part1_Simple()
    {
        var input = Program.ReadRequirementsAndUpdates("Input2.txt");
        var result = Program.GetPart1Result(input);
        
        CollectionAssert.AreEqual(result.ValidUpdates, (int[][])[[75,47,61,53,29],[97,61,53,29,13],[75,29,13]]);
        Assert.That(result.ResultNumber, Is.EqualTo(143));
    }

    public record TopoSortTestCase(IReadOnlyList<(int, int)> Edges, IReadOnlyList<int> Sorted)
    {
        public override string ToString()
        {
            return $"Edges: {string.Join(", ", Edges)}, Sorted: {string.Join(", ", Sorted)}"; 
        }
    };
    
    public static IEnumerable<TopoSortTestCase> TopoSortTestData()
    {
        yield return new TopoSortTestCase
        (
            Edges: [(1,2), (2,3), (3, 4)],
            Sorted: [1,2,3,4]
        );
        yield return new TopoSortTestCase
        (
            Edges: [(1,2), (3, 4), (2,3)],
            Sorted: [1,2,3,4]
        );
        yield return new TopoSortTestCase
        (
            Edges: [(3, 4), (1,2), (2,3)],
            Sorted: [1,2,3,4]
        );
        yield return new TopoSortTestCase
        (
            Edges: [(3, 4), (1, 2), (2, 3)],
            Sorted: [1, 2, 3, 4]
        );

        yield return new TopoSortTestCase
        (
            Edges: [(5, 6), (4, 5), (2, 3), (1, 2)],
            Sorted: [1, 2, 3, 4, 5, 6]
        );

        yield return new TopoSortTestCase
        (
            Edges: [(1, 3), (2, 3), (3, 4)],
            Sorted: [1, 2, 3, 4]
        );

        yield return new TopoSortTestCase
        (
            Edges: [(4, 5), (3, 5), (2, 4), (1, 3)],
            Sorted: [1, 2, 3, 4, 5]
        );

        yield return new TopoSortTestCase
        (
            Edges: [(1, 2), (2, 3), (3, 4), (4, 5)],
            Sorted: [1, 2, 3, 4, 5]
        );

        yield return new TopoSortTestCase
        (
            Edges: [(2, 3), (3, 1), (4, 2), (5, 4)],
            Sorted: [5, 4, 2, 3, 1]
        );

        yield return new TopoSortTestCase
        (
            Edges: [(1, 4), (1, 3), (2, 4), (3, 5)],
            Sorted: [1, 2, 3, 4, 5]
        );

        yield return new TopoSortTestCase
        (
            Edges: [(1, 3), (2, 3)],
            Sorted: [1, 2, 3]
        );

        yield return new TopoSortTestCase
        (
            Edges: [],
            Sorted: []
        );

        yield return new TopoSortTestCase
        (
            Edges: [(1, 3), (2, 3), (3, 5), (4, 5), (5, 6), (6, 7), (4, 6)],
            Sorted: [1, 2, 3, 4, 5, 6, 7]
        );

        yield return new TopoSortTestCase
        (
            Edges: [(1, 4), (2, 4), (3, 5), (4, 6), (5, 6), (6, 7)],
            Sorted: [1, 2, 3, 4, 5, 6, 7]
        );

        yield return new TopoSortTestCase
        (
            Edges: [(1, 5), (2, 5), (3, 6), (4, 6), (5, 7), (6, 7)],
            Sorted: [1, 2, 3, 4, 5, 6, 7]
        );

        yield return new TopoSortTestCase
        (
            Edges: [(1, 3), (2, 3), (3, 4), (4, 6), (5, 6), (6, 7), (3, 5), (2, 5)],
            Sorted: [1, 2, 3, 4, 5, 6, 7]
        );

        yield return new TopoSortTestCase
        (
            Edges: [(1, 4), (2, 4), (2, 5), (3, 6), (4, 7), (5, 7), (6, 8), (7, 8)],
            Sorted: [1, 2, 3, 4, 5, 6, 7, 8]
        );

        yield return new TopoSortTestCase
        (
            Edges: [(1, 2), (2, 3), (3, 4), (4, 5), (5, 6), (6, 7), (7, 8), (8, 9), (9, 10)],
            Sorted: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
        );

        yield return new TopoSortTestCase
        (
            Edges: [(1, 3), (2, 3), (3, 6), (4, 5), (5, 6), (6, 7), (7, 9), (8, 9)],
            Sorted: [1, 2, 3, 4, 5, 6, 7, 8, 9]
        );

    }
    
    [TestCaseSource(nameof(TopoSortTestData))]
    public void Toposort_Dag(TopoSortTestCase testCase)
    {
        var input = Program.ReadRequirementsAndUpdates("Input2.txt");

        var graph = new DirectedGraph<int>(testCase.Edges);
        var result = graph.TopologicalSort().ToList();
        TestContext.Out.WriteLine(string.Join(", ",result));
        Assert.That(result, Is.EqualTo(testCase.Sorted).AsCollection);
    }
    
    [Test]
    public void Test_Part1_Full()
    {
        var input = Program.ReadRequirementsAndUpdates("Input.txt");
        var result = Program.GetPart1Result(input);
        
        Assert.That(result.ResultNumber, Is.EqualTo(4569));
    }

    [Test]
    public void Test_Part2_Simple()
    {
        var input = Program.ReadRequirementsAndUpdates("Input.txt");
        var result = Program.GetPart2Result(input);
        
        CollectionAssert.AreEqual(result.CorrectedUpdates, (int[][])[[97,75,47,61,53],[61,29,13],[97,75,47,29,13]]);
        Assert.That(result.ResultNumber, Is.EqualTo(123));
    }
}