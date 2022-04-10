using System.Collections.Generic;
using DataAccess.API.Abstractions;
using DataAccess.API.DTO;

namespace DataAccess.SampleImpl;

internal class LibraryDataContext : ILibraryDataContext
{
    internal readonly IDictionary<string, IBookInfo> _bookInfo;
    internal readonly ICollection<IBook> _books;
    internal readonly ICollection<ILibraryEvent> _events;
    internal readonly ICollection<IUser> _users;

    public LibraryDataContext()
    {
        _books = new List<IBook>();
        _bookInfo = new Dictionary<string, IBookInfo>();
        _events = new List<ILibraryEvent>();
        _users = new List<IUser>();
    }

    public IUserRepository Users => new UserRepository(this);
    public IBookRepository Books => new BookRepository(this);
    public IEventRepository Events => new LibraryEventRepository(this);
}