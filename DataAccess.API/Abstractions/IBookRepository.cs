using System.Collections.Generic;
using DataAccess.API.DTO;

namespace DataAccess.API.Abstractions;

public interface IBookRepository : IRepository<IBook>
{
    public void UpdateBookInfo(IBookInfo newInfo);

    public IEnumerable<IBookInfo> GetAllBookInfo();
    public IUser? GetUserWhoLeased(IBook book);
}