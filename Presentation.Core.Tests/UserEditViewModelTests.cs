using System.Threading.Tasks;
using BusinessLogic.Abstractions;
using Presentation.Core.ViewModels;
using NSubstitute;
using FluentAssertions;
using Xunit;

namespace Presentation.Core.Tests;

public class UserEditViewModelTests
{
    private readonly UserViewModel _viewmodel;
    private readonly IUserModel _model;
    public UserEditViewModelTests()
    {
        _model = Substitute.For<IUserModel>();
        _viewmodel = new UserViewModel(_model);
    }

    [Fact]
    private async Task SaveCommandSavesUserIfItIsNotNewUser()
    {
        await _viewmodel.SaveCommand.ExecuteAsync(null);
        await _model.Received(1).Save();
    }

    [Fact]
    private async Task CreateCommandCreatesNewUserAfterSave()
    {
        _viewmodel.NewUserCommand.Execute(null);
        await _viewmodel.SaveCommand.ExecuteAsync(null);
        await _model.DidNotReceive().Save();
        await _model.Received(1).Crete();
    }
}