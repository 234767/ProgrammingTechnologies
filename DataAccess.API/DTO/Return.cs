using System;

namespace DataAccess.API.DTO;

public record Return(string Id, string LeaseId, DateOnly Date);