using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VR.DiamondSquare.Model.Interfaces;
using VR.DiamondSquare.Model.Models;
using VR.DiamondSquare.ViewModel.ViewModels;

namespace VR.DiamondSquare.ViewModel
{
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder().ConfigureServices(ConfigureServices).Build();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<INormalMapGenerator, NormalMapGenerator>();
            services.AddTransient<IHeightMapGenerator, HeightMapGenerator>();

            services.AddSingleton<IRandomGenerator, RandomGenerator>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<MainWindow>(services => new MainWindow
            {
                DataContext = services.GetService<MainViewModel>()
            });
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();

            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            using (_host)
            {
                await _host.StopAsync();
            }

            base.OnExit(e);
        }
    }
}