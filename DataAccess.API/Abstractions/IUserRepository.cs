using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.API.DTO;

namespace DataAccess.API.Abstractions;

public interface IUserRepository : IRepository<IUser>
{
    public Task<IEnumerable<IBook>> GetBooksLeasedByUserAsync(IUser user);
}