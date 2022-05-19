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

    public async Task AddUser(string id, string name, string surname)
    {
        await _users.CreateAsync(new User(id, name, surname));
    }

    public async Task AddBook(IBookInfo bookInfo)
    {
        DataAccess.API.DTO.IBookInfo? info = ( await _books.GetAllBookInfoAsync() )
            .FirstOrDefault(info => info.Author == bookInfo.Author
                                    && info.Title == bookInfo.Title
                                    && info.DatePublished == bookInfo.DateOfIssue);
        info ??= new BookInfo(Guid.NewGuid().ToString(), bookInfo.Title, bookInfo.Author, bookInfo.DateOfIssue);
        await _books.CreateAsync(new Book(bookInfo.BookId, info));
    }

    private record BookInfImpl(string BookId, string Author, string Title, DateOnly? DateOfIssue) : IBookInfo;

    public async Task<IBookInfo?> GetBookInfoById(string bookId)
    {
        var book = await _books.GetAsync(bookId);
        if ( book is null )
        {
            return null;
        }
        return new BookInfImpl(bookId, book.BookInfo.Author, book.BookInfo.Title, book.BookInfo.DatePublished);
    }

    public async Task RemoveUser(string userId)
    {
        await _users.DeleteAsync(userId);
    }

    public async Task RemoveBook(string bookId)
    {
        await _books.DeleteAsync(bookId);
    }

    public async Task<bool> TryBorrow(string userId, string bookId)
    {
        ILibraryEvent? lastEvent = await _events.GetLatestEventForBookAsync(bookId);
        if ( lastEvent is ILease )
            return false;


        IBook? leasedBook = await _books.GetAsync(bookId);
        IUser? borrower = await _users.GetAsync(userId);
        if ( leasedBook is null || borrower is null )
            return false;

        await _events.CreateAsync(new Lease(Guid.NewGuid().ToString(), DateTime.Now, leasedBook, borrower, TimeSpan.MaxValue));
        return true;
    }

    public async Task ReturnBook(string bookId)
    {
        ILibraryEvent? lastEvent = await _events.GetLatestEventForBookAsync(bookId);
        if ( lastEvent is not Lease l )
            throw new InvalidOperationException("Cannot return book when it is not borrowed");

        await _events.CreateAsync(new Return(Guid.NewGuid().ToString(), l, DateTime.Now));
    }
}