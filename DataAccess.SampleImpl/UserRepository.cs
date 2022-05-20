using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DataAccess.API.Abstractions;
using DataAccess.API.DTO;

namespace DataAccess.Database;

internal class UserRepository : IUserRepository
{
    private readonly LibraryDataContext _context;

    public UserRepository(LibraryDataContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(IUser user)
    {
        if ( _context.Users.Any(u => u.Id == user.Id) )
            return;
        //_context.Users.Add(user);
    }

    public async Task<IUser?> GetAsync(string id) => _context.Users.FirstOrDefault(u => u.Id.Equals(id));

    public async Task<IEnumerable<IUser>> WhereAsync(Expression<Func<IUser, bool>> predicate)
    {
        return _context.Users.Where(predicate.Compile());
    }

    public async Task UpdateAsync(IUser item)
    {
        foreach ( IUser user in _context.Users )
        {
            if ( user.Id != item.Id )
                continue;

            //_context.Users.Remove(user);
            //_context.Users.Add(item);
            return;
        }
    }

    public async Task DeleteAsync(string id)
    {
        _context.Users.Remove(_context.Users.Single(u => u.Id.Equals(id)));
    }

    public async Task<IEnumerable<IUser>> GetAllAsync() => _context.Users;

    public async Task<IEnumerable<IBook>> GetBooksLeasedByUserAsync(IUser user)
    {
        return null;
        //return from lease in _context._events.OfType<ILease>()
        //       where lease.Borrower.Equals(user)
        //       && _context._events.OfType<IReturn>().All(r => r.Lease.Id != lease.Id)
        //       select lease.LeasedBook;
    }
}