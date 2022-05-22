using System.Globalization;
using BusinessLogic.Abstractions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Presentation.Core.Models;

namespace Presentation.Core.ViewModels;

public partial class UserViewModel : ViewModelBase
{
    [ObservableProperty] 
    [AlsoNotifyChangeFor(nameof(Id))]
    [AlsoNotifyChangeFor(nameof(Id))]
    [AlsoNotifyChangeFor(nameof(Id))]
    private IUserModel _user;

    private bool _newUser = false;

    public UserViewModel( IUserModel user )
    {
        _user = user;
    }

    #region UserModel Members

    public string Id
    {
        get => _user.Id;
        set
        {
            _user.Id = value;
            OnPropertyChanged(nameof(Id));
        }
    }

    public string FirstName
    {
        get => _user.FirstName;
        set
        {
            _user.FirstName = value;
            OnPropertyChanged(nameof(FirstName));
        }
    }

    public string Surname
    {
        get => _user.Surname;
        set
        {
            _user.Surname = value;
            OnPropertyChanged(nameof(Surname));
        }
    }

    #endregion

    [ICommand]
    private async Task Save()
    {
        if ( _newUser )
        {
            await _user.Crete();
            _newUser = false;
        }
        else
        {
            await _user.Save();
        }
    }

    [ICommand]
    private void NewUser()
    {
        _newUser = true;
        _user = new UserModel( _user.Library );
        OnPropertyChanged(nameof(Id));
        OnPropertyChanged(nameof(FirstName));
        OnPropertyChanged(nameof(Surname));
    }

    [ICommand]
    private async Task Delete()
    {
        await _user.Delete();
    }
}