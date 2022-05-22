using System.Collections.ObjectModel;
using System.Collections.Specialized;
using BusinessLogic.Abstractions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Presentation.Core.Models;

namespace Presentation.Core.ViewModels
{
    public partial class UsersViewModel : ViewModelBase
    {
        private readonly UserCollectionModel _users;

        public UsersViewModel(UserCollectionModel users)
        {
            _users = users;
        }

        [ObservableProperty]
        UserViewModel _activeUser = new (new UserModel(null)
        {
            Id = "user id 1",
            FirstName = "Imie",
            Surname = "Nazwisko"
        });

        [ObservableProperty] 
        private ObservableCollection<IUserModel> _userSearchResults = new(Enumerable.Empty<IUserModel>());

        [ObservableProperty] 
        private string _searchString = string.Empty;


        private int _selectedUser;

        public int SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
                try
                {
                    _activeUser = new UserViewModel( UserSearchResults[value] );
                    OnPropertyChanged( nameof(ActiveUser) );
                }
                catch ( ArgumentOutOfRangeException ) { }
            }
        }


        [ICommand]
        private async Task ExecuteSearch()
        {
            UserSearchResults.Clear();
            foreach ( IUserModel user in await _users.Search( SearchString ) )
            {
                UserSearchResults.Add(user);
            }

            SelectedUser = 0;
        }

        [ICommand]
        private async Task DeleteUser()
        {
            await UserSearchResults[SelectedUser].Delete();
            UserSearchResults.RemoveAt(SelectedUser);
            SelectedUser = 0;
        }
    }
}
