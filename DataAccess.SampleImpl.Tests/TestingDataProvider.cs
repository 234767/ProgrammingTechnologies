using System;
using DataAccess.API.Abstractions;
using DataAccess.API.DTO;

namespace DataAccess.SampleImpl.Tests;

public static class TestingDataProvider
{
    public static readonly IUser User1 = new User("user_1", "Steve", "Mason");
    public static readonly IBook Book1 = new Book("book_1_1", new BookInfo("book_1", "Les Miserables", "Victor Hugo", null));
    public static readonly IUser User2 = new User("user_2", "Richard", "Rider");

    public static readonly IBook Book2 = new Book("book_2_1",
                                                  new BookInfo("book_2",
                                                               "The Catcher in the Rye",
                                                               "J. D. Salinger",
                                                               new DateOnly(1951, 06, 16)));

    public static readonly ILease Lease1 =
        new Lease("lease_1", DateTime.Parse("02/27/2022 12:00:00"), Book1, User1, TimeSpan.MaxValue);
    public static readonly IReturn Return1 =
        new Return("return_1",Lease1, DateTime.Parse("03/03/2022 12:00:00"));
    public static readonly ILease Lease2 =
        new Lease("lease_2", DateTime.Parse("04/14/2022 12:00:00"), Book2, User2, TimeSpan.MaxValue);
    
    public static ILibraryDataContext GenerateHardCodedData()
    {
        ILibraryDataContext context = new LibraryDataContextFactory().CreateDataContext();
        context.Users.CreateAsync(User1);
        context.Users.CreateAsync(User2);
        context.Books.CreateAsync(Book1);
        context.Books.CreateAsync(Book2);
        context.Events.CreateAsync(Lease1);
        context.Events.CreateAsync(Return1);
        context.Events.CreateAsync(Lease2);
        return context;
    }

    public static ILibraryDataContext GenerateRandomData()
    {
        ILibraryDataContext context = new LibraryDataContextFactory().CreateDataContext();
        User user1 = new User(Guid.NewGuid().ToString(),"","");
        User user2 = new User(Guid.NewGuid().ToString(), "", "");
        IBook book = new Random().Next(2) == 1 ? Book1 : Book2;
        context.Users.CreateAsync(user1);
        context.Users.CreateAsync(user2);
        context.Books.CreateAsync(book);
        return context;
    }

    public static ILibraryDataContext GetEmptyDataContext()
    {
        return new LibraryDataContextFactory().CreateDataContext();
    }
}