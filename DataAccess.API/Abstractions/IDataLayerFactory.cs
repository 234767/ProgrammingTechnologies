using System;

namespace DataAccess.API.Abstractions;

public interface IDataLayerFactory
{
    IBookRepository CreateBookRepository();

    IUserRepository CreateUserRepository();

    IEventRepository CreateEventRepository();
}
