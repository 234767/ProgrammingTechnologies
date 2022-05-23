using System.Collections;
using System.Linq.Expressions;
using BusinessLogic.Abstractions;
using BusinessLogic.Abstractions.Models;
using BusinessLogic.Models;
using DataAccess.API.Abstractions;
using DataAccess.API.DTO;

namespace BusinessLogic;

public class LibraryService : ILibraryService
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

    public async Task AddUser(IUserModel user)
    {
        await _users.CreateAsync(new User(GenerateId(), user.FirstName, user.Surname));
    }

    public async Task SaveUser(IUserModel user)
    {
        await _users.UpdateAsync( new User( user.Id, user.FirstName, user.Surname ) );
    }

    public async Task<IEnumerable<IUserModel>> SearchUsers( string? name )
    {
        IEnumerable<IUser> result;
        if ( string.IsNullOrWhiteSpace( name ) )
        {
            result = ( await _users.GetAllAsync() ).ToList();
        }
        else
        {
            string[] names = name.Split(' ');
            string firstName = names[0];
            string? lastName = names.Length > 1 ? names[1] : null;

            Expression<Func<IUser, bool>> queryExpression = lastName is null
                ? user => user.FirstName == firstName : u => u.FirstName == firstName && u.Surname == lastName;

            result = (await _users.WhereAsync(queryExpression)).ToList();
        }

        return result.Select( u => new UserModel( this, u.Id, u.FirstName, u.Surname ) );
    }

    public async Task UpdateUser( IUserModel user )
    {
        await _users.UpdateAsync( new User( user.Id, user.FirstName, user.Surname ) );
    }

    public async Task AddBook(IBookModel book)
    {
        IBookInfo? bookInfo = (await _books.FindBookInfoAsync( book.Author, book.Title )).FirstOrDefault();
        bookInfo ??= new BookInfo( Guid.NewGuid().ToString(), book.Title, book.Author, book.DatePublished );
        await _books.CreateAsync( new Book( GenerateId(), bookInfo ) );
    }

    public async Task<IEnumerable<IBookModel>> SearchBooks( string property, string? name )
    {
        IEnumerable<IBook> results;
        if ( string.IsNullOrWhiteSpace( name ) )
        {
            results = await _books.GetAllAsync();
        }
        else
        {
            results = property.ToLower() switch
            {
                "title"  => ( await _books.WhereAsync( b => b.BookInfo.Title == name ) ).ToList(),
                "author" => ( await _books.WhereAsync( b => b.BookInfo.Author == name ) ).ToList(),
                _        => throw new InvalidOperationException( "Books have to be searched either by Title or Author" )
            };
        }

        List<BookModel> returnValue = new();
        foreach ( IBook book in results )
        {
            returnValue.Add(
                new BookModel(
                    this,
                    book.Id,
                    book.BookInfo.Title,
                    book.BookInfo.Author,
                    book.BookInfo.DatePublished is null
                        ? null
                        : DateOnly.FromDateTime( book.BookInfo.DatePublished.Value ),
                    await IsBookAvailable( book.Id ) ) );
        }

        return returnValue;
    }

    public async Task SaveBook( IBookModel bookModel )
    {
        // todo: fix BookInfoId
        await _books.UpdateAsync(
            new Book(
                GenerateId(),
                new BookInfo( bookModel.Id, bookModel.Title, bookModel.Author, bookModel.DatePublished ) ) );
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
        await _leases.CreateAsync(new Lease(GenerateId(), DateTime.Now, leasedBook, borrower, new DateTime()));
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

    public async Task<bool> IsBookAvailable( string bookId )
    {
        //return true;

        var leases = ( await _leases.WhereAsync( l => l.LeasedBook.Id == bookId ) ).ToList();

        if ( leases.Count == 0 )
            return true;

        foreach ( ILease lease in leases )
        {
            if ( !await IsLeaseReturned( lease ) )
                return false;
        }

        return true;
    }

    public async Task<IEnumerable<ILeaseModel>> GetAllLeases()
    {
        // Todo
        var leases = await _leases.GetAllAsync();
        var returns = await _returns.GetAllAsync();
        return leases.Select(
            l => new LeaseModel(
                this,
                l.Id,
                l.Time,
                l.ReturnDate,
                returns.SingleOrDefault( r => r.Lease.Id == l.Id )?.Time,
                new UserModel( this, l.Borrower.Id, l.Borrower.FirstName, l.Borrower.Surname ),
                new BookModel(
                    this,
                    l.LeasedBook.Id,
                    l.LeasedBook.BookInfo.Title,
                    l.LeasedBook.BookInfo.Author,
                    null,
                    false ) ) );
    }

    public async Task<bool> IsLeaseReturned(ILease lease)
    {
        var returns = (await _returns.WhereAsync(ret => ret.Lease.Id == lease.Id)).ToList();
        return returns.Any(ret => ret.Lease.Id == lease.Id);
    }

    private string GenerateId() => Guid.NewGuid().ToString()[..8];
}