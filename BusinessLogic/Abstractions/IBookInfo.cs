namespace BusinessLogic.Abstractions;

public interface IBookInfo
{
    string BookId { get; }
    string Author { get; }
    string Title { get; }
    DateOnly? DateOfIssue { get; }
}