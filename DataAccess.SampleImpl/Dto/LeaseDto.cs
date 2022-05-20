using System;
using DataAccess.API.DTO;

namespace DataAccess.Database.Dto
{
    internal class LeaseDto : ILease
    {
        public string Id { get; set; } = null!;
        public DateTime Time { get; set; } 
        public IBook LeasedBook { get; set; } = null!;
        public IUser Borrower { get; set; } = null!;
        public TimeSpan Duration { get; set; }
    }
}
