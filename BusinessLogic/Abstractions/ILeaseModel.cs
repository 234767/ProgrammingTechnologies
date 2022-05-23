using BusinessLogic.Abstractions.Models;

namespace BusinessLogic.Abstractions;

public interface ILeaseModel
{
    public string Id { get; }
    public DateTime LeaseDate { get; }
    public string BorrowerId { get; }
    public string LeasedBookId { get; }
    public bool IsReturned { get; }
    public DateTime ReturnDate { get; }
    public DateTime? ActualReturnDate { get; set; }
    Task Return();
    IBookModel GetBook();
    IUserModel GetUser();
}