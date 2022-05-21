using System.Diagnostics.CodeAnalysis;
using DataAccess.API.Abstractions;
using DataAccess.Database.Dto;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Database;

internal class LibraryDataContext : DbContext, ILibraryDataContext
{
    private const string DefaultConnectionString =
        @"Data Source=ASUS-KUBA;Initial Catalog=library;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
    
    private readonly string _connectionString;

    public LibraryDataContext([MaybeNull] string connectionString = default!)
    {
        _connectionString = connectionString ?? DefaultConnectionString;
    }

    internal DbSet<BookInfoDto> BookInfos { get; } = null!;
    internal DbSet<BookDto> Books { get; } = null!;
    internal DbSet<LeaseDto> Leases { get; } = null!;
    internal DbSet<ReturnDto> Returns { get; } = null!;
    internal DbSet<UserDto> Users { get; } = null!;

    protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
    {
        optionsBuilder.UseSqlServer(_connectionString );
    }

    protected override void OnModelCreating( ModelBuilder modelBuilder )
    {
        modelBuilder.Entity<BookDto>().HasOne( book => (BookInfoDto) book.BookInfo );
        modelBuilder.Entity<LeaseDto>().HasOne( lease => (UserDto)lease.Borrower );
        modelBuilder.Entity<LeaseDto>().HasOne( lease => (BookDto)lease.LeasedBook );
    }
}