using DataAccess.API.DTO;

namespace DataAccess.API.Abstractions;

public interface IBookRepository : IRepository<Book>
{
    public User? GetUserWhoLeased(Book book);
}