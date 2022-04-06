using System;

namespace DataAccess.API.DTO;

public record Lease(string Id, Book LeaseBook, User Borrower, DateOnly Date, TimeSpan LeaseDuration) : ILibraryEvent;