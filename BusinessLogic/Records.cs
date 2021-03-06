using System.Runtime.CompilerServices;
using DataAccess.API.DTO;

[assembly: InternalsVisibleTo("BusinessLogic.Tests")]


namespace BusinessLogic;

internal record BookInfo( string Id, string Title, string Author, DateOnly? PublicationDate ) : IBookInfo
{
    DateTime? IBookInfo.DatePublished => PublicationDate?.ToDateTime(new TimeOnly(0,0,0));
}

internal record User(string Id, string FirstName, string Surname) : IUser;

internal record Lease(string Id, DateTime Time, IBook LeasedBook, IUser Borrower, DateTime ReturnDate) : ILease;

internal record Return(string Id, ILease Lease, DateTime Time) : IReturn;

internal class Book : IBook
{
    public Book(string id, IBookInfo bookInfo)
    {
        Id = id;
        BookInfo = bookInfo;
    }

    public string Id { get; }
    public IBookInfo BookInfo { get; set; }
}