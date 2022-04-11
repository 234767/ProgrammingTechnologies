namespace BusinessLogic.Abstractions;

public interface ILibraryService
{
    void AddUser(string id, string name, string surname);
    void AddBook(IBookInfo bookInfo);
    IBookInfo GetBookInfoById(string bookId);
    void RemoveUser(string userId);
    void RemoveBook(string bookId);
    bool TryBorrow(string userId, string bookId);
    void ReturnBook(string bookId);
}