namespace DataAccess.API.Abstractions;

public interface ILibraryDataContext
{
    public IUserRepository Users { get; }
    public IBookRepository Books { get; }
    public IEventRepository Events { get; }
}