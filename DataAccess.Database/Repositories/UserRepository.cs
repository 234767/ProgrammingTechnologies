using System.Threading.Tasks;
using AutoMapper;
using DataAccess.API.Abstractions;
using DataAccess.API.DTO;
using DataAccess.Database.Dto;
using DataAccess.Database.Records;

namespace DataAccess.Database.Repositories;

internal class UserRepository : RepositoryBase<IUser, UserDto, User>, IUserRepository
{
    protected override UserDto? MapToDto( IUser? src )
    {
        return src is null ? null : new UserDto { Id = src.Id, FirstName = src.FirstName, Surname = src.Surname };
    }

    protected override User? MapToResult( IUser? src )
    {
        return src is null ? null : new User( src.Id, src.FirstName, src.Surname );
    }

    public UserRepository( LibraryDataContext dbContext ) : base( dbContext.Users, dbContext ) { }

    public override async Task UpdateAsync( IUser item )
    {
        UserDto? user = await dbSet.FindAsync(item.Id);
        if (user is not null)
        {
            user.FirstName = item.FirstName;
            user.Surname = item.Surname;
            await SaveChanges();
        }
    }
}