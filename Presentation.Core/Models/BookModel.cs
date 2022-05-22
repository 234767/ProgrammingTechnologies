using System.Runtime.CompilerServices;
using BusinessLogic.Abstractions;
using BusinessLogic.Abstractions.Models;

namespace Presentation.Core.Models;

public class BookModel : IBookModel
{
    private readonly ILibraryService _libraryService;

    public BookModel( ILibraryService libraryService )
    {
        _libraryService = libraryService;
    }

    public string Id { get; set; }
    public string Title { get; set; } = null!;
    public string Author { get; set; } = null!;
    public DateOnly? DatePublished { get; set; }

    // Todo
    public bool IsAvailable => true;

    public async Task DeleteAsync()
    {
        await _libraryService.RemoveBook(Id);
    }
}