using System;

namespace DataAccess.API.DTO;

public record User(string Id, string FirstName, string Surname, DateOnly DateOfBirth);