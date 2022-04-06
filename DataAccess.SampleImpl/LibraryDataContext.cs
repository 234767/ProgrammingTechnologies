using System;
using System.Collections.Generic;
using DataAccess.API.Abstractions;
using DataAccess.API.DTO;

namespace DataAccess.SampleImpl;

internal class LibraryDataContext : ILibraryDataContext
{
    internal readonly IDictionary<string, Book> _books;
    internal readonly ICollection<ILibraryEvent> _events;
    internal readonly ICollection<User> _users;

    public LibraryDataContext()
    {
        _books = new Dictionary<string, Book>();
        _events = new List<ILibraryEvent>();
        _users = new List<User>();
    }

    public IUserRepository Users => new UserRepository(this);
    public IBookRepository Books => new BookRepository(this);
    public IEventRepository Events => new LibraryEventRepository(this);
}