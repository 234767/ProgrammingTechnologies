using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.API.DTO;

namespace DataAccess.API.Abstractions;

public interface IBookRepository : IRepository<IBook>
{
    public Task<IEnumerable<IBookInfo?>> FindBookInfoAsync( string? author, string? title );
}