using Microsoft.Extensions.DependencyInjection;

namespace FirstDraft.ApplyDemo.WXWork.Data
{
    internal class ServiceLocator
    {
        public MainWindowModel MainWindowModel => App.Current.Services.GetService<MainWindowModel>();
        public ViewModels.MeetingViewModel MeetingViewModel => App.Current.Services.GetService<ViewModels.MeetingViewModel>();
      

        public IconSet IconSet => App.Current.Services.GetService<IconSet>();

    }
}
