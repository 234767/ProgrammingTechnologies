using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DataAccess.API.Abstractions;
using DataAccess.API.DTO;

namespace DataAccess.SampleImpl;

internal class UserRepository : IUserRepository
{
    private readonly LibraryDataContext _context;

    public UserRepository(LibraryDataContext context)
    {
        _context = context;
    }

    public void Create(User user)
    {
        _context._users.Add(user);
    }

    public User? Get(string id) => _context._users.FirstOrDefault(u => u.Id.Equals(id));

    public IEnumerable<User> Where(Expression<Func<User, bool>> predicate)
    {
        return _context._users.Where(predicate.Compile());
    }

    public void Update(User item)
    {
        {
            foreach ( User user in _context._users )
            {
                if ( user.Id != item.Id )
                    continue;

                _context._users.Remove(user);
                _context._users.Add(item);
            }
        }
    }

    public void Delete(string id)
    {
        _context._users.Remove(_context._users.Single(u => u.Id.Equals(id)));
    }

    public IEnumerable<User> GetAll() => _context._users;

    public IEnumerable<Book> GetBooksLeasedBy(User user)
    {
        foreach ( KeyValuePair<string, Book> keyValuePair in _context._books )
        {
            ( _, Book b ) = keyValuePair;
        }

        return from lease in _context._events.OfType<Lease>()
               where !_context._events.OfType<Return>().Any(r => r.Lease.Equals(lease))
               select lease.LeaseBook;
    }
}