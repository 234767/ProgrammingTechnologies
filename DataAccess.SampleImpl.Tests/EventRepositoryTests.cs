using System;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.API.Abstractions;
using FluentAssertions;
using Xunit;

namespace DataAccess.Database.Tests;

public class EventRepositoryTests
{
    private IEventRepository _repository;

    public EventRepositoryTests()
    {
        _repository = TestingDataProvider.GetEmptyDataContext().Events;
    }

    [Fact]
    public async Task Create_ShouldIncreaseCount_WhenEventDoesNotYetExist()
    {
        ( await _repository.GetAllAsync() ).Count().Should().Be(0);
        await _repository.CreateAsync(new Lease("lease_1", DateTime.MinValue, null!, null!, TimeSpan.Zero));
        ( await _repository.GetAllAsync() ).Count().Should().Be(1);
    }

    [Fact]
    public async Task Create_ShouldNotAddEvent_WhenEventWithTheSameIdAlreadyExists()
    {
        const string id = "lease_1";
        var originalEvent = new Lease(id, DateTime.MinValue, null!, null!, TimeSpan.Zero);
        ( await _repository.GetAllAsync() ).Count().Should().Be(0);
        await _repository.CreateAsync(originalEvent);
        ( await _repository.GetAllAsync() ).Count().Should().Be(1);

        await _repository.CreateAsync(new Lease(id, DateTime.MaxValue, null!, null!, TimeSpan.MaxValue));
        ( await _repository.GetAllAsync() ).Count().Should().Be(1);
        ( await _repository.GetAsync(id) ).Should().NotBeNull().And.BeSameAs(originalEvent);
    }

    [Fact]
    public async Task Where_ShouldReturnAllEvents_ThatMatchTheExpression()
    {
        Lease lease1 = new Lease("lease_1", DateTime.Parse("01/01/2022"), null!, null!, TimeSpan.Zero);
        await _repository.CreateAsync(lease1);
        Lease lease2 = new Lease("lease_2", DateTime.Parse("01/02/2022"), null!, null!, TimeSpan.Zero);
        await _repository.CreateAsync(lease2);
        await _repository.CreateAsync(new Return("return_1", lease1, DateTime.Parse("01/03/2022")));
        await _repository.CreateAsync(new Return("return_2", lease2, DateTime.Parse("01/03/2022")));

        ( await _repository.WhereAsync(e => e is Lease) )
            .Count()
            .Should()
            .Be(2);

        ( await _repository.WhereAsync(e => e.Time > DateTime.Parse("01/02/2022")) )
            .Count()
            .Should()
            .Be(2);

        ( await _repository.WhereAsync(e => e.Time < DateTime.Parse("01/01/2020")) )
            .Count()
            .Should()
            .Be(0);
    }

    [Fact]
    public async Task Update_ShouldChangeEvent_WhenEventWithSuchIdExists()
    {
        const string id = "lease_2";
        await _repository.CreateAsync(new Lease("lease_1", DateTime.Parse("01/01/2022"), null!, null!, TimeSpan.Zero));
        await _repository.CreateAsync(new Lease(id, DateTime.Parse("01/01/2022"), null!, null!, TimeSpan.Zero));
        var updatedBook = new Lease(id, DateTime.Parse("02/02/2022"), null!, null!, TimeSpan.Zero);
        await _repository.UpdateAsync(updatedBook);
        ( await _repository.GetAsync(id) ).Should().BeSameAs(updatedBook);
    }

    [Fact]
    public async Task Delete_ShouldRemoveEvent_IfSuchExists()
    {
        const string id = "lease_1";
        Lease lease = new Lease(id, DateTime.Parse("01/01/2022"), null!, null!, TimeSpan.Zero);
        await _repository.CreateAsync(lease);
        await _repository.CreateAsync(lease with{Id = "lease_2"});
        await _repository.CreateAsync(lease with{Id = "lease_3"});


        (await _repository.GetAllAsync()).Count().Should().Be(3);
        await _repository.DeleteAsync(id);
        (await _repository.GetAllAsync()).Count().Should().Be(2);
        (await _repository.GetAsync(id)).Should().BeNull();
    }

    [Fact]
    public async Task GetLatesEventForBook_ReturnsCorrectEvent()
    {
        _repository = TestingDataProvider.GenerateHardCodedData().Events;
        (await _repository.GetLatestEventForBookAsync(TestingDataProvider.Book1.Id)).Should().Be(TestingDataProvider.Return1);
        (await _repository.GetLatestEventForBookAsync(TestingDataProvider.Book2.Id)).Should().Be(TestingDataProvider.Lease2);
    }

}