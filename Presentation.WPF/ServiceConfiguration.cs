using BusinessLogic;
using BusinessLogic.Abstractions;
using BusinessLogic.Abstractions.Models;
using DataAccess.API.Abstractions;
using DataAccess.Database;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Core.Models;
using Presentation.Core.ViewModels;

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
            services.AddTransient<BooksViewModel>();
            //services.AddTransient<BookView>(s => new BookView()
            //{
            //    DataContext = s.GetRequiredService<BooksViewModel>()
            //});
            services.AddSingleton<UsersViewModel>();
            services.AddSingleton<ILibraryService, LibraryService>();
            services.AddSingleton<UserCollectionModel>();
            services.AddSingleton<BookCollectionModel>();
            services.AddSingleton<LeaseCollectionViewModel>();
            services.AddSingleton<LeaseCollectionModel>();

            var data = new DatabaseDataLayerFactory();
            services.AddSingleton<IUserRepository>( data.CreateUserRepository() );
            services.AddSingleton<IBookRepository>( data.CreateBookRepository() );
            services.AddSingleton<ILeaseRepository>( data.CreateLeaseRepository() );
            services.AddSingleton<IReturnRepository>( data.CreateReturnRepository() );
        }
    }
}
