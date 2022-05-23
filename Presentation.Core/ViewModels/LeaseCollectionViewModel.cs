using System.Collections.ObjectModel;
using BusinessLogic.Abstractions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Presentation.Core.Models;

namespace Presentation.Core.ViewModels;

public partial class LeaseCollectionViewModel : ViewModelBase
{
    private LeaseCollectionModel model;

    public LeaseCollectionViewModel( LeaseCollectionModel model )
    {
        this.model = model;
        _activeLease = null;
        _ = ExecuteSearch();
    }

    [ObservableProperty] 
    private LeaseDetailsViewModel _activeLease;

    [ObservableProperty]
    private ObservableCollection<ILeaseModel> _leaseSearchResults = new(Enumerable.Empty<ILeaseModel>());

    private int _selectedLeaseIndex;

    public int SelectedLeaseIndex
    {
        get => _selectedLeaseIndex;
        set
        {
            _selectedLeaseIndex = value;
            OnPropertyChanged();
            try
            {
                _activeLease = new LeaseDetailsViewModel( _leaseSearchResults[value] );
                OnPropertyChanged( nameof(ActiveLease) );
            }
            catch ( ArgumentOutOfRangeException ) { }
        }
    }

    [ICommand]
    private async Task ExecuteSearch()
    {
        LeaseSearchResults.Clear();
        foreach ( ILeaseModel lease in await model.GetAll() )
        {
            LeaseSearchResults.Add(lease);
        }

        SelectedLeaseIndex = 0;
    }

    [ObservableProperty]
    private string _newLeaseBookId = string.Empty;

    [ObservableProperty] 
    private string _newLeaseUserId = string.Empty;

    [ICommand]
    private async Task AddNewLease()
    {
        await model.AddNew( NewLeaseBookId, NewLeaseUserId );
    }
}