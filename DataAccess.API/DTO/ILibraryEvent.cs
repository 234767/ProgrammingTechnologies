using System;

namespace DataAccess.API.DTO;

public interface ILibraryEvent
{
    public string Id { get; }
    public DateTime Time { get; }
}