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
        }
    }

    public void Delete(string id)
    {
        _context._events.Remove(_context._events.Single(e => e.Id == id));
    }

    public IEnumerable<ILibraryEvent> GetAll() => _context._events;

    public IUser GetBorrower(ILibraryEvent libraryEvent) => libraryEvent switch{
                                                               ILease l  => l.Borrower,
                                                               IReturn r => r.Lease.Borrower,
                                                               _ => throw MakeInvalidTypeException(nameof(libraryEvent),
                                                                        libraryEvent)
                                                           };

    public IBook GetBorrowedBook(ILibraryEvent libraryEvent) => libraryEvent switch{
                                                                          ILease l  => l.LeasedBook,
                                                                          IReturn r => r.Lease.LeasedBook,
                                                                          _ => throw MakeInvalidTypeException(nameof(libraryEvent),
                                                                                   libraryEvent)
                                                                      };

    private ArgumentException MakeInvalidTypeException(string argumentName, object invalidArgument)
    {
        return new
            ArgumentException($"Argument {argumentName} should be of type {nameof(ILease)} or {nameof(IReturn)}. Instead got {invalidArgument.GetType()}");
    }
}