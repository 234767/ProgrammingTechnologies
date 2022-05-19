namespace BusinessLogic.Abstractions;

public interface ILibraryService
{
    Task AddUser(string id, string name, string surname);
    Task AddBook(IBookInfo bookInfo);
    Task<IBookInfo?> GetBookInfoById(string bookId);
    Task RemoveUser(string userId);
    Task RemoveBook(string bookId);
    Task<bool> TryBorrow(string userId, string bookId);
    Task ReturnBook(string bookId);
}