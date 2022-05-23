using BusinessLogic.Abstractions.Models;
using BusinessLogic.Models;

namespace BusinessLogic.Abstractions;

public interface ILibraryService
{
    Task AddUser(IUserModel user);
    Task AddBook(IBookModel book);
    Task SaveUser(IUserModel userModel);
    Task RemoveUser(string userId);
    Task RemoveBook(string bookId);

    /// <summary>
    /// Tries to create new lease of the book with specified <paramref name="bookId"/> to the user with specified <paramref name="userId"/>
    /// </summary>
    /// <param name="userId">Id of the user who wants to borrow the book</param>
    /// <param name="bookId">Id if the book that is to be borrowed</param>
    /// <returns>
    /// true if the book was successfully borrowed; false if the book was already borrowed, user did not exist or book did not exist
    /// </returns>
    Task<bool> TryBorrow(string userId, string bookId);

    /// <summary>
    /// Tries to register a return of a book
    /// </summary>
    /// <param name="bookId">Id of the book being returned</param>
    /// <exception cref="InvalidOperationException"></exception>
    Task ReturnBook(string bookId);

    Task<IEnumerable<IUserModel>> SearchUsers( string? name );
    Task<IEnumerable<IBookModel>> SearchBooks( string property, string? name );
    Task SaveBook( IBookModel bookModel );
    Task<bool> IsBookAvailable( string id );
}