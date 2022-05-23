using BusinessLogic.Abstractions;
using BusinessLogic.Abstractions.Models;
using DataAccess.API.DTO;

namespace Presentation.Core.Models;

public class LeaseModel
{
    public ILibraryService Library { get; }
    public DateTime LeaseDate { get; set; }
    public IBookModel LeasedBook { get; set; }
    public IUserModel Borrower { get; set; }
    public DateTime ReturnDate { get; set; }
}