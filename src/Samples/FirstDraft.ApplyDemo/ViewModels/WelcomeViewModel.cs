using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace FirstDraft.ApplyDemo.ViewModels
{
    internal class WelcomeViewModel : ObservableObject
    {
        public RelayCommand NavigateToMvvmCommand => new RelayCommand(() =>
        {
            string path = @"https://learn.microsoft.com/zh-cn/dotnet/communitytoolkit/mvvm/";
            System.Diagnostics.Process.Start("explorer.exe", path);
        });

        public RelayCommand NavigateToBilibiliVedioCommand => new RelayCommand(() =>
        {
            string path = @"https://www.bilibili.com/video/BV1AA411J7vZ";
            System.Diagnostics.Process.Start("explorer.exe", path);
        });
    }
}
