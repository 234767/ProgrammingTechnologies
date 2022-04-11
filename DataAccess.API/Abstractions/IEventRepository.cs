using DataAccess.API.DTO;

namespace DataAccess.API.Abstractions;

public interface IEventRepository : IRepository<ILibraryEvent>
{
    public ILibraryEvent? GetLatestEventForBook(string bookId);
}