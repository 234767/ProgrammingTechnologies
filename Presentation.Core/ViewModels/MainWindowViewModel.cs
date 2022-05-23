using System;
using System.Collections.Generic;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Presentation.Core.Models;

namespace Presentation.Core.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private readonly Dictionary<Type, ViewModelBase> subViews = new();

        [ObservableProperty]
        private ViewModelBase _selectedViewModel;

        public MainWindowViewModel( BooksViewModel booksViewModel, UsersViewModel usersViewModel )
        {
            subViews[typeof(BooksViewModel)] = booksViewModel;
            subViews[typeof(UsersViewModel)] = usersViewModel;
            SelectBooks();
        }

        [ICommand]
        private void SelectBooks()
        {
            SelectedViewModel = subViews[typeof(BooksViewModel)];
        }

        [ICommand]
        private void SelectUsers()
        {
            SelectedViewModel = subViews[typeof(UsersViewModel)];
        }

        [ICommand]
        private void SelectTransactions()
        {
            return;
            SelectedViewModel = subViews[typeof(LeaseReturnViewModel)];
        }
    }
}