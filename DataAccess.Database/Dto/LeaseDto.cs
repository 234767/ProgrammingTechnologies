using System;
using DataAccess.API.DTO;

namespace DataAccess.Database.Dto
{
    public class LeaseDto : ILease
    {
        public string Id { get; set; } = null!;
        public DateTime Time { get; set; } 
        public IBook LeasedBook { get; set; } = null!;
        public IUser Borrower { get; set; } = null!;
        public DateTime ReturnDate { get; set; }
    }
}
