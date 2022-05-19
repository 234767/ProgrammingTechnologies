using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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

    public async Task CreateAsync(IUser user)
    {
        if ( _context._users.Any(u => u.Id == user.Id) )
            return;
        _context._users.Add(user);
    }

    public async Task<IUser?> GetAsync(string id) => _context._users.FirstOrDefault(u => u.Id.Equals(id));

    public async Task<IEnumerable<IUser>> WhereAsync(Expression<Func<IUser, bool>> predicate)
    {
        return _context._users.Where(predicate.Compile());
    }

    public async Task UpdateAsync(IUser item)
    {
        foreach ( IUser user in _context._users )
        {
            if ( user.Id != item.Id )
                continue;

            _context._users.Remove(user);
            _context._users.Add(item);
            return;
        }
    }

    public async Task DeleteAsync(string id)
    {
        _context._users.Remove(_context._users.Single(u => u.Id.Equals(id)));
    }

    public async Task<IEnumerable<IUser>> GetAllAsync() => _context._users;

    public async Task<IEnumerable<IBook>> GetBooksLeasedByUserAsync(IUser user)
    {
        return from lease in _context._events.OfType<ILease>()
               where lease.Borrower.Equals(user)
               && _context._events.OfType<IReturn>().All(r => r.Lease.Id != lease.Id)
               select lease.LeasedBook;
    }
}