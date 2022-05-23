using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.API.Abstractions;
using DataAccess.API.DTO;
using DataAccess.Database.Dto;
using DataAccess.Database.Records;
using Microsoft.EntityFrameworkCore;

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

    protected override IQueryable<BookDto> LoadRelations( IQueryable<BookDto> data )
    {
        return data.Include( b => b.BookInfo );
    }

    protected override Book? MapToResult( IBook? src )
    {
        if ( src is null )
            return null;

        return new Book( 
            src.Id, 
            new BookInfo(
                src.BookInfo?.Id!,
                src.BookInfo?.Title!,
                src.BookInfo?.Author!,
                src.BookInfo?.DatePublished
            ) 
        );
    }

    public override async Task<IBook?> GetAsync( string id )
    {
        return MapToResult( await LoadRelations( dbSet ).SingleOrDefaultAsync( b => b.Id == id ) );
    }

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

    public async Task<IEnumerable<IBookInfo?>> FindBookInfoAsync( string? author, string? title )
    {
        if (author is not null && title is not null)
            return await dbContext.BookInfos.Where( info => info.Author == author && info.Title == title ).ToListAsync();
        else if (author is not null && title is null)
            return await dbContext.BookInfos.Where(info => info.Author == author).ToListAsync();
        else if (author is null && title is not null)
            return await dbContext.BookInfos.Where(info => info.Title == title).ToListAsync();
        else 
            return Enumerable.Empty<IBookInfo>();
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

    public override async Task<IEnumerable<IBook>> GetAllAsync()
    {
        var results = await dbSet.Include( b => b.BookInfo ).ToListAsync();
        return (IEnumerable<IBook>)results.Select(MapToResult).ToList();
    }
}