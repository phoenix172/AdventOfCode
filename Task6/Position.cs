namespace Task6;

public readonly struct Position : IEquatable<Position>
{
    public int Row { get; }
    public int Column { get; }

    public Position(int row, int column)
    {
        Row = row;
        Column = column;
    }

    public Position Move(Direction direction) => new(Row + direction.YOffset, Column + direction.XOffset);

    public bool Equals(Position other)
    {
        return Row == other.Row && Column == other.Column;
    }

    public override bool Equals(object? obj)
    {
        return obj is Position other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Row, Column);
    }
};