using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DataAccess.API.Abstractions;
using DataAccess.API.DTO;

namespace DataAccess.Database;

internal class BookRepository : IBookRepository
{
    private readonly LibraryDataContext _context;

    public BookRepository(LibraryDataContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(IBook item)
    {
        if ( _context.Books.Any(b => b.Id == item.Id) )
            return;
        //_context.Books.Add(item);
        //if ( !_context.BookInfos.ContainsKey(item.BookInfo.Id) )
        //{
        //    _context.BookInfos.Add(item.BookInfo.Id, item.BookInfo);
        //}
    }

    public async Task<IBook?> GetAsync(string id)
    {
        return _context.Books.FirstOrDefault(b => b.Id == id);
    }

    public async Task<IEnumerable<IBook>> WhereAsync(Expression<Func<IBook, bool>> predicate)
    {
        return ( await GetAllAsync() ).Where(predicate.Compile());
    }

    public async Task UpdateAsync(IBook item)
    {
        foreach ( IBook book in _context.Books )
        {
            if ( book.Id != item.Id )
                continue;

            //_context.Books.Remove(book);
            //_context.Books.Add(item);
            return;
        }
    }

    public async Task UpdateBookInfoAsync(IBookInfo newInfo)
    {
        //IBookInfo oldInfo = _context._bookInfo[newInfo.Id];
        //_context._bookInfo[newInfo.Id] = newInfo;

        //foreach ( IBook book in _context.Books.Where(b => b.BookInfo.Equals(oldInfo)) )
        //{
        //    book.BookInfo = newInfo;
        //}
    }

    public async Task DeleteAsync(string id)
    {
        //IBook bookToRemove = _context.Books.First(b => b.Id == id);
        //_context.Books.Remove(bookToRemove);
        //if ( _context.Books.All(book => book.BookInfo.Id != bookToRemove.BookInfo.Id) )
        //{
        //    _context._bookInfo.Remove(bookToRemove.BookInfo.Id);
        //}
    }

    public async Task<IEnumerable<IBook>> GetAllAsync() => _context.Books;

    public async Task<IEnumerable<IBookInfo>> GetAllBookInfoAsync() => null;//_context._bookInfo.Values;

    public async Task<IUser?> GetUserWhoLeased(IBook book)
    {
        return null;
        //    return _context._events
        //                   .OfType<ILease>()
        //                   .FirstOrDefault(l => l.LeasedBook == book && _context._events.OfType<IReturn>().All(r => r.Lease != l))
        //                   ?.Borrower;
    }
}