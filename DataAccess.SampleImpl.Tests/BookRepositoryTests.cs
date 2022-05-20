using System;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.API.Abstractions;
using DataAccess.API.DTO;
using FluentAssertions;
using Xunit;

namespace DataAccess.Database.Tests;

public class BookRepositoryTests
{
    private IBookRepository _repository;

    public BookRepositoryTests()
    {
        _repository = TestingDataProvider.GetEmptyDataContext().Books;
    }

    [Fact]
    public async Task Create_ShouldIncreasesCount_WhenBookDoesNotYetExist()
    {
        ( await _repository.GetAllAsync() ).Count().Should().Be(0);
        await _repository.CreateAsync(new Book("book_1_1", new BookInfo("book_1", "", "", null)));
        ( await _repository.GetAllAsync() ).Count().Should().Be(1);
    }

    [Fact]
    public async Task Create_ShouldNotAddBook_WhenBookWithTheSameIdAlreadyExists()
    {
        const string id = "book_1_1";
        IBookInfo info = new BookInfo("book_1", "Title", "Author", null);
        var originalBook = new Book(id, info);
        ( await _repository.GetAllAsync() ).Count().Should().Be(0);
        await _repository.CreateAsync(originalBook);
        ( await _repository.GetAllAsync() ).Count().Should().Be(1);

        await _repository.CreateAsync(new Book(id, info));
        ( await _repository.GetAllAsync() ).Count().Should().Be(1);
        ( await _repository.GetAsync(id) ).Should().NotBeNull().And.BeSameAs(originalBook);
    }

    [Fact]
    public async Task Create_ShouldAddBookInfo_WhenItDoesNotCurrentlyExist()
    {
        ( await _repository.GetAllBookInfoAsync() ).Count().Should().Be(0);
        await _repository.CreateAsync(new Book("book_1_1", new BookInfo("book_1", "", "", null)));
        ( await _repository.GetAllBookInfoAsync() ).Count().Should().Be(1);
        ( await _repository.GetAllBookInfoAsync() ).First().Id.Should().Be("book_1");
    }

    [Fact]
    public async Task Create_ShouldNotAddBookInfo_WhenItAlreadyExists()
    {
        ( await _repository.GetAllBookInfoAsync() ).Count().Should().Be(0);
        IBookInfo info = new BookInfo("book_1", "", "", null);
        await _repository.CreateAsync(new Book("book_1_1", info));
        await _repository.CreateAsync(new Book("book_1_2", info));
        ( await _repository.GetAllBookInfoAsync() ).Count().Should().Be(1);
    }

    [Fact]
    public async Task Where_ShouldReturnAllBooks_ThatMatchTheExpression()
    {
        IBookInfo info1 = new BookInfo("book_1", "Title", "Author", null);
        IBookInfo info2 = new BookInfo("book_2", "Title2", "Author2", null);
        await _repository.CreateAsync(new Book("book_1_1", info1));
        await _repository.CreateAsync(new Book("book_1_2", info1));
        await _repository.CreateAsync(new Book("book_2_1", info2));
        await _repository.CreateAsync(new Book("book_2_2", info2));

        ( await _repository.WhereAsync(book => book.BookInfo.Equals(info1)) )
            .Count()
            .Should()
            .Be(2);

        ( await _repository.WhereAsync(book => book.BookInfo.Title.Equals("Title2",
                                                                          StringComparison.InvariantCultureIgnoreCase)) )
            .Count()
            .Should()
            .Be(2);

        ( await _repository.WhereAsync(book => book.BookInfo.Author.Equals("J. D. Salinger")) )
            .Count()
            .Should()
            .Be(0);
    }

    [Fact]
    public async Task Update_ShouldChangeTheBookData_WhenBookWithSuchIdExists()
    {
        IBookInfo info = new BookInfo("book_1", "Title", "Author", null);
        const string id = "book_1_2";
        await _repository.CreateAsync(new Book("book_1_1", info));
        await _repository.CreateAsync(new Book("book_1_2", info));
        Book updatedBook = new Book(id, new BookInfo("book_2", "New Title", "New Author", null));
        await _repository.UpdateAsync(updatedBook);
        ( await _repository.GetAsync(id) ).Should().BeSameAs(updatedBook);
    }

    [Fact]
    public async Task Delete_ShouldRemoveBook_IfSuchExists()
    {
        IBookInfo info = new BookInfo("book_1", "Title", "Author", null);
        const string id = "book_1_1";
        await _repository.CreateAsync(new Book(id, info));
        await _repository.CreateAsync(new Book("book_1_2", info));
        await _repository.CreateAsync(new Book("book_1_3", info));


        ( await _repository.GetAllAsync() ).Count().Should().Be(3);
        await _repository.DeleteAsync(id);
        ( await _repository.GetAllAsync() ).Count().Should().Be(2);
        ( await _repository.GetAsync(id) ).Should().BeNull();
    }

    [Fact]
    public async Task Delete_ShouldRemoveBookInfo_WhenItIsTheLastSuchBook()
    {
        IBookInfo info = new BookInfo("book_1", "", "", null);
        await _repository.CreateAsync(new Book("book_1_1", info));
        await _repository.CreateAsync(new Book("book_1_2", info));
        await _repository.DeleteAsync("book_1_1");
        ( await _repository.GetAllBookInfoAsync() ).Count().Should().Be(1);
        await _repository.DeleteAsync("book_1_2");
        ( await _repository.GetAllBookInfoAsync() ).Count().Should().Be(0);
    }

    [Fact]
    public async Task UpdateBookInfo_ShouldChangeBookReferences_WhenModified()
    {
        const string id = "book_1";
        IBookInfo oldInfo = new BookInfo(id, "Title", "Author", null);
        await _repository.CreateAsync(new Book("book_1_1", oldInfo));
        IBookInfo newInfo = new BookInfo(id, "New Title", "New Author", null);
        await _repository.UpdateBookInfoAsync(newInfo);
        ( await _repository.GetAsync("book_1_1") )?.BookInfo.Should().NotBeNull().And.BeSameAs(newInfo);
    }

    [Fact]
    public async Task GetUserWhoLeased_ShouldReturnUser_WhoLeasedButDidNotReturnBook()
    {
        _repository = TestingDataProvider.GenerateHardCodedData().Books;
        ( await _repository.GetUserWhoLeased(TestingDataProvider.Book2) ).Should().Be(TestingDataProvider.User2);
    }

    [Fact]
    public async Task GetUserWhoLeased_ShouldReturnNull_WhenBookIsReturned()
    {
        _repository = TestingDataProvider.GenerateHardCodedData().Books;
        ( await _repository.GetUserWhoLeased(TestingDataProvider.Book1) ).Should().BeNull();
    }
}