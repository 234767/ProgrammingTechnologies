using System;
using DataAccess.API.DTO;

namespace DataAccess.Database.Tests;

public record BookInfo(string Id, string Title, string Author, DateTime? DatePublished) : IBookInfo;

public record User(string Id, string FirstName, string Surname) : IUser;

public record Lease(string Id, DateTime Time, IBook LeasedBook, IUser Borrower, TimeSpan Duration) : ILease;

public record Return(string Id, ILease Lease, DateTime Time) : IReturn;

public class Book : IBook
{
    public Book(string id, IBookInfo bookInfo)
    {
        Id = id;
        BookInfo = bookInfo;
    }
    public string Id { get; }
    public IBookInfo BookInfo { get; set; }
}