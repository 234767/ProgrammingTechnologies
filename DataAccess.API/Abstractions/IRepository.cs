using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DataAccess.API.DTO;

namespace DataAccess.API.Abstractions;

public interface IRepository<T>
{
    public void Create(T user);
    public T? Get(string id);
    public IEnumerable<T> Where(Expression<Func<ILibraryEvent, bool>> predicate);
    public void Update(T item);
    public void Delete(string id);
    public IEnumerable<T> GetAll();

}