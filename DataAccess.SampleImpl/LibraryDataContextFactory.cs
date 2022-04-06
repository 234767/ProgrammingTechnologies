using DataAccess.API.Abstractions;

namespace DataAccess.SampleImpl;

public class LibraryDataContextFactory : IDataContextFactory
{
    public ILibraryDataContext CreateDataContext() => new LibraryDataContext();
}