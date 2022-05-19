using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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

    public async Task CreateAsync(IBook item)
    {
        if ( _context._books.Any(b => b.Id == item.Id) )
            return;
        _context._books.Add(item);
        if ( !_context._bookInfo.ContainsKey(item.BookInfo.Id) )
        {
            _context._bookInfo.Add(item.BookInfo.Id, item.BookInfo);
        }
    }

    public async Task<IBook?> GetAsync(string id)
    {
        return _context._books.FirstOrDefault(b => b.Id == id);
    }

    public async Task<IEnumerable<IBook>> WhereAsync(Expression<Func<IBook, bool>> predicate)
    {
        return ( await GetAllAsync() ).Where(predicate.Compile());
    }

    public async Task UpdateAsync(IBook item)
    {
        foreach ( IBook book in _context._books )
        {
            if ( book.Id != item.Id )
                continue;

            _context._books.Remove(book);
            _context._books.Add(item);
            return;
        }
    }

    public async Task UpdateBookInfoAsync(IBookInfo newInfo)
    {
        IBookInfo oldInfo = _context._bookInfo[newInfo.Id];
        _context._bookInfo[newInfo.Id] = newInfo;

        foreach ( IBook book in _context._books.Where(b => b.BookInfo.Equals(oldInfo)) )
        {
            book.BookInfo = newInfo;
        }
    }

    public async Task DeleteAsync(string id)
    {
        IBook bookToRemove = _context._books.First(b => b.Id == id);
        _context._books.Remove(bookToRemove);
        if ( _context._books.All(book => book.BookInfo.Id != bookToRemove.BookInfo.Id) )
        {
            _context._bookInfo.Remove(bookToRemove.BookInfo.Id);
        }
    }

    public async Task<IEnumerable<IBook>> GetAllAsync() => _context._books;

    public async Task<IEnumerable<IBookInfo>> GetAllBookInfoAsync() => _context._bookInfo.Values;

    public async Task<IUser?> GetUserWhoLeased(IBook book)
    {
        return _context._events
                       .OfType<ILease>()
                       .FirstOrDefault(l => l.LeasedBook == book && _context._events.OfType<IReturn>().All(r => r.Lease != l))
                       ?.Borrower;
    }
}