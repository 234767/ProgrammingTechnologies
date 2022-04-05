namespace DataAccess.API.Abstractions;

public interface IDataContextFactory
{
    ILibraryDataContext CreateDataContext();
}