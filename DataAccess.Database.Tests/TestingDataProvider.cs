using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.API.Abstractions;
using DataAccess.API.DTO;
using DataAccess.Database.Repositories;
using FluentAssertions;
using Xunit;

namespace DataAccess.Database.Tests;

public static class TestingDataProvider
{
    
    
    public static readonly IUser User1 = new User("user_1", "Steve", "Mason");
    public static readonly IBook Book1 = new Book("book_1_1", new BookInfo("book_1", "Les Miserables", "Victor Hugo", null));
    public static readonly IUser User2 = new User("user_2", "Richard", "Rider");

    public static readonly IBook Book2 = new Book("book_2_1",
        new BookInfo("book_2",
            "The Catcher in the Rye",
            "J. D. Salinger",
            new DateOnly(1951, 06, 16).ToDateTime(new TimeOnly())));

    public static readonly ILease Lease1 =
        new Lease("lease_1", new DateTime(2022,02,27), Book1, User1, new DateTime(2020,01,01));

    public static readonly IReturn Return1 =
        new Return("return_1", Lease1, new DateTime(2022,02,03));

    public static readonly ILease Lease2 =
        new Lease("lease_2", new DateTime(2022, 04,14), Book2, User2, new DateTime(2020, 01, 01));

    public static async Task<(IBookRepository Books, IUserRepository Users, ILeaseRepository Events, IReturnRepository Returns)> GenerateHardCodedData()
    {
        var (books, users, leases, returns) = await GetEmptyDataContext();

        await users.CreateAsync(User1);
        await users.CreateAsync(User2);
        await books.CreateAsync(Book1);
        await books.CreateAsync(Book2);
        await leases.CreateAsync(Lease1);
        await returns.CreateAsync(Return1);
        await leases.CreateAsync(Lease2);
        return ( books, users, leases, returns );
    }

    public static async Task<(IBookRepository Books, IUserRepository Users, ILeaseRepository Events, IReturnRepository Returns)> GenerateRandomData()
    {
        User user1 = new User(Guid.NewGuid().ToString(), "", "");
        User user2 = new User(Guid.NewGuid().ToString(), "", "");
        IBook book1 = new Random().Next(2) == 1 ? Book1 : Book2;

        var (books, users, leases, returns) = await GetEmptyDataContext();

        await users.CreateAsync(user1);
        await users.CreateAsync(user2);
        await books.CreateAsync(book1);
        return ( books, users, leases, returns );
    }

    public static async Task<(IBookRepository Books, IUserRepository Users, ILeaseRepository Leases, IReturnRepository Returns)> GetEmptyDataContext()
    {
        const string dbRelativePath = @"..\..\..\TestData.mdf";
        string dbAbsolutePath = Path.Combine( Environment.CurrentDirectory, dbRelativePath );
        new FileInfo( dbAbsolutePath ).Exists.Should().BeTrue($"File {dbAbsolutePath} should exist for tests");

        string connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={dbAbsolutePath};Integrated Security=True";

        DatabaseDataLayerFactory data = new (connectionString);
        IUserRepository users = data.CreateUserRepository();
        foreach (IUser user in await users.GetAllAsync())
        {
            await users.DeleteAsync(user.Id);
        }

        IBookRepository books = data.CreateBookRepository();
        var test = await books.GetAsync( "B1.1" );
        foreach (IBook book in await books.GetAllAsync())
        {
            await books.DeleteAsync(book.Id);
        }

        ILeaseRepository leases = data.CreateLeaseRepository();
        foreach (ILease libraryEvent in await leases.GetAllAsync())
        {
            await leases.DeleteAsync(libraryEvent.Id);
        }

        IReturnRepository returns = data.CreateReturnRepository();
        foreach ( IReturn @return in await returns.GetAllAsync() )
        {
            await returns.DeleteAsync(@return.Id);
        }

        (await users.GetAllAsync()).Count().Should().Be(0);
        (await books.GetAllAsync()).Count().Should().Be(0);
        (await leases.GetAllAsync()).Count().Should().Be(0);
        (await returns.GetAllAsync()).Count().Should().Be(0);

        return ( books, users, leases, returns );
    }
}