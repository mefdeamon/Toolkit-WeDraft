using FirstDraft.Demo.Sign.Core;
using FirstDraft.Demo.Sign.ViewModels;
using FirstDraft.Demo.Sign.ViewModels.Sign;
using FirstDraft.Demo.Sign.Views.Sign;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;

namespace FirstDraft.Demo.Sign
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public App()
        {
            Services = ConfigureServices();
        }

        public new static App Current => (App)Application.Current;

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
        /// </summary>
        public IServiceProvider Services { get; }

        /// <summary>
        /// Configures the services for the application.
        /// </summary>
        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton<IconSet>();
            services.AddSingleton<MainWindowModel>();
            services.AddSingleton<SignDbService>();
            services.AddSingleton<SignViewModel>();
            services.AddSingleton<SignInViewModel>();
            services.AddSingleton<SignUpViewModel>();
            services.AddSingleton<SignedViewModel>();



            services.AddSingleton<SignedView>();
            services.AddSingleton<SignInView>();
            services.AddSingleton<SignUpView>();



            return services.BuildServiceProvider();
        }
    }

}
