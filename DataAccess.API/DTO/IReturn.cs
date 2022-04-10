using System;

namespace DataAccess.API.DTO;

public interface IReturn : ILibraryEvent
{
    public ILease Lease { get; }
    public DateTime Time { get; }
}