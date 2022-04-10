using System;
using DataAccess.API.Abstractions;
using DataAccess.API.DTO;

namespace DataAccess.SampleImpl.Tests;

public record BookInfo(string Id, string Title, string Author, DateOnly? DatePublished) : IBookInfo;

public record Book(string Id, IBookInfo BookInfo) : IBook;

public record User(string Id, string FirstName, string Surname) : IUser;

public record Lease(string Id, DateTime Time, IBook LeasedBook, IUser Borrower, TimeSpan Duration) : ILease;

public record Return(string Id, ILease Lease, DateTime Time) : IReturn;