using System;

namespace DataAccess.API.DTO;

public record Return(string Id, Lease Lease, DateOnly Date);