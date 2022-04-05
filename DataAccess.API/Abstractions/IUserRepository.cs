using System.Collections.Generic;
using DataAccess.API.DTO;

namespace DataAccess.API.Abstractions;

public interface IUserRepository : IRepository<User>
{
    public IEnumerable<Book> GetBooksLeasedBy(User user);
}