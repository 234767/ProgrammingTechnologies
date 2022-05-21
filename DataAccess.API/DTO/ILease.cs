using System;

namespace DataAccess.API.DTO;

public interface ILease : ILibraryEvent
{
    public IBook LeasedBook { get; }
    public IUser Borrower { get; }
    public DateTime ReturnDate { get; }
}