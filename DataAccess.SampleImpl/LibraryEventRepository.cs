using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DataAccess.API.Abstractions;
using DataAccess.API.DTO;

namespace DataAccess.Database;

internal class LibraryEventRepository : IEventRepository
{
    private readonly LibraryDataContext _context;

    public LibraryEventRepository(LibraryDataContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(ILibraryEvent libraryEvent)
    {
        //if ( _context._events.Any(e => e.Id == libraryEvent.Id) )
        //    return;
        //_context._events.Add(libraryEvent);
    }

    public async Task<ILibraryEvent?> GetAsync( string id ) => null;//_context._events.FirstOrDefault(e => e.Id.Equals(id));

    public async Task<IEnumerable<ILibraryEvent>> WhereAsync(Expression<Func<ILibraryEvent, bool>> predicate)
    {
        return null;//_context._events.Where(predicate.Compile());
    }

    public async Task UpdateAsync(ILibraryEvent item)
    {
        //foreach ( ILibraryEvent libraryEvent in _context._events )
        //{
        //    if ( libraryEvent.Id != item.Id )
        //        continue;

        //    _context._events.Remove(libraryEvent);
        //    _context._events.Add(item);
        //    return;
        //}
    }

    public async Task DeleteAsync(string id)
    {
        //_context._events.Remove(_context._events.Single(e => e.Id == id));
    }

    public async Task<IEnumerable<ILibraryEvent>> GetAllAsync() => null;//_context._events;

    public async Task<ILibraryEvent?> GetLatestEventForBookAsync( string bookId ) => null;//_context._events.Where(e => GetBookFromEvent(e)?.Id == bookId)
                                                                                     //.OrderByDescending(e => e.Time)
                                                                                     //.FirstOrDefault();

    private static IBook? GetBookFromEvent(ILibraryEvent e) => e switch{
                                                                   ILease l  => l.LeasedBook,
                                                                   IReturn r => r.Lease.LeasedBook,
                                                                   var _     => null
                                                               };
}