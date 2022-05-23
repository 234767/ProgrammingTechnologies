using System.Collections.ObjectModel;
using BusinessLogic.Abstractions.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Presentation.Core.Models;

namespace Presentation.Core.ViewModels
{
    public partial class BooksViewModel : ViewModelBase
    {
        private readonly BookCollectionModel _books;

        public BooksViewModel( BookCollectionModel books )
        {
            _books = books;
            _activeBook = new BookEditViewModel( _books.GetNewBook() );
            _ = ExecuteSearch();
        }

        [ObservableProperty] 
        private BookEditViewModel _activeBook;

        [ObservableProperty] 
        private ObservableCollection<IBookModel> _bookSearchResults = new(Enumerable.Empty<IBookModel>());

        [ObservableProperty] private string _searchCategory = string.Empty;
        [ObservableProperty] private string _searchString = string.Empty;


        private int _selectedBook;

        public int SelectedBook { 
            get => _selectedBook;
            set
            {
                _selectedBook = value;
                OnPropertyChanged();
                try
                {
                    _activeBook = new BookEditViewModel( _bookSearchResults[value] );
                    OnPropertyChanged( nameof(ActiveBook) );
                }
                catch ( ArgumentOutOfRangeException ) { }
            }
        }


        [ICommand]
        private async Task ExecuteSearch()
        {
            BookSearchResults.Clear();
            foreach ( IBookModel book in await _books.Search(SearchCategory, SearchString) )
            {
                BookSearchResults.Add(book);
            }

            SelectedBook = 0;
        }


        [ICommand]
        private async Task DeleteBook()
        {
            await BookSearchResults[SelectedBook].DeleteAsync();
            BookSearchResults.RemoveAt(SelectedBook);
            SelectedBook = 0;
        }
    }
}
