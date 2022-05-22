using BusinessLogic.Abstractions.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Presentation.Core.Models;

namespace Presentation.Core.ViewModels
{
    public partial class BookViewModel : ViewModelBase
    {
        [ObservableProperty]
        private IBookModel _currentBook;

        public BookViewModel( IBookModel currentBook )
        {
            this._currentBook = currentBook;
        }

        #region IBookModel Members

        public string Id => _currentBook.Id;

        public string Title
        {
            get => _currentBook.Title;
            set
            {
                _currentBook.Title = value;
                OnPropertyChanged( nameof(Title) );
            }
        }

        public string Author
        {
            get => _currentBook.Author;
            set
            {
                _currentBook.Author = value;
                OnPropertyChanged( nameof(Author) );
            }
        }

        #endregion

        [ObservableProperty] private List<IBookModel> bookSearchResults;

        private int _selectedBook;

        public int SelectedBook { 
            get => _selectedBook;
            set
            {
                _selectedBook = value;
                _currentBook = bookSearchResults[value];
                OnPropertyChanged(nameof(CurrentBook));
                OnPropertyChanged(nameof(BookSearchResults));
            }
        }

        [ICommand]
        private async Task DeleteBook()
        {
            await _currentBook.DeleteAsync();
        }

        [ICommand]
        private void Test()
        {
            Author = "HHHHHHH";
        }
    }
}
