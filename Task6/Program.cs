using System.Collections.Immutable;
using System.Diagnostics;

namespace Task6;

class Program
{
    static void Main(string[] args)
    {
        //Part1(board);
        
        var blockingPointsSimple = Run("Input2.txt");
        Debug.Assert(blockingPointsSimple.Contains(new Position(6,3)));
        Debug.Assert(blockingPointsSimple.Contains(new Position(7,6)));
        Debug.Assert(blockingPointsSimple.Contains(new Position(7,7)));
        Debug.Assert(blockingPointsSimple.Contains(new Position(8,1)));
        Debug.Assert(blockingPointsSimple.Contains(new Position(8,3)));
        Debug.Assert(blockingPointsSimple.Contains(new Position(9,7)));

        var blockingPoints = Run("Input.txt");
    }

    private static HashSet<Position> Run(string path)
    {
        return new Part2Solution(new Board<char>([..File.ReadAllLines(path).Select(x => x.ToImmutableArray())])).Part2();
    }

    private static void Part1(Board<char> board)
    {
        Guard guard = new Guard(board);
        Guard.MoveResult moveResult;
        HashSet<Position> visited = new HashSet<Position>([guard.Position]);
        while ((moveResult = guard.Forward()).Success)
        {
            if (moveResult.NextCharacter == Guard.WallCharacter)
                guard.TurnRight();
            visited.Add(guard.Position);
            // Console.WriteLine("--------------------------");
            // Console.WriteLine(moveResult);
            // Console.WriteLine(guard.Position);
            // Console.WriteLine("--------------------------");
        }

        Console.WriteLine(visited.Count);
    }


    
}