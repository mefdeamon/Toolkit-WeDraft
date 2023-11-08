using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;

namespace FirstDraft.ApplyDemo.ViewModels
{
    public class ApplyButtonViewModel : ObservableObject
    {
        public ApplyButtonViewModel() { }


        private Boolean isRunning;

        public Boolean IsRunning
        {
            get { return isRunning; }
            set { SetProperty(ref isRunning, value); }
        }


        public RelayCommand SignCommand => new RelayCommand(async () =>
        {
            IsRunning = true;

            await Sign();
        },
            () =>
            {
                return !IsRunning;
            }
            );

        private async Task Sign()
        {
            await Task.Delay(5000);

            IsRunning = false;
        }

    }

}
