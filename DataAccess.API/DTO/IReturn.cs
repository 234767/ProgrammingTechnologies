using System;

namespace DataAccess.API.DTO;

public interface IReturn
{
    public ILease Lease { get; }
    public DateTime Time { get; }
}