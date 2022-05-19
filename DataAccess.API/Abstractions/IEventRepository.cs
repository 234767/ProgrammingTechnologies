using System.Threading.Tasks;
using DataAccess.API.DTO;

namespace DataAccess.API.Abstractions;

public interface IEventRepository : IRepository<ILibraryEvent>
{
    public Task<ILibraryEvent?> GetLatestEventForBookAsync(string bookId);
}