using System.Collections.Immutable;

namespace Task6;

public record Position(int Row, int Column)
{
    public Position Move(Direction direction) => new(Row + direction.YOffset, Column + direction.XOffset);
    
    public bool IsSameRowOrColumnsAs(Position other)
    {
        bool rowEqual = Row == other.Row;
        bool columnEqual = Column == other.Column;
        if (rowEqual && columnEqual) return false;
        return rowEqual || columnEqual;
    }
};

public record Direction(int YOffset, int XOffset, string Label)
{
    public static readonly Direction Up = new(-1, 0, nameof(Up));
    public static readonly Direction Right = new(0, 1, nameof(Right));
    public static readonly Direction Down = new(1, 0, nameof(Down));
    public static readonly Direction Left = new(0, -1, nameof(Left));

    public static ImmutableArray<Direction> All { get; } = [Up, Right, Down, Left];

    public Direction Next()
    {
        var index = All.IndexOf(this);
        index = (index + 1) % All.Length;
        return All[index];
    }
};

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
            return new(false, nextCharacter, nextPosition);

        Position = nextPosition;
        var lookaheadPosition = Position.Move(Direction);
        return new(true, Board.Get(lookaheadPosition), lookaheadPosition);
    }

    public void TurnRight() => Direction = Direction.Next();

    public record MoveResult(bool Success, char? NextCharacter, Position NextPosition);

    public override string ToString() => GuardCharacters[Direction].ToString();
}

public class Board<T>
{
    public ImmutableArray<ImmutableArray<T>> Grid { get; }

    public Board(ImmutableArray<ImmutableArray<T>> grid)
    {
        Grid = grid;
    }

    public Position? Find(T item)
    {
        var found = Grid.SelectMany((row, rowIndex) =>
            row.Select((cellValue, cellIndex) => (Equal: cellValue?.Equals(item) ?? false, Index: cellIndex))
                .Where(x => x.Equal).Select(x=>new Position(rowIndex, x.Index)));
        return found.FirstOrDefault();
    }

    public T? Get(Position position)
    {
        if (position.Row < 0 || position.Row >= Grid.Length)
            return default;
        if (position.Column < 0 || position.Column >= Grid[position.Row].Length)
            return default;
        return Grid[position.Row][position.Column];
    }
}