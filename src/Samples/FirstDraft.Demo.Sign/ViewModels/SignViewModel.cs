using CommunityToolkit.Mvvm.ComponentModel;
using FirstDraft.Demo.Sign.Views;
using FirstDraft.Demo.Sign.Views.Sign;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FirstDraft.Demo.Sign.ViewModels
{
    public class SignViewModel : ObservableObject
    {
        public SignViewModel()
        {
            ViewType = SignViewType.IN;
        }

        private SignViewType viewType = SignViewType.UP;
        public SignViewType ViewType
        {
            get { return viewType; }
            set
            {
                if (SetProperty(ref viewType, value))
                {
                    switch (viewType)
                    {
                        case SignViewType.IN:
                            CurrentView = App.Current.Services.GetService<SignInView>();
                            break;
                        case SignViewType.UP:
                            CurrentView = App.Current.Services.GetService<SignUpView>();
                            break;
                        case SignViewType.ED:
                            CurrentView = App.Current.Services.GetService<SignedView>();
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private FrameworkElement currentView;

        public FrameworkElement CurrentView
        {
            get { return currentView; }
            private set { currentView = value; OnPropertyChanged(); }
        }


        private bool isSigned;
        public bool IsSigned
        {
            get { return isSigned; }
            set { SetProperty(ref isSigned, value); }
        }


    }

    public enum SignViewType
    {
        IN,
        UP,
        ED
    }
}
