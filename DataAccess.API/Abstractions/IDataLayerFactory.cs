using System;

namespace DataAccess.API.Abstractions;

public interface IDataLayerFactory
{
    [Obsolete]
    ILibraryDataContext CreateDataContext();

    IBookRepository CreateBookRepository();

    IUserRepository CreateUserRepository();

    IEventRepository CreateEventRepository();
}

public interface ILibraryDataContext { }