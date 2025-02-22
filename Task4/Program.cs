using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Task4;

class Program
{
    static void Main(string[] args)
    {
        var lines = File.ReadAllLines("Input3.txt");
        var wordBoard = new WordBoard(lines);
        Console.WriteLine(XmasCounts(wordBoard));
        Console.WriteLine(MasXCounts(wordBoard));
    }

    private static int XmasCounts(WordBoard wordBoard)
    {
        const string word = "XMAS";
        var wordStarts = wordBoard.IndicesOf(word[0]);
        var words = wordStarts.SelectMany(
            start => wordBoard.GetAllWords(start, word.Length, WordBoard.Index.Directions));

        int count = words
            .Count(x => x.ToString() == word);
        return count;
    }

    private static int MasXCounts(WordBoard wordBoard)
    {
        const string word = "MAS";
        var wordStarts = wordBoard.IndicesOf(word[0]);
        var words = wordStarts.SelectMany(start =>
            wordBoard.GetAllWords(start, word.Length, WordBoard.Index.Diagonals))
            .Where(x => x.ToString() == word);
        var groupedByMiddle = words.GroupBy(x => x.Indices[1]);
        var count = groupedByMiddle.Count(group =>
            group.Any(x =>
                x.Direction == WordBoard.Index.DiagonalUpRight || x.Direction == WordBoard.Index.DiagonalDownLeft) &&
            group.Any(x =>
                x.Direction == WordBoard.Index.DiagonalDownRight || x.Direction == WordBoard.Index.DiagonalUpLeft));
        return count;
    }
}

class WordBoard
{
    private readonly IReadOnlyList<IReadOnlyList<char>> _grid;

    public WordBoard(string[] inputLines)
    {
        _grid = [..inputLines.Select(x => x.ToImmutableArray())];
    }

    public IEnumerable<Index> IndicesOf(char searchCharacter) =>
        _grid.SelectMany((line, lineIndex) => line.Index()
            .Where(character => character.Item == searchCharacter)
            .Select(character => new Index(lineIndex, character.Index, _grid)));

    public IEnumerable<Word>
        GetAllWords(Index index, int length, params IReadOnlyList<Func<Index, Index>> directions) =>
        directions.Select(direction => GetWord(index, length, direction)).OfType<Word>();

    public Word? GetWord(Index index, int length, Func<Index, Index> direction)
    {
        var indices = Enumerable.Range(0, length - 1)
            .Aggregate(
                new List<Index>() { index },
                (word, _) =>
                {
                    Index nextCharacter = direction(word.Last());
                    if (nextCharacter.Value is not null)
                        word.Add(nextCharacter);
                    return word;
                })
            .AsReadOnly();
        if (indices.Count == length)
            return new Word(indices, direction, length);
        return null;
    }

    public readonly record struct Word(IReadOnlyList<Index> Indices, Func<Index, Index> Direction, int Length)
    {
        public override string? ToString()
        {
            if (Indices is null) return null;
            return string.Join("", Indices.Select(x => x.Value));
        }
    }

    public readonly record struct Index
    {
        private readonly IReadOnlyList<IReadOnlyList<char>> _grid;

        public Index(int Row, int Column, IReadOnlyList<IReadOnlyList<char>> grid)
        {
            this.Row = Row;
            this.Column = Column;
            _grid = grid;
        }

        public static IReadOnlyList<Func<Index, Index>> Directions { get; } =
            [Up, Down, Left, Right, DiagonalUpLeft, DiagonalUpRight, DiagonalDownLeft, DiagonalDownRight];

        public static IReadOnlyList<Func<Index, Index>> Diagonals { get; } =
            [DiagonalUpLeft, DiagonalUpRight, DiagonalDownLeft, DiagonalDownRight];

        public static Index Up(Index current) => new(current.Row - 1, current.Column, current._grid);
        public static Index Down(Index current) => new(current.Row + 1, current.Column, current._grid);
        public static Index Left(Index current) => new(current.Row, current.Column - 1, current._grid);
        public static Index Right(Index current) => new(current.Row, current.Column + 1, current._grid);
        public static Index DiagonalUpLeft(Index current) => new(current.Row - 1, current.Column - 1, current._grid);
        public static Index DiagonalUpRight(Index current) => new(current.Row - 1, current.Column + 1, current._grid);
        public static Index DiagonalDownRight(Index current) => new(current.Row + 1, current.Column + 1, current._grid);
        public static Index DiagonalDownLeft(Index current) => new(current.Row + 1, current.Column - 1, current._grid);


        public char? Value
        {
            get
            {
                if (Row < _grid.Count && Row >= 0 && Column < _grid[Row].Count && Column >= 0)
                    return _grid[Row][Column];
                return null;
            }
        }

        public int Row { get; init; }
        public int Column { get; init; }
    };
}