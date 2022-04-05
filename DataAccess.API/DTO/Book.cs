using System;

namespace DataAccess.API.DTO;

public record Book(string Id, string Title, string Author, string State, DateOnly? DateOfIssue);