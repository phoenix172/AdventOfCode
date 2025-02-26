using Newtonsoft.Json;

namespace Task5.Tests;

public class Root
{
    public record RootObject(
        Books[] Books,
        Users[] Users
    );

    public record Books(
        int Id,
        string Title,
        string Author,
        int PublishedYear,
        string Genre,
        bool IsAvailable
    );

    public record Users(
        int Id,
        string Name,
        string Email,
        BorrowedBooks[] BorrowedBooks
    );

    public record BorrowedBooks(
        int BookId,
        string BorrowDate,
        string DueDate
    );
}