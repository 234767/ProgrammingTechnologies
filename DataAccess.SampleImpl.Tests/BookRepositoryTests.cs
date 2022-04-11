using System;
using System.Linq;
using Xunit;
using DataAccess.API.Abstractions;
using DataAccess.API.DTO;
using FluentAssertions;

namespace DataAccess.SampleImpl.Tests;

public class BookRepositoryTests
{
    private IBookRepository _repository;

    public BookRepositoryTests()
    {
        _repository = TestingDataProvider.GetEmptyDataContext().Books;
    }
    
    [Fact]
    public void Create_ShouldIncreasesCount_WhenBookDoesNotYetExist()
    {
        _repository.GetAll().Count().Should().Be(0);
        _repository.Create(new Book("book_1_1", new BookInfo("book_1", "", "", null)));
        _repository.GetAll().Count().Should().Be(1);
    }

    [Fact]
    public void Create_ShouldNotAddBook_WhenBookWithTheSameIdAlreadyExists()
    {
        const string id = "book_1_1";
        IBookInfo info = new BookInfo("book_1", "Title", "Author", null);
        var originalBook = new Book(id, info);
        _repository.GetAll().Count().Should().Be(0);
        _repository.Create(originalBook);
        _repository.GetAll().Count().Should().Be(1);

        _repository.Create(new Book(id, info));
        _repository.GetAll().Count().Should().Be(1);
        _repository.Get(id).Should().NotBeNull().And.BeSameAs(originalBook);
    }

    [Fact]
    public void Create_ShouldAddBookInfo_WhenItDoesNotCurrentlyExist()
    {
        _repository.GetAllBookInfo().Count().Should().Be(0);
        _repository.Create(new Book("book_1_1", new BookInfo("book_1", "", "", null)));
        _repository.GetAllBookInfo().Count().Should().Be(1);
        _repository.GetAllBookInfo().First().Id.Should().Be("book_1");
    }

    [Fact]
    public void Create_ShouldNotAddBookInfo_WhenItAlreadyExists()
    {
        _repository.GetAllBookInfo().Count().Should().Be(0);
        IBookInfo info = new BookInfo("book_1", "", "", null);
        _repository.Create(new Book("book_1_1", info));
        _repository.Create(new Book("book_1_2", info));
        _repository.GetAllBookInfo().Count().Should().Be(1);
    }

    [Fact]
    public void Where_ShouldReturnAllBooks_ThatMatchTheExpression()
    {
        IBookInfo info1 = new BookInfo("book_1", "Title", "Author", null);
        IBookInfo info2 = new BookInfo("book_2", "Title2", "Author2", null);
        _repository.Create(new Book("book_1_1", info1));
        _repository.Create(new Book("book_1_2", info1));
        _repository.Create(new Book("book_2_1", info2));
        _repository.Create(new Book("book_2_2", info2));

        _repository.Where(book => book.BookInfo.Equals(info1))
                   .Count()
                   .Should()
                   .Be(2);

        _repository.Where(book => book.BookInfo.Title.Equals("Title2", StringComparison.InvariantCultureIgnoreCase))
                   .Count()
                   .Should()
                   .Be(2);

        _repository.Where(book => book.BookInfo.Author.Equals("J. D. Salinger"))
                   .Count()
                   .Should()
                   .Be(0);
    }

    [Fact]
    public void Update_ShouldChangeTheBookData_WhenBookWithSuchIdExists()
    {
        IBookInfo info = new BookInfo("book_1", "Title", "Author", null);
        const string id = "book_1_2";
        _repository.Create(new Book("book_1_1", info));
        _repository.Create(new Book("book_1_2", info));
        Book updatedBook = new Book(id, new BookInfo("book_2", "New Title", "New Author", null));
        _repository.Update(updatedBook);
        _repository.Get(id).Should().BeSameAs(updatedBook);
    }

    [Fact]
    public void Delete_ShouldRemoveBook_IfSuchExists()
    {
        IBookInfo info = new BookInfo("book_1", "Title", "Author", null);
        const string id = "book_1_1";
        _repository.Create(new Book(id, info));
        _repository.Create(new Book("book_1_2", info));
        _repository.Create(new Book("book_1_3", info));


        _repository.GetAll().Count().Should().Be(3);
        _repository.Delete(id);
        _repository.GetAll().Count().Should().Be(2);
        _repository.Get(id).Should().BeNull();
    }

    [Fact]
    public void Delete_ShouldRemoveBookInfo_WhenItIsTheLastSuchBook()
    {
        IBookInfo info = new BookInfo("book_1", "", "", null);
        _repository.Create(new Book("book_1_1", info));
        _repository.Create(new Book("book_1_2", info));
        _repository.Delete("book_1_1");
        _repository.GetAllBookInfo().Count().Should().Be(1);
        _repository.Delete("book_1_2");
        _repository.GetAllBookInfo().Count().Should().Be(0);
    }

    [Fact]
    public void UpdateBookInfo_ShouldChangeBookReferences_WhenModified()
    {
        const string id = "book_1";
        IBookInfo oldInfo = new BookInfo(id, "Title", "Author", null);
        _repository.Create(new Book("book_1_1", oldInfo));
        IBookInfo newInfo = new BookInfo(id, "New Title", "New Author", null);
        _repository.UpdateBookInfo(newInfo);
        _repository.Get("book_1_1")?.BookInfo.Should().NotBeNull().And.BeSameAs(newInfo);
    }

    [Fact]
    public void GetUserWhoLeased_ShouldReturnUser_WhoLeasedButDidNotReturnBook()
    {
        _repository = TestingDataProvider.GenerateHardCodedData().Books;
        _repository.GetUserWhoLeased(TestingDataProvider.Book2).Should().Be(TestingDataProvider.User2);
    }

    [Fact]
    public void GetUserWhoLeased_ShouldReturnNull_WhenBookIsReturned()
    {
        _repository = TestingDataProvider.GenerateHardCodedData().Books;
        _repository.GetUserWhoLeased(TestingDataProvider.Book1).Should().BeNull();
    }
}