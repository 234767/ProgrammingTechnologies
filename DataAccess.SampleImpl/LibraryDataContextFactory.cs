using DataAccess.API.Abstractions;

namespace DataAccess.Database;

public class LibraryDataContextFactory : IDataContextFactory
{
    public ILibraryDataContext CreateDataContext() => new LibraryDataContext();
}