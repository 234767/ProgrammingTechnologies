using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.API.DTO;

namespace DataAccess.API.Abstractions;

public interface IBookRepository : IRepository<IBook>
{
    public Task UpdateBookInfoAsync(IBookInfo newInfo);

    public Task<IEnumerable<IBookInfo>> GetAllBookInfoAsync();
    public Task<IUser?> GetUserWhoLeased(IBook book);
}