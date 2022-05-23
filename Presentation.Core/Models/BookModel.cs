using System.Runtime.CompilerServices;
using BusinessLogic.Abstractions;
using BusinessLogic.Abstractions.Models;

namespace Presentation.Core.Models;

public class BookModel : IBookModel
{
    public ILibraryService Library { get; }

    public BookModel( ILibraryService libraryService )
    {
        Library = libraryService;
    }

    public string Id { get; set; } = null!;
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public DateOnly? DatePublished { get; set; }

    public bool IsAvailable => true;

    public async Task DeleteAsync()
    {
        await Library.RemoveBook(Id);
    }

    public async Task Create()
    {
        await Library.AddBook(this);
    }

    public async Task Save()
    {
        await Library.SaveBook(this);
    }
}