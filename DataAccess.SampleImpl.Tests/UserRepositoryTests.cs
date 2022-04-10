using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess.API.Abstractions;
using DataAccess.API.DTO;
using FluentAssertions;
using Xunit;

namespace DataAccess.SampleImpl.Tests;

public class UserRepositoryTests
{
    private IUserRepository _repository;

    public UserRepositoryTests()
    {
        _repository = TestingDataProvider.GetEmptyDataContext().Users;
    }

    [Fact]
    public void Create_ShouldIncreasesCount_WhenUserDoesNotYetExist()
    {
        _repository.GetAll().Count().Should().Be(0);
        _repository.Create(new User("user_1", "A", "B"));
        _repository.GetAll().Count().Should().Be(1);
    }

    [Fact]
    public void Create_ShouldNotAddUser_WhenUserWithTheSameIdAlreadyExists()
    {
        const string id = "user_1";
        var originalUser = new User(id, "A", "B");
        _repository.GetAll().Count().Should().Be(0);
        _repository.Create(originalUser);
        _repository.GetAll().Count().Should().Be(1);

        _repository.Create(new User(id, "C", "D"));
        _repository.GetAll().Count().Should().Be(1);
        _repository.Get(id).Should().NotBeNull().And.BeSameAs(originalUser);
    }

    [Fact]
    public void Where_ShouldReturnAllUsers_ThatMatchTheExpression()
    {
        _repository.Create(new User("user_1", "ABC", "DEF"));
        _repository.Create(new User("user_2", "abc", "def"));
        _repository.Create(new User("user_3", "ASD", "zxc"));

        _repository.Where(user => user.FirstName.Equals("abc", StringComparison.InvariantCultureIgnoreCase))
                   .Count()
                   .Should()
                   .Be(2);

        _repository.Where(user => user.FirstName.Contains('A'))
                   .Count()
                   .Should()
                   .Be(2);

        _repository.Where(user => user.FirstName.Equals("DSA"))
                   .Count()
                   .Should()
                   .Be(0);
    }

    [Fact]
    public void Update_ShouldChangeTheUserData_WhenUserWithSuchIdExists()
    {
        const string id = "user_2";
        _repository.Create(new User("user_1", "abc", "def"));
        _repository.Create(new User(id, "ABC", "DEF"));
        User updatedUser = new User(id, "QWE", "RTY");
        _repository.Update(updatedUser);
        _repository.Get(id).Should().BeSameAs(updatedUser);
    }

    [Fact]
    public void Delete_ShouldRemoveUser_IfSuchExists()
    {
        const string id = "user_1";
        _repository.Create(new User(id,"abd","def"));
        _repository.Create(new User("user_2","aaa","bbb"));
        _repository.Create(new User("user_3","ccc","ddd"));

        _repository.GetAll().Count().Should().Be(3);
        _repository.Delete(id);
        _repository.GetAll().Count().Should().Be(2);
        _repository.Get(id).Should().BeNull();
    }

    [Fact]
    public void GetBooksLeasedBy_ShouldReturnBooks_ThatWereNotReturned()
    {
        _repository = TestingDataProvider.GenerateHardCodedData().Users;
        var leasedBooks = _repository.GetBooksLeasedBy(TestingDataProvider.User2).ToList();
        leasedBooks.Count.Should().Be(1);
        leasedBooks.First().Should().Be(TestingDataProvider.Book2);
    }

    [Fact]
    public void GetBooksLeasedBy_ShoudldNotReturnBooks_ThatWereReturned()
    {
        _repository = TestingDataProvider.GenerateHardCodedData().Users;
        var leasedBooks = _repository.GetBooksLeasedBy(TestingDataProvider.User1).ToList();
        leasedBooks.Count.Should().Be(0);
    }
}