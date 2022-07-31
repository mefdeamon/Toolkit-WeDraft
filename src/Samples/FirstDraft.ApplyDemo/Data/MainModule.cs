using FirstDraft.ApplyDemo.ViewModels;
using MeiMvvm;

namespace FirstDraft.ApplyDemo.Data
{
    internal class MainModule : IModule
    {
        public void OnLoad(IBinder binder)
        {
            binder.BindSingleton<IconSet>();
            binder.BindSingleton<MainWindowModel>();
            binder.BindSingleton<ApplyIconButtonViewModel>();
            binder.BindSingleton<WelcomeViewModel>();
            binder.BindSingleton<IconSetViewModel>();
            binder.BindSingleton<ApplyComboBoxViewModel>();
            
        }
    }
}
