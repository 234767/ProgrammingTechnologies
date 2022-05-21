using System;
using DataAccess.API.Abstractions;
using DataAccess.Database.Repositories;

namespace DataAccess.Database;

public class DatabaseDataLayerFactory : IDataLayerFactory
{
    private readonly LibraryDataContext _dataContext;

    public DatabaseDataLayerFactory()
    {
        _dataContext = new LibraryDataContext();
    }
    
    public DatabaseDataLayerFactory(string connectionString)
    {
        _dataContext = new LibraryDataContext(connectionString);
    }

    public IBookRepository CreateBookRepository() => new BookRepository( _dataContext );

    public IUserRepository CreateUserRepository() => new UserRepository( _dataContext );

    public ILeaseRepository CreateLeaseRepository() => new LeaseRepository( _dataContext );

    public IReturnRepository CreateReturnRepository() => new ReturnRepository( _dataContext );
}