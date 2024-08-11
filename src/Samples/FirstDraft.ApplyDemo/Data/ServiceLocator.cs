using FirstDraft.ApplyDemo.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace FirstDraft.ApplyDemo.Data
{
    internal class ServiceLocator
    {
        public MainWindowModel MainWindowModel => App.Current.Services.GetService<MainWindowModel>();
        public ApplyIconButtonViewModel ButtonViewModel => App.Current.Services.GetService<ApplyIconButtonViewModel>();
        public WelcomeViewModel WelcomeViewModel => App.Current.Services.GetService<WelcomeViewModel>();
        public IconSetViewModel IconSetViewModel => App.Current.Services.GetService<IconSetViewModel>();
        public ApplyComboBoxViewModel ApplyComboBoxViewModel => App.Current.Services.GetService<ApplyComboBoxViewModel>();
        public ApplyRadioButtonViewModel ApplyRadioButtonViewModel => App.Current.Services.GetService<ApplyRadioButtonViewModel>();
        public ApplyIpAddressBoxViewModel ApplyIpAddressBoxViewModel => App.Current.Services.GetService<ApplyIpAddressBoxViewModel>();
        public ApplyDateTimePickBoxViewModel ApplyDateTimePickBoxViewModel => App.Current.Services.GetService<ApplyDateTimePickBoxViewModel>();

        public ApplyButtonViewModel ApplyButtonViewModel => App.Current.Services.GetService<ApplyButtonViewModel>();
        
        public IconSet IconSet => App.Current.Services.GetService<IconSet>();
        public AboutWindowModel AboutWindowModel => App.Current.Services.GetService<AboutWindowModel>();
        public ApplyDataGridViewModel ApplyDataGridViewModel => App.Current.Services.GetService<ApplyDataGridViewModel>();

    }

}
