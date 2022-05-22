using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using BusinessLogic.Abstractions.Models;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Core.Models;
using Presentation.Core.ViewModels;
using Presentation.WPF.Views;

namespace Presentation.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            ServiceCollection services = new ServiceCollection();
            services.ConfigureServices();
            _serviceProvider = services.BuildServiceProvider();
        }

        private void OnStartup( object sender, StartupEventArgs e )
        {
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}
