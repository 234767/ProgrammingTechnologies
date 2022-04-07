using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DataAccess.API.Abstractions;
using DataAccess.API.DTO;

namespace DataAccess.SampleImpl;

internal class BookRepository : IBookRepository
{
    private readonly LibraryDataContext _context;

    public BookRepository(LibraryDataContext context)
    {
        _context = context;
    }

    public void Create(IBook item)
    {
        _context._books.Add(item.Id, item);
    }

    public IBook? Get(string id)
    {
        return _context._books.TryGetValue(id, out IBook? book) ? book : null;
    }

    public IEnumerable<IBook> Where(Expression<Func<IBook, bool>> predicate)
    {
        return GetAll().Where(predicate.Compile());
    }

    public void Update(IBook item)
    {
        _context._books[item.Id] = item;
    }

    public void Delete(string id)
    {
        _context._books.Remove(id);
    }

    public IEnumerable<IBook> GetAll() => _context._books.Select(kvp => kvp.Value);

    public IUser? GetUserWhoLeased(IBook book)
    {
        return _context._events
                       .OfType<ILease>()
                       .FirstOrDefault(l => _context._events.OfType<IReturn>().All(r => r.Lease != l))
                       ?.Borrower;
    }
}