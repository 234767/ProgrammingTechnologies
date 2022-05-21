using System;
using DataAccess.API.DTO;

namespace DataAccess.Database.Dto
{
    internal class ReturnDto : IReturn
    {
        public string Id { get; set; } = null!;
        public DateTime Time { get; set; }
        internal LeaseDto LeaseDto { get; set; } = null!;
        public ILease Lease => LeaseDto;
    }
}
