namespace DataAccess.API.Abstractions;

public interface IDataContextFactory
{
    ILibraryDataContext CreateDataContext();
}

public interface ILibraryDataContext { }