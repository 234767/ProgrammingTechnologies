using BusinessLogic.Abstractions;
using BusinessLogic.Abstractions.Models;
using DataAccess.API.Abstractions;
using DataAccess.API.DTO;

namespace BusinessLogic;

public class LibraryService 
{
    private readonly IUserRepository _users;
    private readonly IBookRepository _books;
    private readonly ILeaseRepository _leases;
    private readonly IReturnRepository _returns;

    public LibraryService(IUserRepository users, IBookRepository books, ILeaseRepository leases, IReturnRepository returns )
    {
        _users = users;
        _books = books;
        _returns = returns;
        _leases = leases;
    }

    public async Task AddUser(string id, string name, string surname)
    {
        await _users.CreateAsync(new User(id, name, surname));
    }

    public async Task AddBook(IBookModel book)
    {
        IBookInfo? bookInfo = (await _books.FindBookInfoAsync( book.Author, book.Title )).FirstOrDefault();
        bookInfo ??= new BookInfo( Guid.NewGuid().ToString(), book.Title, book.Author, book.DatePublished );
        await _books.CreateAsync( new Book( new Guid().ToString(), bookInfo ) );
    }


    public async Task RemoveUser(string userId)
    {
        await _users.DeleteAsync(userId);
    }

    public async Task RemoveBook(string bookId)
    {
        await _books.DeleteAsync(bookId);
    }


    /// <summary>
    /// Tries to create new lease of the book with specified <paramref name="bookId"/> to the user with specified <paramref name="userId"/>
    /// </summary>
    /// <param name="userId">Id of the user who wants to borrow the book</param>
    /// <param name="bookId">Id if the book that is to be borrowed</param>
    /// <returns>
    /// true if the book was successfully borrowed; false if the book was already borrowed, user did not exist or book did not exist
    /// </returns>
    public async Task<bool> TryBorrow(string userId, string bookId)
    {
        if (!await IsBookAvailable(bookId))
            return false;

        IBook? leasedBook = await _books.GetAsync(bookId);
        IUser? borrower = await _users.GetAsync(userId);
        if ( leasedBook is null || borrower is null )
            return false;

        // todo: set return date
        await _leases.CreateAsync(new Lease(Guid.NewGuid().ToString(), DateTime.Now, leasedBook, borrower, new DateTime()));
        return true;
    }

    /// <summary>
    /// Tries to register a return of a book
    /// </summary>
    /// <param name="bookId">Id of the book being returned</param>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task ReturnBook(string bookId)
    {
        ILease? lastEvent = (await _leases.WhereAsync(lease => lease.LeasedBook.Id == bookId)).OrderBy(lease => lease.Time).FirstOrDefault();
        if ( lastEvent is null )
            throw new InvalidOperationException("Cannot return book when it is not borrowed");

        if ( await IsBookAvailable(bookId) )
        {
            throw new InvalidOperationException( "Book has already been returned" );
        }

        await _returns.CreateAsync(new Return(Guid.NewGuid().ToString(), lastEvent, DateTime.Now));
    }

    private async Task<bool> IsBookAvailable( string bookId )
    {
        var leases = await _leases.WhereAsync( l => l.LeasedBook.Id == bookId );

        var returns = await _returns.WhereAsync( ret => ret.Lease.LeasedBook.Id == bookId );

        return leases.All( IsReturned );

        bool IsReturned(ILease lease)
        {
            return returns.Any( ret => ret.Lease.Id == lease.Id );
        }
    }
}