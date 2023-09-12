using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

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


        private Uri theme;

        public Uri Theme
        {
            get { return theme; }
            set
            {
                SetProperty(ref theme, value);
                var themeDict = new ResourceDictionary { Source = theme };
                Application.Current.Resources.MergedDictionaries.Add(themeDict);
            }
        }
        private Boolean isDarkTheme;

        public Boolean IsDarkTheme
        {
            get { return isDarkTheme; }
            set
            {
                SetProperty(ref isDarkTheme, value);
                if (isDarkTheme)
                {
                    Theme = Sources["幽暗"];
                }
                else
                {
                    Theme = Sources["明亮"];
                }
            }
        }
        public Dictionary<string, Uri> Sources { get; set; }

        public WelcomeViewModel()
        {
            Sources = new Dictionary<string, Uri>();
            Sources.Add("明亮", new Uri("/FirstDraft;component/Themes/Ui.Light.xaml", UriKind.Relative));
            Sources.Add("幽暗", new Uri("/FirstDraft;component/Themes/Ui.Dark.xaml", UriKind.Relative));
            Theme = Sources["明亮"];
        }
    }
}
