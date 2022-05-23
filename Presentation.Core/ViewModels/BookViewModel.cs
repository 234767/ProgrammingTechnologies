using BusinessLogic.Abstractions.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Presentation.Core.ViewModels
{
    public partial class BookViewModel : ViewModelBase
    {
        private IBookModel _currentBook;

        public BookViewModel( IBookModel currentBook )
        {
            this._currentBook = currentBook;
        }



        [ObservableProperty] 
        private List<IBookModel> _bookSearchResults;

        private BookEditViewModel _activeBook;

        private int _selectedBook;

        public int SelectedBook { 
            get => _selectedBook;
            set
            {
                _selectedBook = value;
                _currentBook = _bookSearchResults[value];
                OnPropertyChanged();
                OnPropertyChanged(nameof(BookSearchResults));
            }
        }

        [ICommand]
        private void Test()
        {
            Author = "HHHHHHH";
        }

        [ICommand]
        private void ExecuteSearch()
        {
            throw new NotImplementedException();
        }
    }
}
