using System.Collections.Generic;
using DataAccess.API.DTO;

namespace DataAccess.API.Abstractions;

public interface IUserRepository : IRepository<IUser>
{
    public IEnumerable<IBook> GetBooksLeasedBy(IUser user);
}