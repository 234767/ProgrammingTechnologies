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

    public void Create(IUser user)
    {
        _context._users.Add(user);
    }

    public IUser? Get(string id) => _context._users.FirstOrDefault(u => u.Id.Equals(id));

    public IEnumerable<IUser> Where(Expression<Func<IUser, bool>> predicate)
    {
        return _context._users.Where(predicate.Compile());
    }

    public void Update(IUser item)
    {
        foreach ( IUser user in _context._users )
        {
            if ( user.Id != item.Id )
                continue;

            _context._users.Remove(user);
            _context._users.Add(item);
        }
    }

    public void Delete(string id)
    {
        _context._users.Remove(_context._users.Single(u => u.Id.Equals(id)));
    }

    public IEnumerable<IUser> GetAll() => _context._users;

    public IEnumerable<IBook> GetBooksLeasedBy(IUser user)
    {
        return from lease in _context._events.OfType<ILease>()
               where !_context._events.OfType<IReturn>().Any(r => r.Lease.Equals(lease))
               select lease.LeasedBook;
    }
}