using DataAccess.API.Abstractions;
using DataAccess.API.DTO;

namespace DataAccess.SampleImpl;

public class LibraryDataContext : ILibraryDataContext
{
    private readonly IDictionary<string, Book> _books;
    private readonly ICollection<ILibraryEvent> _events;
    private readonly ICollection<User> _users;

    public IUserRepository Users => throw new NotImplementedException();
    public IBookRepository Books => throw new NotImplementedException();
    public IEventRepository Events => throw new NotImplementedException();
}