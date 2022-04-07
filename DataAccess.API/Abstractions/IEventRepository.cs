using DataAccess.API.DTO;

namespace DataAccess.API.Abstractions;

public interface IEventRepository : IRepository<ILibraryEvent>
{
    public IUser GetBorrower(ILibraryEvent libraryLibraryEvent);
    public IBook GetBorrowedBook(ILibraryEvent libraryLibraryEvent);
}