using System;

namespace DataAccess.API.DTO;

public interface IBookInfo
{
    public string Id { get; }
    public string Title { get; }
    public string Author { get; }
    public DateTime? DatePublished { get; }
}