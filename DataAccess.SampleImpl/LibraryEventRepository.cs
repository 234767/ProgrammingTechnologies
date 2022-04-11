using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DataAccess.API.Abstractions;
using DataAccess.API.DTO;

namespace DataAccess.SampleImpl;

internal class LibraryEventRepository : IEventRepository
{
    private readonly LibraryDataContext _context;

    public LibraryEventRepository(LibraryDataContext context)
    {
        _context = context;
    }

    public void Create(ILibraryEvent libraryEvent)
    {
        if ( _context._events.Any(e => e.Id == libraryEvent.Id) )
            return;
        _context._events.Add(libraryEvent);
    }

    public ILibraryEvent? Get(string id) => _context._events.FirstOrDefault(e => e.Id.Equals(id));

    public IEnumerable<ILibraryEvent> Where(Expression<Func<ILibraryEvent, bool>> predicate)
    {
        return _context._events.Where(predicate.Compile());
    }

    public void Update(ILibraryEvent item)
    {
        foreach ( ILibraryEvent libraryEvent in _context._events )
        {
            if ( libraryEvent.Id != item.Id )
                continue;

            _context._events.Remove(libraryEvent);
            _context._events.Add(item);
            return;
        }
    }

    public void Delete(string id)
    {
        _context._events.Remove(_context._events.Single(e => e.Id == id));
    }

    public IEnumerable<ILibraryEvent> GetAll() => _context._events;

    public ILibraryEvent? GetLatestEventForBook(IBook book) => _context._events.Where(e => GetBookFromEvent(e) == book)
                                                                       .OrderByDescending(e => e.Time)
                                                                       .FirstOrDefault();

    private static IBook? GetBookFromEvent(ILibraryEvent e) => e switch{
                                                                   ILease l  => l.LeasedBook,
                                                                   IReturn r => r.Lease.LeasedBook,
                                                                   var _     => null
                                                               };
}