using System;
using DataAccess.API.Abstractions;

namespace DataAccess.Database;

public class DatabaseDataLayerFactory : IDataLayerFactory
{
    private readonly LibraryDataContext _dataContext;

    public DatabaseDataLayerFactory()
    {
        _dataContext = new LibraryDataContext();
    }

    public IBookRepository CreateBookRepository() => new BookRepository( _dataContext );

    public IUserRepository CreateUserRepository() => new UserRepository( _dataContext );

    public IEventRepository CreateEventRepository() => new LibraryEventRepository( _dataContext );

    [Obsolete]
    public ILibraryDataContext CreateDataContext()
    {
        throw new System.NotSupportedException("This method is deprecated");
    }
}