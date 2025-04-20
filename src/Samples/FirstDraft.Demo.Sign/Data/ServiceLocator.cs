using FirstDraft.Demo.Sign.ViewModels.Sign;
using FirstDraft.Demo.Sign.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstDraft.Demo.Sign.Data
{
    internal class ServiceLocator
    {
        public MainWindowModel MainWindowModel => App.Current.Services.GetService<MainWindowModel>();

        public SignViewModel SignVm => App.Current.Services.GetService<SignViewModel>();
        public SignInViewModel SignInVm => App.Current.Services.GetService<SignInViewModel>();
        public SignUpViewModel SignUpVm => App.Current.Services.GetService<SignUpViewModel>();
        public SignedViewModel SignedVm => App.Current.Services.GetService<SignedViewModel>();
    }
}
