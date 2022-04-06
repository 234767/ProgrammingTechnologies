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

    public IUserRepository Users => new UserRepository(this);
    public IBookRepository Books => throw new NotImplementedException();
    public IEventRepository Events => new LibraryEventRepository(this);
}