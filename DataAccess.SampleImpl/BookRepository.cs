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

    public void Create(Book item)
    {
        _context._books.Add(item.Id, item);
    }

    public Book? Get(string id)
    {
        return _context._books.TryGetValue(id, out Book? book) ? book : null;
    }

    public IEnumerable<Book> Where(Expression<Func<Book, bool>> predicate)
    {
        return GetAll().Where(predicate.Compile());
    }

    public void Update(Book item)
    {
        _context._books[item.Id] = item;
    }

    public void Delete(string id)
    {
        _context._books.Remove(id);
    }

    public IEnumerable<Book> GetAll() => _context._books.Select(kvp => kvp.Value);

    public User? GetUserWhoLeased(Book book)
    {
        return _context._events.OfType<Lease>().OrderByDescending(l => l.Date).FirstOrDefault()?.Borrower;
    }
}