using BusinessLogic.Abstractions;

namespace Presentation.Core.ViewModels;

public class LeaseReturnModel
{
    private ILibraryService libraryService;
    public LeaseReturnModel( ILibraryService libraryService )
    {
        this.libraryService = libraryService;
    }
}