using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess.API.Abstractions;
using DataAccess.API.DTO;
using DataAccess.Database.Dto;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Database;

internal class LibraryEventRepository : IEventRepository
{
    private readonly LibraryDataContext _context;
    private static readonly IMapper Mapper;

    static LibraryEventRepository()
    {
        Mapper = new MapperConfiguration(
            cfg =>
            {
                cfg.CreateMap<LeaseDto, ILease>();
                cfg.CreateMap<ReturnDto, IReturn>();
            } ).CreateMapper();
    }

    public LibraryEventRepository(LibraryDataContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(ILibraryEvent libraryEvent)
    {
        if ( await GetAsync( libraryEvent.Id ) is not null )
            throw new DuplicateNameException( $"Event with Id={libraryEvent.Id} already exists" );

        switch ( libraryEvent )
        {
            case ILease lease:
                await _context.Leases.AddAsync(Mapper.Map<LeaseDto>(lease));
                return;
            case IReturn ret:
                await _context.Returns.AddAsync(Mapper.Map<ReturnDto>(ret));
                return;
            default:
                throw new NotSupportedException( $"Type {libraryEvent.GetType()} has to implement either {nameof(ILease)} or {nameof(IReturn)}" );
        }
    }

    public async Task<ILibraryEvent?> GetAsync( string id )
    {
        return ( await _context.Leases.FindAsync( id ) as ILibraryEvent ) ?? ( await _context.Returns.FindAsync( id ) as ILibraryEvent );
    }

    public async Task<IEnumerable<ILibraryEvent>> WhereAsync(Expression<Func<ILibraryEvent, bool>> predicate)
    {
        var leases = _context.Leases.Where( predicate ).Cast<ILibraryEvent>();
        var returns = _context.Returns.Where( predicate ).Cast<ILibraryEvent>();

        return await leases.Union( returns ).ToListAsync();
    }

    public async Task UpdateAsync(ILibraryEvent item)
    {
        if (await GetAsync(item.Id) is null)
            return;

        switch (item)
        {
            case ILease lease:
                _context.Leases.Update(Mapper.Map<LeaseDto>(lease));
                return;
            case IReturn ret:
                _context.Returns.Update(Mapper.Map<ReturnDto>(ret));
                return;
            default:
                throw new NotSupportedException($"Type {item.GetType()} has to implement either {nameof(ILease)} or {nameof(IReturn)}");
        }
    }

    public async Task DeleteAsync(string id)
    {
        ILibraryEvent? item = await GetAsync( id );
        if (item is null)
            return;

        switch (item)
        {
            case ILease lease:
                _context.Leases.Remove( ( await _context.Leases.FindAsync( id ) )! );
                return;
            case IReturn ret:
                _context.Returns.Update( ( await _context.Returns.FindAsync( id ) )! );
                return;
            default:
                throw new NotSupportedException($"Type {item.GetType()} has to implement either {nameof(ILease)} or {nameof(IReturn)}");
        }
    }

    public async Task<IEnumerable<ILibraryEvent>> GetAllAsync() => await WhereAsync(e => true);

    public async Task<ILibraryEvent?> GetLatestEventForBookAsync(string bookId)
    {
        return ( await this.GetAllAsync() ).Where(e => GetBookFromEvent(e).Id == bookId).OrderByDescending( e => e.Time )
                                           .FirstOrDefault();
    }

    private static IBook GetBookFromEvent(ILibraryEvent e) => e switch{
        ILease l  => l.LeasedBook,
        IReturn r => r.Lease.LeasedBook,
        var _     => throw new NotSupportedException($"Type {e.GetType()} has to implement either {nameof(ILease)} or {nameof(IReturn)}")
    };
}