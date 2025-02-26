using System.Collections.Immutable;

namespace Task6;

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