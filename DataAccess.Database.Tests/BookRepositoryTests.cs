using System;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.API.Abstractions;
using DataAccess.API.DTO;
using FluentAssertions;
using Xunit;

namespace DataAccess.Database.Tests;

[Collection("DatabaseTests")]
public class BookRepositoryTests
{
    private IBookRepository _repository;

    public BookRepositoryTests()
    {
        _repository = TestingDataProvider.GetEmptyDataContext().Result.Books;
    }

    [Fact]
    public async Task Create_ShouldIncreasesCount_WhenBookDoesNotYetExist()
    {
        _repository = TestingDataProvider.GetEmptyDataContext().Result.Books;
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

        Func<Task> invalidAddition = () => _repository.CreateAsync(new Book(id, info));
        await invalidAddition.Should().ThrowAsync<InvalidOperationException>();
        ( await _repository.GetAllAsync() ).Count().Should().Be(1);
        var result = await _repository.GetAsync( id );
        result.Should().NotBeNull().And.BeEquivalentTo(originalBook);
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

        ( await _repository.WhereAsync(book => book.BookInfo.DatePublished == null) )
            .Count()
            .Should()
            .Be(4);

        ( await _repository.WhereAsync(book => book.BookInfo.Title.Equals("Title2", StringComparison.InvariantCultureIgnoreCase)) )
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
        ( await _repository.GetAsync(id) ).Should().BeEquivalentTo(updatedBook);
    }

    [Fact]
    public async Task Delete_ShouldRemoveBook_IfSuchExists()
    {
        var currentContent = await _repository.GetAllAsync();
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
}