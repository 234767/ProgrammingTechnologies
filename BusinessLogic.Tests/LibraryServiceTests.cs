using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using DataAccess.API.Abstractions;
using DataAccess.API.DTO;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace BusinessLogic.Tests;

public class LibraryServiceTests
{
    private readonly IUserRepository _userRepository;
    private readonly IBookRepository _bookRepository;
    private readonly ILeaseRepository _leaseRepository;
    private readonly IReturnRepository _returnRepository;
    private readonly LibraryService _library;

    public LibraryServiceTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _bookRepository = Substitute.For<IBookRepository>();
        _leaseRepository = Substitute.For<ILeaseRepository>();
        _returnRepository = Substitute.For<IReturnRepository>();
        _library = new LibraryService(_userRepository, _bookRepository, _leaseRepository, _returnRepository);
    }

    [Fact]
    public async Task BorrowingBook_ReturnsTrue_WhenBookWasNotLeased()
    {
        const string userId = "user_1";
        const string bookId = "book_1";
        _bookRepository.GetAsync(bookId).Returns(new Book(bookId, new BookInfo("", "", "", null)));
        _userRepository.GetAsync(userId).Returns(new User(userId, "", ""));
        _leaseRepository.WhereAsync(Arg.Any<Expression<Func<ILease, bool>>>()).Returns(Task.FromResult(Enumerable.Empty<ILease>()));
        ( await _library.TryBorrow(userId, bookId) ).Should().BeTrue();
    }

    [Fact]
    public async Task BorrowingBook_ReturnsFalse_WhenBookIsLeased()
    {
        const string userId = "user_1";
        const string bookId = "book_1";
        var book = new Book(bookId, new BookInfo("", "", "", null));
        _bookRepository.GetAsync(bookId).Returns(book);
        var user = new User(userId, "", "");
        _userRepository.GetAsync(userId).Returns(user);
        _leaseRepository.WhereAsync(Arg.Any<Expression<Func<ILease, bool>>>()).Returns(new []
        {
            new Lease(
                "lease_1",
                DateTime.Now,
                book,
                user,
                new DateTime())
        });
        ( await _library.TryBorrow(userId, bookId) ).Should().BeFalse();
    }

    [Fact]
    public async Task BorrowingBook_ReturnsTrue_WhenBookWasLeasedAndReturned()
    {
        const string userId = "user_1";
        const string bookId = "book_1";
        var book = new Book(bookId, new BookInfo("", "", "", null));
        _bookRepository.GetAsync(bookId).Returns(book);
        var user = new User(userId, "", "");
        _userRepository.GetAsync(userId).Returns(user);
        var lease = new Lease(
            "lease_1",
            DateTime.Now,
            book,
            user,
            new DateTime());
        _leaseRepository.WhereAsync(Arg.Any<Expression<Func<ILease, bool>>>()).Returns(new[] { lease });
        _returnRepository.WhereAsync(Arg.Any<Expression<Func<IReturn, bool>>>()).Returns(new[]
        {
            new Return(
                "return_1",
                lease,
                new DateTime())
        });
        (await _library.TryBorrow(userId, bookId)).Should().BeTrue();
    }

    [Fact]
    public async Task BorrowingBook_ReturnsFalse_WhenBookDoesNotExist()
    {
        const string userId = "user_1";
        const string bookId = "book_1";
        _bookRepository.GetAsync(bookId).ReturnsNull();
        var user = new User(userId, "", "");
        _userRepository.GetAsync(userId).Returns(user);
        ( await _library.TryBorrow(userId, bookId) ).Should().BeFalse();
    }
    
    [Fact]
    public async Task BorrowingBook_ReturnsFalse_WhenUserDoesNotExist()
    {
        const string userId = "user_1";
        const string bookId = "book_1";
        var book = new Book(bookId, new BookInfo("", "", "", null));
        _bookRepository.GetAsync(bookId).Returns(book);
        _userRepository.GetAsync(userId).ReturnsNull();
        ( await _library.TryBorrow(userId, bookId) ).Should().BeFalse();
    }
}