using System;
using System.Threading.Tasks;
using BusinessLogic.Abstractions;
using Presentation.Core.ViewModels;
using NSubstitute;
using FluentAssertions;
using Xunit;

namespace Presentation.Core.Tests;

public class UserEditViewModelTests
{
    private readonly UserEditViewModel _viewmodel;
    private readonly IUserModel _model;
    public UserEditViewModelTests()
    {
        _model = Substitute.For<IUserModel>();
        _viewmodel = new UserEditViewModel(_model);
    }

    [Fact]
    private async Task SaveCommand_ShouldSaveUser_IfItIsNotNewUser()
    {
        await _viewmodel.SaveCommand.ExecuteAsync(null);
        await _model.Received(1).Save();
    }

    [Fact]
    private async Task CreateCommand_ShouldCreatesNewUser_WhenSavingNewUser()
    {
        var library = Substitute.For<ILibraryService>();
        _model.Library.Returns(library);
        
        _viewmodel.NewUserCommand.Execute(null);
        await _viewmodel.SaveCommand.ExecuteAsync(null);
        await _model.DidNotReceive().Save();
        await library.Received(1).AddUser(Arg.Any<IUserModel>());
    }
}