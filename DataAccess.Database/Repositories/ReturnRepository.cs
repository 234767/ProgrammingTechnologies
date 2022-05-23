using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess.API.Abstractions;
using DataAccess.API.DTO;
using DataAccess.Database.Dto;
using DataAccess.Database.Records;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Database.Repositories
{
    internal class ReturnRepository : RepositoryBase<IReturn, ReturnDto, Return>, IReturnRepository
    {
        public ReturnRepository( LibraryDataContext dbContext ) : base( dbContext.Returns, dbContext ) { }

        protected override IQueryable<ReturnDto> LoadRelations( IQueryable<ReturnDto> data )
        {
            return data.Include( r => r.Lease )
                       .ThenInclude( l => l.Borrower )
                       .Include( r => r.Lease )
                       .ThenInclude( l => l.LeasedBook )
                       .ThenInclude( b => b.BookInfo );
        }

        public override async Task<IReturn?> GetAsync( string id )
        {
            return MapToResult( await LoadRelations( dbSet.Where( r => r.Id == id ) ).FirstOrDefaultAsync() );
        }

        protected override ReturnDto? MapToDto( IReturn? src )
        {
            if (src is null)
                return null;

            return new ReturnDto
            {
                Id = src.Id,
                Lease = dbContext.Leases.Find( src.Lease.Id ) ??
                        throw new ArgumentException( "Lease associated with return does not exist" ),
                Time = src.Time
            };
        }

        protected override Return? MapToResult( IReturn? src )
        {
            if (src is null)
                return null;

            return new Return(
                Id: src.Id,
                Time: src.Time,
                Lease: new Lease(
                    Id: src.Id,
                    Time: src.Time,
                    ReturnDate: src.Lease.ReturnDate,
                    Borrower: new User(
                        src.Lease.Borrower.Id,
                        src.Lease.Borrower.FirstName,
                        src.Lease.Borrower.Surname ),
                    LeasedBook: new Book(
                        src.Lease.LeasedBook.Id,
                        new BookInfo(
                            src.Lease.LeasedBook.BookInfo.Id,
                            src.Lease.LeasedBook.BookInfo.Title,
                            src.Lease.LeasedBook.BookInfo.Author,
                            src.Lease.LeasedBook.BookInfo.DatePublished ) ) ) );
        }

        public override async Task UpdateAsync( IReturn item )
        {
            ReturnDto? ret = await dbSet.FindAsync(item.Id);
            if (ret is not null)
            {
                LeaseDto? lease = await dbContext.Leases.FindAsync(item.Lease.Id);
                if (lease is null)
                    throw new ArgumentException("Lease must be added before it can be assigned to a lease");

                ret.Lease = lease;

                ret.Time = item.Time;

                await SaveChanges();
            }
        }
    }
}
