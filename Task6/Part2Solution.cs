namespace Task6;

public class Part2Solution
{
    private readonly HashSet<Position> _blockingPoints = new HashSet<Position>();
    private readonly Board<char> _board;

    public Part2Solution(Board<char> board)
    {
        this._board = board;
    }

    public HashSet<Position> Part2()
    {
        Guard initialGuard = new Guard(_board);
        ExploreJob initialJob = new([], initialGuard);
        Queue<ExploreJob> pointsToExplore = new([initialJob]);

        while (pointsToExplore.Count > 0)
        {
            var job = pointsToExplore.Dequeue();
            Explore(job);
        }
        
        Console.WriteLine(string.Join("\n", _blockingPoints));
        Console.WriteLine(_blockingPoints.Count);

        return _blockingPoints;
    }
    
    public HashSet<Position> Part22()
    {
        ExploreJob initialJob = new([], new Guard(_board));
        Explore(initialJob);

        // ExploreJob secondJob = new(initialJob.TurningPoints, new Guard(_board));
        // Explore(secondJob);
        //
        // ExploreJob thirdJob = new(secondJob.TurningPoints, new Guard(_board));
        // Explore(thirdJob);
        //
        // ExploreJob thirdJob1 = new(thirdJob.TurningPoints, new Guard(_board));
        // Explore(thirdJob1);

        Console.WriteLine(string.Join("\n", _blockingPoints));
        Console.WriteLine(_blockingPoints.Count);

        return _blockingPoints;
    }
    
    void Explore(ExploreJob job)
    {
        Guard.MoveResult moveResult = new(false, _board.Get(job.Guard.NextPosition), job.Guard.NextPosition);
        do
        {
            TryInsertBlock(job);

            if (moveResult.NextCharacter == Guard.WallCharacter)
            {
                job.TurningPoints.Add((Position: job.Guard.Position, DirectionBeforeTurn: job.Guard.Direction));
                job.Guard.TurnRight();
            }

            TryInsertBlock(job);
        } while ((moveResult = job.Guard.Forward()).Success);
    }
    
    void TryInsertBlock(ExploreJob job)
    {
        var facing = _board.Get(job.Guard.NextPosition);
        if (facing == Guard.DefaultGuardCharacter)
            return;
            
        var points = job.TurningPoints.Where(Predicate).ToList();

        if (points.Any() && facing == Guard.EmptySpaceCharacter)
            _blockingPoints.Add(job.Guard.NextPosition);
            
        bool Predicate((Position Position, Direction DirectionBeforeTurn) turningPoint)
            => job.Guard.Position.IsSameRowOrColumnsAs(turningPoint.Position) &&
               job.Guard.Direction.Next() == turningPoint.DirectionBeforeTurn;
    }
    
    private class ExploreJob(
        List<(Position Position, Direction DirectionBeforeTurn)> turningPoints,
        Guard guard)
    {
        public List<(Position Position, Direction DirectionBeforeTurn)> TurningPoints { get; } = turningPoints;
        public Guard Guard { get; } = guard;
    }
}