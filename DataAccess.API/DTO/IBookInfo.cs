using System;

namespace DataAccess.API.DTO;

public class IBookInfo
{
    public string Id { get; }
    public string Title { get; }
    public string Author { get; }
    public DateOnly? DatePublished { get; }
}