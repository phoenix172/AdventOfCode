namespace Task6;

public class Guard
{
    public const char DefaultGuardCharacter = '^';
    public const char WallCharacter = '#';
    public const char EmptySpaceCharacter = '.';

    private static readonly Dictionary<Direction, char> GuardCharacters = new()
    {
        { Direction.Up, '^' },
        { Direction.Right, '>' },
        { Direction.Down, 'v' },
        { Direction.Left, '<' },
    };

    public Board<char> Board { get; }

    public Guard(Board<char> board)
    {
        Board = board;

        Position = Board.Find(DefaultGuardCharacter) ??
                   throw new ArgumentException("Guard is not present on the board");
        Direction = Direction.Up;
    }

    public Position Position { get; set; }
    public Direction Direction { get; set; }
    
    public Position NextPosition => Position.Move(Direction);

    public MoveResult Forward()
    {
        var nextPosition = Position.Move(Direction);
        var nextCharacter = Board.Get(nextPosition);

        if(nextCharacter == WallCharacter)
            throw new InvalidOperationException("Tried to move through a wall");
        
        if (nextCharacter == default)
            return new(false, nextCharacter);

        Position = nextPosition;
        var lookaheadPosition = Position.Move(Direction);
        return new(true, Board.Get(lookaheadPosition));
    }

    public void TurnRight() => Direction = Direction.Next();
    
    
    public HashSet<Position> GetPath()
    {
        var initialPosition = Position;
        var initialDirection = Direction;
        Guard.MoveResult moveResult;
        HashSet<Position> visited = new HashSet<Position>([Position]);
        while ((moveResult = Forward()).Success)
        {
            if (moveResult.NextCharacter == WallCharacter)
                TurnRight();
            visited.Add(Position);
        }

        Position = initialPosition;
        Direction = initialDirection;
        return visited;
    }

    public record struct MoveResult(bool Success, char? NextCharacter);

    public override string ToString() => GuardCharacters[Direction].ToString();
}