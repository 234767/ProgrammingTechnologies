using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess.API.Abstractions;
using DataAccess.API.DTO;
using DataAccess.Database.Dto;
using DataAccess.Database.Records;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Database.Repositories
{
    internal class LeaseRepository : RepositoryBase<ILease, LeaseDto, Lease>, ILeaseRepository
    {
        public LeaseRepository( LibraryDataContext dbContext ) : base( dbContext.Leases, dbContext ) { }

        protected override IQueryable<LeaseDto> LoadRelations( IQueryable<LeaseDto> data )
        {
            return data.Include( l => l.Borrower ).Include( l => l.LeasedBook ).ThenInclude( b => b.BookInfo );
        }

        public override async Task<ILease?> GetAsync( string id )
        {
            return MapToResult(await LoadRelations( dbSet.Where(l => l.Id == id) ).SingleOrDefaultAsync());
        }

        protected override LeaseDto? MapToDto( ILease? src )
        {
            if ( src is null )
                return null;

            return new LeaseDto()
            {
                Id = src.Id,
                Time = src.Time,
                ReturnDate = src.ReturnDate,
                Borrower =
                    dbContext.Users.Find( src.Borrower.Id ) ?? throw new ArgumentException(
                        "User must be added before it can be assigned to a lease" ),
                LeasedBook = dbContext.Books.Find( src.LeasedBook.Id ) ??
                             throw new ArgumentException(
                                 "Book must be added before it can be assigned to a lease" )
            };
        }

        protected override Lease? MapToResult( ILease? src )
        {
            if ( src is null )
                return null;

            return new Lease(
                Id: src.Id,
                Time: src.Time,
                ReturnDate: src.ReturnDate,
                Borrower: new User(src.Borrower.Id, src.Borrower.FirstName, src.Borrower.Surname),
                LeasedBook: new Book(
                    src.LeasedBook.Id,
                    new BookInfo(
                        src.LeasedBook.BookInfo.Id,
                        src.LeasedBook.BookInfo.Title,
                        src.LeasedBook.BookInfo.Author,
                        src.LeasedBook.BookInfo.DatePublished)));
        }

        public override async Task UpdateAsync( ILease item )
        {
            LeaseDto? lease = await dbSet.FindAsync(item.Id);
            if (lease is not null)
            {
                BookDto? book = await dbContext.Books.FindAsync( item.LeasedBook.Id );
                if ( book is null )
                    throw new ArgumentException( "Book must be added before it can be assigned to a lease" );

                lease.LeasedBook = book;

                UserDto? user = await dbContext.Users.FindAsync(item.Borrower.Id);
                if (user is null)
                    throw new ArgumentException("User must be added before it can be assigned to a lease");

                lease.Borrower = user;

                lease.Time = item.Time;
                lease.ReturnDate = item.ReturnDate;

                await SaveChanges();
            }
        }
    }
}
