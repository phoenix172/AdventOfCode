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