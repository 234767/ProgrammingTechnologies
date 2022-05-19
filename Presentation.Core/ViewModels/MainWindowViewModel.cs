using System;
using System.Collections.Generic;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Presentation.Core.Models;

namespace Presentation.Core.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private double progress;

        [ObservableProperty]
        private List<BookModel> books = new(){
                                                 new(){Author = "Helo", Title = "Jacek"},
                                                 new (){Author = "Victor Hugo", Title = "Nedznicy Nedzicy"}
                                             };

        [ICommand]
        private async Task OnButtonClick()
        {
            await Task.Delay(2000);
            Progress = 50;
        }
    }
}