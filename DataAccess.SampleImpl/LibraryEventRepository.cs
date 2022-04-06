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

    public User GetBorrower(ILibraryEvent libraryEvent) => libraryEvent switch{
                                                               Lease l  => l.Borrower,
                                                               Return r => r.Lease.Borrower,
                                                               _ => throw MakeInvalidTypeException(nameof(libraryEvent),
                                                                        libraryEvent)
                                                           };

    public Book GetBorrowedBook(ILibraryEvent libraryEvent) => libraryEvent switch{
                                                                          Lease l  => l.LeaseBook,
                                                                          Return r => r.Lease.LeaseBook,
                                                                          _ => throw MakeInvalidTypeException(nameof(libraryEvent),
                                                                                   libraryEvent)
                                                                      };

    private ArgumentException MakeInvalidTypeException(string argumentName, object invalidArgument)
    {
        return new
            ArgumentException($"Argument {argumentName} should be of type {nameof(Lease)} or {nameof(Return)}. Instead got {invalidArgument.GetType()}");
    }
}