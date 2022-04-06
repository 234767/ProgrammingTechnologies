using DataAccess.API.DTO;

namespace DataAccess.API.Abstractions;

public interface IEventRepository : IRepository<ILibraryEvent>
{
    public User GetBorrower(ILibraryEvent libraryLibraryEvent);
    public Book GetBorrowedBook(ILibraryEvent libraryLibraryEvent);
}