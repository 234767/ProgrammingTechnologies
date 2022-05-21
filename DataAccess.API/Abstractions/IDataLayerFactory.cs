using System;

namespace DataAccess.API.Abstractions;

public interface IDataLayerFactory
{
    IBookRepository CreateBookRepository();

    IUserRepository CreateUserRepository();

    ILeaseRepository CreateLeaseRepository();

    IReturnRepository CreateReturnRepository();
}
