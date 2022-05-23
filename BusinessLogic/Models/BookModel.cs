using BusinessLogic.Abstractions;
using BusinessLogic.Abstractions.Models;

namespace BusinessLogic.Models;

public class BookModel : IBookModel
{
    public BookModel( ILibraryService libraryService, string id, string title, string author,
                      DateOnly? datePublished,
                      bool isAvailable )
    {
        Library = libraryService;
        Id = id;
        Title = title;
        Author = author;
        DatePublished = datePublished;
        IsAvailable = isAvailable;
    }

    public ILibraryService Library { get; }
    public string Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public DateOnly? DatePublished { get; set; }

    public bool IsAvailable { get; }

    public async Task Create()
    {
        await Library.AddBook(this);
    }

    public async Task Save()
    {
        await Library.SaveBook(this);
    }

    public async Task DeleteAsync()
    {
        await Library.RemoveBook(this.Id);
    }
}