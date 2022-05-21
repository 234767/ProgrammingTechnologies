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

internal class BookRepository : IBookRepository
{
    private readonly LibraryDataContext _context;
    private static readonly IMapper Mapper;
    
    static BookRepository()
    {
        Mapper = new MapperConfiguration(
            cfg =>
            {
                cfg.CreateMap<IBook, BookDto>();
                cfg.CreateMap<IBookInfo, BookInfoDto>();
            } ).CreateMapper();
    }

    public BookRepository(LibraryDataContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(IBook item)
    {
        if ( await _context.Books.AnyAsync(b => b.Id == item.Id) )
            return;
        BookDto dto = Mapper.Map<IBook, BookDto>( item );
        await _context.Books.AddAsync( dto );
        if (!await _context.BookInfos.AnyAsync(info => info.Id == item.BookInfo.Id))
        {
            await _context.BookInfos.AddAsync( Mapper.Map<BookInfoDto>(item.BookInfo));
        }
    }

    public async Task<IBook?> GetAsync(string id)
    {
        return await _context.Books.FindAsync((BookDto b) => b.Id == id);
    }

    public async Task<IEnumerable<IBook>> WhereAsync(Expression<Func<IBook, bool>> predicate)
    {
        return await _context.Books.Where(predicate).ToListAsync();
    }

    public async Task UpdateAsync(IBook item)
    {
        await Task.Run(
            () =>
            {
                _context.Books.Update( Mapper.Map<BookDto>( item ) );
            } );
    }

    public async Task UpdateBookInfoAsync(IBookInfo newInfo)
    {
        IBookInfo? oldInfo = await _context.FindAsync<BookInfoDto>( ( BookInfoDto info ) => info.Id == newInfo.Id);
        if ( oldInfo is null )
        {
            throw new KeyNotFoundException( $"Book info with Id {newInfo.Id} not found" );
        }

        BookInfoDto newInfoDto = Mapper.Map<BookInfoDto>( newInfo );
        _context.BookInfos.Update(newInfoDto);

        foreach (BookDto book in _context.Books.Where(b => b.BookInfo.Equals(oldInfo)))
        {
            book.BookInfo = newInfoDto;
        }
    }

    public async Task DeleteAsync(string id)
    {
        BookDto bookToRemove = await _context.Books.FirstAsync(b => b.Id == id);
        _context.Books.Remove(bookToRemove);

        if (_context.Books.All(book => book.BookInfo.Id != bookToRemove.BookInfo.Id))
        {
            _context.BookInfos.Remove(await _context.BookInfos.SingleAsync(info => info.Id == bookToRemove.BookInfo.Id));
        }
    }

    public async Task<IEnumerable<IBook>> GetAllAsync() => await _context.Books.ToListAsync();

    public async Task<IEnumerable<IBookInfo>> GetAllBookInfoAsync() => await _context.BookInfos.ToListAsync();

    public async Task<IUser?> GetUserWhoLeased(IBook book)
    {
        return ( await _context.Leases.FirstOrDefaultAsync(
            l => l.LeasedBook == book && 
                 _context.Returns.All( r => r.Lease != l ) ) )?.Borrower;
    }
}