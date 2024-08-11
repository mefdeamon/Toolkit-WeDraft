using FirstDraft.ApplyDemo.Data;
using FirstDraft.ApplyDemo.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace FirstDraft.ApplyDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Services = ConfigureServices();

            this.InitializeComponent();
        }

        /// <summary>
        /// Gets the current <see cref="App"/> instance in use
        /// </summary>
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
            services.AddSingleton<ApplyIconButtonViewModel>();
            services.AddSingleton<WelcomeViewModel>();
            services.AddSingleton<IconSetViewModel>();
            services.AddSingleton<ApplyComboBoxViewModel>();
            services.AddSingleton<ApplyRadioButtonViewModel>();
            services.AddSingleton<ApplyIpAddressBoxViewModel>();
            services.AddSingleton<ApplyDateTimePickBoxViewModel>();
            services.AddSingleton<ApplyButtonViewModel>();
            services.AddSingleton<AboutWindowModel>();
            services.AddSingleton<ApplyDataGridViewModel>();
            


            return services.BuildServiceProvider();
        }
    }
}
