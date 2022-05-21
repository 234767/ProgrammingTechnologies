using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess.API.Abstractions;
using DataAccess.API.DTO;
using DataAccess.Database.Dto;
using DataAccess.Database.Records;

namespace DataAccess.Database.Repositories;

internal class BookRepository : RepositoryBase<IBook, BookDto, Book>, IBookRepository
{
    public BookRepository(LibraryDataContext context) : base(context.Books, context) { }

    protected override BookDto? MapToDto( IBook? src )
    {
        if ( src is null )
            return null;

        return new BookDto
        {
            Id = src.Id,
            BookInfo = dbContext.BookInfos.Find(src.BookInfo.Id) ?? new BookInfoDto
            {
                Id = src.BookInfo.Id,
                Title = src.BookInfo.Title,
                Author = src.BookInfo.Author,
                DatePublished = src.BookInfo.DatePublished
            }
        };
    }

    protected override Book? MapToResult( IBook? src )
    {
        if ( src is null )
            return null;

        return new Book( 
            src.Id, 
            new BookInfo(
                src.BookInfo.Id,
                src.BookInfo.Title,
                src.BookInfo.Author,
                src.BookInfo.DatePublished
            ) 
        );
    }

    #if false

    //todo: delete
    public override async Task<IBook?> GetAsync(string id)
    {
        var dto = new BookDto
        {
            Id = "id",
            BookInfo = new BookInfoDto { Author = "auth", DatePublished = null, Id = "info id", Title = "title" }
        };
        var info = Mapper.Map<BookInfo>(
            new BookInfoDto { Author = "auth", DatePublished = null, Id = "info id", Title = "title" } );
        var rec = Mapper.Map<IBook, Book>(dto);
        return await Task.FromResult(rec);
    }

    #endif


    public override async Task DeleteAsync( string id )
    {
        await base.DeleteAsync( id );
        await DeleteUnusedInfos();
    }

    public override async Task UpdateAsync( IBook item )
    {
        BookDto? book = await dbSet.FindAsync( item.Id );
        if ( book is not null )
        {
            book.BookInfo = (await dbContext.BookInfos.FindAsync(item.BookInfo.Id)) ?? new BookInfoDto()
            {
                Id = item.BookInfo.Id,
                Title = item.BookInfo.Title,
                Author = item.BookInfo.Author,
                DatePublished = item.BookInfo.DatePublished
            };
            await SaveChanges();
            await DeleteUnusedInfos();
        }
    }

    private async Task DeleteUnusedInfos()
    {
        var unusedInfos = from info in dbContext.BookInfos
                          where dbContext.Books.All( b => b.BookInfo.Id != info.Id )
                          select info;
        foreach ( BookInfoDto info in unusedInfos )
        {
            dbContext.BookInfos.Remove( info );
        }

        await SaveChanges();
    }
}