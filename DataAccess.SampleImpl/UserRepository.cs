using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess.API.Abstractions;
using DataAccess.API.DTO;
using DataAccess.Database.Dto;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Database;

internal class UserRepository : IUserRepository
{
    private readonly LibraryDataContext _context;
    private static readonly IMapper Mapper;

    static UserRepository()
    {
        Mapper = new MapperConfiguration(
            cfg =>
            {
                cfg.CreateMap<UserDto, IUser>();
            } ).CreateMapper();
    }

    public UserRepository(LibraryDataContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(IUser user)
    {
        if ( await _context.Users.AnyAsync(u => u.Id == user.Id) )
            return;
        await _context.Users.AddAsync(Mapper.Map<UserDto>(user));
    }

    public async Task<IUser?> GetAsync(string id) => await _context.Users.FirstOrDefaultAsync(u => u.Id.Equals(id));

    public async Task<IEnumerable<IUser>> WhereAsync(Expression<Func<IUser, bool>> predicate)
    {
        return await _context.Users.Where(predicate).ToListAsync();
    }

    public async Task UpdateAsync(IUser item)
    {
        await Task.Run(
            () =>
            {
                _context.Users.Update( Mapper.Map<UserDto>( item ) );
            } );
    }

    public async Task DeleteAsync(string id)
    {
        _context.Users.Remove(await _context.Users.SingleAsync(u => u.Id.Equals(id)));
    }

    public async Task<IEnumerable<IUser>> GetAllAsync() => await _context.Users.ToListAsync();

    public async Task<IEnumerable<IBook>> GetBooksLeasedByUserAsync(IUser user)
    {
        var result = from lease in _context.Leases
                     where lease.Borrower.Equals(user)
                           && _context.Returns.All(r => r.Lease.Id != lease.Id)
                     select lease.LeasedBook;

        return await result.ToListAsync();
    }
}