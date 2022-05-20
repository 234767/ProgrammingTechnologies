using System;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.API.Abstractions;
using FluentAssertions;
using Xunit;

namespace DataAccess.Database.Tests;

public class UserRepositoryTests
{
    private IUserRepository _repository;

    public UserRepositoryTests()
    {
        _repository = TestingDataProvider.GetEmptyDataContext().Users;
    }

    [Fact]
    public async Task Create_ShouldIncreasesCount_WhenUserDoesNotYetExist()
    {
        (await _repository.GetAllAsync()).Count().Should().Be(0);
        await _repository.CreateAsync(new User("user_1", "A", "B"));
        (await _repository.GetAllAsync()).Count().Should().Be(1);
    }

    [Fact]
    public async Task Create_ShouldNotAddUser_WhenUserWithTheSameIdAlreadyExists()
    {
        const string id = "user_1";
        var originalUser = new User(id, "A", "B");
        (await _repository.GetAllAsync()).Count().Should().Be(0);
        await _repository.CreateAsync(originalUser);
        (await _repository.GetAllAsync()).Count().Should().Be(1);

        await _repository.CreateAsync(new User(id, "C", "D"));
        (await _repository.GetAllAsync()).Count().Should().Be(1);
        (await _repository.GetAsync(id)).Should().NotBeNull().And.BeSameAs(originalUser);
    }

    [Fact]
    public async Task Where_ShouldReturnAllUsers_ThatMatchTheExpression()
    {
        await _repository.CreateAsync(new User("user_1", "ABC", "DEF"));
        await _repository.CreateAsync(new User("user_2", "abc", "def"));
        await _repository.CreateAsync(new User("user_3", "ASD", "zxc"));

        (await _repository.WhereAsync(user => user.FirstName.Equals("abc", StringComparison.InvariantCultureIgnoreCase)))
                   .Count()
                   .Should()
                   .Be(2);

        (await _repository.WhereAsync(user => user.FirstName.Contains('A')))
                   .Count()
                   .Should()
                   .Be(2);

        (await _repository.WhereAsync(user => user.FirstName.Equals("DSA")))
                   .Count()
                   .Should()
                   .Be(0);
    }

    [Fact]
    public async Task Update_ShouldChangeTheUserData_WhenUserWithSuchIdExists()
    {
        const string id = "user_2";
        await _repository.CreateAsync(new User("user_1", "abc", "def"));
        await _repository.CreateAsync(new User(id, "ABC", "DEF"));
        User updatedUser = new User(id, "QWE", "RTY");
        await _repository.UpdateAsync(updatedUser);
        (await _repository.GetAsync(id)).Should().BeSameAs(updatedUser);
    }

    [Fact]
    public async Task Delete_ShouldRemoveUser_IfSuchExists()
    {
        const string id = "user_1";
        await _repository.CreateAsync(new User(id,"abd","def"));
        await _repository.CreateAsync(new User("user_2","aaa","bbb"));
        await _repository.CreateAsync(new User("user_3","ccc","ddd"));

        (await _repository.GetAllAsync()).Count().Should().Be(3);
        await _repository.DeleteAsync(id);
        (await _repository.GetAllAsync()).Count().Should().Be(2);
        (await _repository.GetAsync(id)).Should().BeNull();
    }

    [Fact]
    public async Task GetBooksLeasedBy_ShouldReturnBooks_ThatWereNotReturned()
    {
        _repository = TestingDataProvider.GenerateHardCodedData().Users;
        var leasedBooks = (await _repository.GetBooksLeasedByUserAsync(TestingDataProvider.User2)).ToList();
        leasedBooks.Count.Should().Be(1);
        leasedBooks.First().Should().Be(TestingDataProvider.Book2);
    }

    [Fact]
    public async Task GetBooksLeasedBy_ShouldNotReturnBooks_ThatWereReturned()
    {
        _repository = TestingDataProvider.GenerateHardCodedData().Users;
        var leasedBooks = (await _repository.GetBooksLeasedByUserAsync(TestingDataProvider.User1)).ToList();
        leasedBooks.Count.Should().Be(0);
    }
}