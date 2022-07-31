using FirstDraft.ApplyDemo.ViewModels;
using MeiMvvm;

namespace FirstDraft.ApplyDemo.Data
{
    internal class ServiceLocator
    {
        public MainWindowModel MainWindowModel => ServiceProvider.Get<MainWindowModel>();
        public ApplyIconButtonViewModel ButtonViewModel => ServiceProvider.Get<ApplyIconButtonViewModel>();
        public WelcomeViewModel WelcomeViewModel => ServiceProvider.Get<WelcomeViewModel>();
        public IconSetViewModel IconSetViewModel => ServiceProvider.Get<IconSetViewModel>();
        public ApplyComboBoxViewModel ApplyComboBoxViewModel => ServiceProvider.Get<ApplyComboBoxViewModel>();
        
        public IconSet IconSet => ServiceProvider.Get<IconSet>();

    }
}
