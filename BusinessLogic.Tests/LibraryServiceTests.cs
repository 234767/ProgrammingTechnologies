using System;
using DataAccess.API.Abstractions;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace BusinessLogic.Tests;

public class LibraryServiceTests
{
    private readonly IUserRepository _userRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IEventRepository _eventRepository;
    private readonly LibraryService _library;

    public LibraryServiceTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _bookRepository = Substitute.For<IBookRepository>();
        _eventRepository = Substitute.For<IEventRepository>();
        _library = new LibraryService(_userRepository, _bookRepository, _eventRepository);
    }

    [Fact]
    public void BorrowingBook_ReturnsTrue_WhenBookIsAvailable()
    {
        const string userId = "user_1";
        const string bookId = "book_1";
        _bookRepository.Get(bookId).Returns(new Book(bookId, new BookInfo("", "", "", null)));
        _userRepository.Get(userId).Returns(new User(userId, "", ""));
        _eventRepository.GetLatestEventForBook(bookId).ReturnsNull();
        _library.TryBorrow(userId, bookId).Should().BeTrue();
    }

    [Fact]
    public void BorrowingBook_ReturnsFalse_WhenBookIsLeased()
    {
        const string userId = "user_1";
        const string bookId = "book_1";
        var book = new Book(bookId, new BookInfo("", "", "", null));
        _bookRepository.Get(bookId).Returns(book);
        var user = new User(userId, "", "");
        _userRepository.Get(userId).Returns(user);
        _eventRepository.GetLatestEventForBook(bookId).Returns(new Lease("lease_1", DateTime.Now, book, user, TimeSpan.MaxValue));
        _library.TryBorrow(userId, bookId).Should().BeFalse();
    }
    
    [Fact]
    public void BorrowingBook_ReturnsFalse_WhenBookDoesNotExist()
    {
        const string userId = "user_1";
        const string bookId = "book_1";
        _bookRepository.Get(bookId).ReturnsNull();
        var user = new User(userId, "", "");
        _userRepository.Get(userId).Returns(user);
        _library.TryBorrow(userId, bookId).Should().BeFalse();
    }
    
    [Fact]
    public void BorrowingBook_ReturnsFalse_WhenUserDoesNotExist()
    {
        const string userId = "user_1";
        const string bookId = "book_1";
        var book = new Book(bookId, new BookInfo("", "", "", null));
        _bookRepository.Get(bookId).Returns(book);
        _userRepository.Get(userId).ReturnsNull();
        _library.TryBorrow(userId, bookId).Should().BeFalse();
    }
}