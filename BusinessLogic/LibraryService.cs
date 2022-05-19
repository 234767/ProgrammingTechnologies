using BusinessLogic.Abstractions;
using DataAccess.API.Abstractions;
using DataAccess.API.DTO;
using IBookInfo = BusinessLogic.Abstractions.IBookInfo;

namespace BusinessLogic;

public class LibraryService : ILibraryService
{
    private readonly IUserRepository _users;
    private readonly IBookRepository _books;
    private readonly IEventRepository _events;

    public LibraryService(IUserRepository users, IBookRepository books, IEventRepository events)
    {
        _users = users;
        _books = books;
        _events = events;
    }

    public void AddUser(string id, string name, string surname)
    {
        _users.CreateAsync(new User(id, name, surname));
    }

    public void AddBook(IBookInfo bookInfo)
    {
        DataAccess.API.DTO.IBookInfo? info = _books.GetAllBookInfoAsync()
                                                   .FirstOrDefault(info => info.Author == bookInfo.Author
                                                                           && info.Title == bookInfo.Title
                                                                           && info.DatePublished == bookInfo.DateOfIssue);
        info ??= new BookInfo(Guid.NewGuid().ToString(), bookInfo.Title, bookInfo.Author, bookInfo.DateOfIssue);
        _books.CreateAsync(new Book(bookInfo.BookId, info));
    }

    private record BookInfImpl(string BookId, string Author, string Title, DateOnly? DateOfIssue) : IBookInfo;
    public IBookInfo GetBookInfoById(string bookId)
    {
        var book = _books.GetAsync(bookId);
        return new BookInfImpl(bookId, book.BookInfo.Author, book.BookInfo.Title, book.BookInfo.DatePublished);
    }

    public void RemoveUser(string userId)
    {
        _users.DeleteAsync(userId);
    }

    public void RemoveBook(string bookId)
    {
        _books.DeleteAsync(bookId);
    }

    public bool TryBorrow(string userId, string bookId)
    {
        ILibraryEvent? lastEvent = _events.GetLatestEventForBookAsync(bookId);
        if ( lastEvent is ILease )
            return false;


        IBook? leasedBook = _books.GetAsync(bookId);
        IUser? borrower = _users.GetAsync(userId);
        if ( leasedBook is null || borrower is null )
            return false;
        
        _events.CreateAsync(new Lease(Guid.NewGuid().ToString(), DateTime.Now, leasedBook, borrower, TimeSpan.MaxValue));
        return true;
    }

    public void ReturnBook(string bookId)
    {
        ILibraryEvent? lastEvent = _events.GetLatestEventForBookAsync(bookId);
        if ( lastEvent is not Lease l )
            throw new InvalidOperationException("Cannot return book when it is not borrowed");
        
        _events.CreateAsync(new Return(Guid.NewGuid().ToString(), l, DateTime.Now));
    }
}