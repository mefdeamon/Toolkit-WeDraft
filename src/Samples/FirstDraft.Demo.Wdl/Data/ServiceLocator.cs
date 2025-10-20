using FirstDraft.Demo.Wdl;
using FirstDraft.Demo.Wdl.ViewModels;

namespace FirstDraft.Demo.Wdl.Data
{
    public class ServiceLocator
    {
        //public MainWindowModel MainWindowModel => App.Current.Services.GetService<MainWindowModel>();

        //public HomeViewModel HomeVm => App.Current.Services.GetService<HomeViewModel>();
        public HomeViewModel HomeVm { get; set; } = new HomeViewModel();

    }

    public static class SL
    {
        public static ServiceLocator Locator { get; set; } = new ServiceLocator();
    }

}
