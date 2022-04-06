using System;

namespace DataAccess.API.DTO;

public record Lease(string Id, string BookId, string UserId, DateOnly Date, TimeSpan LeaseDuration) : ILibraryEvent;