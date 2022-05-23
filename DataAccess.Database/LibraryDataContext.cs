using System.Diagnostics.CodeAnalysis;
using DataAccess.API.Abstractions;
using DataAccess.Database.Dto;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Database;

internal class LibraryDataContext : DbContext
{
    private const string DefaultConnectionString =
        @"Data Source=ASUS-KUBA;Initial Catalog=library;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

    private readonly string _connectionString;

    public LibraryDataContext([MaybeNull] string connectionString = default!)
    {
        _connectionString = connectionString ?? DefaultConnectionString;
    }

    public DbSet<BookInfoDto> BookInfos { get; set; } = null!;
    public DbSet<BookDto> Books { get; set; } = null!;
    public DbSet<LeaseDto> Leases { get; set; } = null!;
    public DbSet<ReturnDto> Returns { get; set; } = null!;
    public DbSet<UserDto> Users { get; set; } = null!;

    protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
    {
        optionsBuilder.UseSqlServer(_connectionString );
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<BookDto>().HasOne(book => (BookInfoDto)book.BookInfo);
        modelBuilder.Entity<LeaseDto>().HasOne(lease => (UserDto)lease.Borrower);
        modelBuilder.Entity<LeaseDto>().HasOne(lease => (BookDto)lease.LeasedBook);
        modelBuilder.Entity<ReturnDto>().HasOne(ret => (LeaseDto)ret.Lease);
    }
}