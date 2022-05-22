using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DataAccess.API.DTO;

namespace DataAccess.API.Abstractions;

public interface IRepository<T>
{
    public Task CreateAsync(T item);
    public Task<T?> GetAsync(string id);
    public Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>> predicate);
    public Task UpdateAsync(T item);
    public Task DeleteAsync(string id);
    public Task<IEnumerable<T>> GetAllAsync();

}