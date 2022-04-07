using DataAccess.API.DTO;

namespace DataAccess.API.Abstractions;

public interface IBookRepository : IRepository<IBook>
{
    public IUser? GetUserWhoLeased(IBook book);
}