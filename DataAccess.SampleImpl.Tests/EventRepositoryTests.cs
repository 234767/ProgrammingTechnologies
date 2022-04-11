using System;
using System.Linq;
using DataAccess.API.Abstractions;
using DataAccess.API.DTO;
using FluentAssertions;
using Xunit;

namespace DataAccess.SampleImpl.Tests;

public class EventRepositoryTests
{
    private IEventRepository _repository;

    public EventRepositoryTests()
    {
        _repository = TestingDataProvider.GetEmptyDataContext().Events;
    }

    [Fact]
    public void Create_ShouldIncreaseCount_WhenEventDoesNotYetExist()
    {
        _repository.GetAll().Count().Should().Be(0);
        _repository.Create(new Lease("lease_1", DateTime.MinValue, null!, null!, TimeSpan.Zero));
        _repository.GetAll().Count().Should().Be(1);
    }

    [Fact]
    public void Create_ShouldNotAddEvent_WhenEventWithTheSameIdAlreadyExists()
    {
        const string id = "lease_1";
        var originalEvent = new Lease(id, DateTime.MinValue, null!, null!, TimeSpan.Zero);
        _repository.GetAll().Count().Should().Be(0);
        _repository.Create(originalEvent);
        _repository.GetAll().Count().Should().Be(1);

        _repository.Create(new Lease(id, DateTime.MaxValue, null!, null!, TimeSpan.MaxValue));
        _repository.GetAll().Count().Should().Be(1);
        _repository.Get(id).Should().NotBeNull().And.BeSameAs(originalEvent);
    }

    [Fact]
    public void Where_ShouldReturnAllEvents_ThatMatchTheExpression()
    {
        Lease lease1 = new Lease("lease_1", DateTime.Parse("01/01/2022"), null!, null!, TimeSpan.Zero);
        _repository.Create(lease1);
        Lease lease2 = new Lease("lease_2", DateTime.Parse("01/02/2022"), null!, null!, TimeSpan.Zero);
        _repository.Create(lease2);
        _repository.Create(new Return("return_1", lease1, DateTime.Parse("01/03/2022")));
        _repository.Create(new Return("return_2", lease2, DateTime.Parse("01/03/2022")));

        _repository.Where(e => e is Lease)
                   .Count()
                   .Should()
                   .Be(2);

        _repository.Where(e => e.Time > DateTime.Parse("01/02/2022"))
                   .Count()
                   .Should()
                   .Be(2);

        _repository.Where(e => e.Time < DateTime.Parse("01/01/2020"))
                   .Count()
                   .Should()
                   .Be(0);
    }

    [Fact]
    public void Update_ShouldChangeEvent_WhenEventWithSuchIdExists()
    {
        const string id = "lease_2";
        _repository.Create(new Lease("lease_1", DateTime.Parse("01/01/2022"), null!, null!, TimeSpan.Zero));
        _repository.Create(new Lease(id, DateTime.Parse("01/01/2022"), null!, null!, TimeSpan.Zero));
        var updatedBook = new Lease(id, DateTime.Parse("02/02/2022"), null!, null!, TimeSpan.Zero);
        _repository.Update(updatedBook);
        _repository.Get(id).Should().BeSameAs(updatedBook);
    }

    [Fact]
    public void Delete_ShouldRemoveEvent_IfSuchExists()
    {
        const string id = "lease_1";
        Lease lease = new Lease(id, DateTime.Parse("01/01/2022"), null!, null!, TimeSpan.Zero);
        _repository.Create(lease);
        _repository.Create(lease with{Id = "lease_2"});
        _repository.Create(lease with{Id = "lease_3"});


        _repository.GetAll().Count().Should().Be(3);
        _repository.Delete(id);
        _repository.GetAll().Count().Should().Be(2);
        _repository.Get(id).Should().BeNull();
    }

    [Fact]
    public void GetLatesEventForBook_ReturnsCorrectEvent()
    {
        _repository = TestingDataProvider.GenerateHardCodedData().Events;
        _repository.GetLatestEventForBook(TestingDataProvider.Book1).Should().Be(TestingDataProvider.Return1);
        _repository.GetLatestEventForBook(TestingDataProvider.Book2).Should().Be(TestingDataProvider.Lease2);
        
    }

}