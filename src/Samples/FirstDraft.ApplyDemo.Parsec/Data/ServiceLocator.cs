using FirstDraft.ApplyDemo.Parsec.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstDraft.ApplyDemo.Parsec.Data
{
    internal class ServiceLocator
    {
        public MainWindowModel? MainWindowModel => App.Current.Services.GetService<MainWindowModel>();
        public SettingsViewModel? SettingsViewModel => App.Current.Services.GetService<SettingsViewModel>();
        
    }
}
