using BusinessLogic.Abstractions.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Presentation.Core.Models;

namespace Presentation.Core.ViewModels;

public partial class BookEditViewModel : ViewModelBase
{
    [ObservableProperty]
    private IBookModel _book;

    private bool _newBook = false;

    public BookEditViewModel( IBookModel book )
    {
        _book = book;
    }


    #region IBookModel Members

    public string Id => _book.Id;

    public string Title
    {
        get => _book.Title;
        set
        {
            _book.Title = value;
            OnPropertyChanged();
        }
    }

    public string Author
    {
        get => _book.Author;
        set
        {
            _book.Author = value;
            OnPropertyChanged();
        }
    }

    public DateTime? PublicationDate
    {
        get => _book.DatePublished?.ToDateTime(new TimeOnly());
        set
        {
            _book.DatePublished = value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
            OnPropertyChanged();
        }
    }

    #endregion

    [ICommand]
    private async Task Save()
    {
        if ( _newBook )
        {
            await _book.Create();
            _newBook = false;
        }
        else
        {
            await _book.Save();
        }
    }

    [ICommand]
    private void ClearDate()
    {
        PublicationDate = null;
    }

    [ICommand]
    private void NewBook()
    {
        _newBook = true;
        _book = new BookModel( _book.Library );
        OnPropertyChanged(nameof(Id));
        OnPropertyChanged(nameof(Title));
        OnPropertyChanged(nameof(Author));
        OnPropertyChanged(nameof(PublicationDate));
    }

}