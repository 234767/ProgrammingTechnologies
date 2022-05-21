using System;
using DataAccess.API.DTO;

namespace DataAccess.Database.Dto
{
    public class ReturnDto : IReturn
    {
        public string Id { get; set; } = null!;
        public DateTime Time { get; set; }
        public ILease Lease { get; set; } = null!;
    }
}
