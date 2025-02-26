using System.Collections.Immutable;

namespace Task6;

public class Board<T>
{
    private T[][] Grid { get; }

    public Board(ImmutableArray<ImmutableArray<T>> grid)
    {
        Grid = grid.Select(x => x.ToArray()).ToArray();
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

    public void Set(Position pos, T value)
    {
        Grid[pos.Row][pos.Column] = value;
    }
}