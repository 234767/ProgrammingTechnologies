using BusinessLogic.Abstractions;

namespace Presentation.Core.Models;

public class LeaseCollectionModel
{
    private readonly ILibraryService _library;

    public LeaseCollectionModel( ILibraryService library )
    {
        _library = library;
    }

    public async Task<IEnumerable<ILeaseModel>> GetAll()
    {
        return ( await _library.GetAllLeases() ).ToList();
    }

    public async Task AddNew( string newLeaseBookId, string newLeaseUserId )
    {
        await _library.TryBorrow( newLeaseBookId, newLeaseUserId );
    }
}