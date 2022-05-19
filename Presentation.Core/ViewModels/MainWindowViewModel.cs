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
        private double rotation;

        [ObservableProperty]
        private ViewModelBase selectedViewModel = new BookViewModel();

        [ObservableProperty]
        private List<BookModel> books = new(){
                                                 new(){Author = "Helo", Title = "Jacek"},
                                                 new (){Author = "Victor Hugo", Title = "Nedznicy Nedzicy", PublicationYear = 666}
                                             };

        [ICommand]
        private async Task OnButtonClick()
        {
            await Task.Delay(2000);
            Progress = 90;
        }

        private CancellationTokenSource cts = null!;

        [ICommand]
        private async Task OnButtonHover()
        {
            cts = new();
            try
            {
                await Task.Run(
                    () =>
                    {
                        while ( true )
                        {
                            cts.Token.ThrowIfCancellationRequested();
                            Rotation += 0.0001;
                            if ( Rotation > 360 )
                            {
                                Rotation = 0;
                            }
                        }
                    } );
            }
            catch ( TaskCanceledException )
            {
                return;
            }
        }

        [ICommand]
        private void OnButtonHoverLeave()
        {
            cts.Cancel();
        }
    }
}