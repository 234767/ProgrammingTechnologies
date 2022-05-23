using System.Runtime.CompilerServices;
using BusinessLogic.Abstractions;
using BusinessLogic.Abstractions.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Presentation.Core.Models;

namespace Presentation.Core.ViewModels;

public partial class LeaseDetailsViewModel : ViewModelBase
{
    private readonly ILeaseModel _lease;

    public LeaseDetailsViewModel( ILeaseModel lease )
    {
        _lease = lease;
        LoadDetails();
    }

    [ObservableProperty] 
    private IBookModel _bookDetails;

    [ObservableProperty]
    private IUserModel _userDetails;

    public string StartDate => _lease.LeaseDate.ToShortDateString();

    public string EndDate => _lease.ReturnDate.ToShortDateString();

    public string ReturnDate => _lease.ActualReturnDate?.ToShortDateString() ?? string.Empty;

    public bool ButtonEnabled => !_lease.IsReturned;

    [ICommand]
    private async Task ReturnButtonClick()
    {
        await _lease.Return();
    }

    private void LoadDetails()
    {
        BookDetails = _lease.GetBook();
        UserDetails = _lease.GetUser();
    }
}