using BusinessLogic;
using BusinessLogic.Abstractions;
using BusinessLogic.Abstractions.Models;
using DataAccess.API.Abstractions;
using DataAccess.Database;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Core.Models;
using Presentation.Core.ViewModels;
using Presentation.WPF.Views;

namespace Presentation.WPF
{
    internal static class ServiceConfiguration
    {
        internal static void ConfigureServices( this IServiceCollection services )
        {
            services.AddSingleton<MainWindow>(s => new MainWindow()
            {
                DataContext = s.GetRequiredService<MainWindowViewModel>()
            });
            services.AddSingleton<MainWindowViewModel>();
            services.AddTransient<IBookModel, BookModel>();
            services.AddTransient<BookViewModel>();
            services.AddTransient<BookView>(s => new BookView()
            {
                DataContext = s.GetRequiredService<BookViewModel>()
            });
            services.AddSingleton<UsersViewModel>();
            services.AddSingleton<ILibraryService, LibraryService>();
            services.AddSingleton<UserCollectionModel>();

            var data = new DatabaseDataLayerFactory();
            services.AddSingleton<IUserRepository>( data.CreateUserRepository() );
            services.AddSingleton<IBookRepository>( data.CreateBookRepository() );
            services.AddSingleton<ILeaseRepository>( data.CreateLeaseRepository() );
            services.AddSingleton<IReturnRepository>( data.CreateReturnRepository() );
        }
    }
}
