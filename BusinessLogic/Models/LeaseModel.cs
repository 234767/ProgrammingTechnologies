using BusinessLogic.Abstractions;
using BusinessLogic.Abstractions.Models;

namespace BusinessLogic.Models;

public class LeaseModel : ILeaseModel
{
    private readonly ILibraryService _libraryService;
    private readonly IUserModel _userModel;
    private readonly IBookModel _bookModel;

    public LeaseModel( ILibraryService libraryService, string id, DateTime leaseDate,
                       DateTime returnDate,
                       DateTime? actualReturnDate,
                       IUserModel userModel,
                       IBookModel bookModel )
    {
        _libraryService = libraryService;
        Id = id;
        LeaseDate = leaseDate;
        ReturnDate = returnDate;
        ActualReturnDate = actualReturnDate;
        _userModel = userModel;
        _bookModel = bookModel;
    }

    public string Id { get; }
    public DateTime LeaseDate { get; }
    public string BorrowerId => _userModel.Id;
    public string LeasedBookId => _bookModel.Id;
    public bool IsReturned => ActualReturnDate.HasValue;
    public DateTime ReturnDate { get; }
    public DateTime? ActualReturnDate { get; set; }

    public async Task Return()
    {
        await _libraryService.ReturnBook( _bookModel.Id );
    }

    public IBookModel GetBook()
    {
        return _bookModel;
    }

    public IUserModel GetUser()
    {
        return _userModel;
    }
}