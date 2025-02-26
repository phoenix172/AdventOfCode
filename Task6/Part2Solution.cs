namespace Task6;

public class Part2Solution
{
    private readonly Board<char> _board;

    public Part2Solution(Board<char> board)
    {
        this._board = board;
    }

    public HashSet<Position> Part2()
    {
        Guard initialGuard = new Guard(_board);
        var blockingPoints =  FindLoopPositions(_board, initialGuard);
        
        Console.WriteLine(string.Join("\n", blockingPoints));
        Console.WriteLine(blockingPoints.Count);

        return blockingPoints;
    }
    
    public HashSet<Position> FindLoopPositions(Board<char> board, Guard guard)
    {
        var path = guard.GetPath();
        var emptyPositions = path.Except([guard.Position]);
        
        var loopPositions = new HashSet<Position>(path.Count*2);
        foreach (var pos in emptyPositions)
        {
            board.Set(pos, '#'); 
            if (GuardCreatesLoop(board))
            {
                loopPositions.Add(pos);
            }
            board.Set(pos, ' '); 
        }

        return loopPositions;
    }

    private bool GuardCreatesLoop(Board<char> board)
    {
        var guard = new Guard(board);
        var visited = new HashSet<(Position pos, Direction dir)>();

        while (true)
        {
            var state = (guard.Position, guard.Direction);
            if (!visited.Add(state))
            {
                return true; 
            }

            
            while (board.Get(guard.NextPosition) == Guard.WallCharacter)
            {
                guard.TurnRight();
            }
            var moveResult = guard.Forward();
            
            if (!moveResult.Success)
            {
                return false; 
            }
        }
    }
}