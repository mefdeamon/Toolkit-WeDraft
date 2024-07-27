using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FirstDraft.ApplyDemo
{
    /// <summary>
    /// AboutWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AboutWindow : FdWindow
    {
        public AboutWindow()
        {
            InitializeComponent();
        }
        static AboutWindow _instance;

        public static void ShowInstance()
        {
            if (_instance == null)
            {
                _instance = new AboutWindow();
                _instance.Owner = Application.Current.MainWindow;
                _instance.Closing += (S, E) => _instance = null;
            }

            _instance.ShowAndFocus();
        }
    }
    public static class WindowsExtensions
    {
        public static void ShowAndFocus(this Window W)
        {
            if (W.IsVisible && W.WindowState == WindowState.Minimized)
            {
                W.WindowState = WindowState.Normal;
            }

            W.Show();

            W.Activate();
        }
    }




    public class GitHubRelease
    {
        public string url { get; set; }
        public string tag_name { get; set; }
        public string name { get; set; }
        public DateTime created_at { get; set; }
        public List<GitHubAsset> assets { get; set; }
        public string body { get; set; }
    }

    public class GitHubAsset
    {
        public string name { get; set; }
        public string browser_download_url { get; set; }
    }
}
