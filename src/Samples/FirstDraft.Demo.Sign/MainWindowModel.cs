using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FirstDraft.Demo.Sign
{
    public class MainWindowModel : ObservableObject
    {
        /// <summary>
        /// 所有的导航页面
        /// </summary>
        private readonly List<NaviItem> NaviItems;
        public string WindowsTitle { get; set; }
        readonly NaviItem sign;

        public MainWindowModel()
        {
            var iconset = App.Current.Services.GetService<IconSet>();
            NaviItems = new List<NaviItem>();
            if (iconset != null)
            {
                NaviItems.Add(new NaviItem() { Title = "首页", Icon = iconset.home_fill, Content = new Views.HomeView() });
                NaviItems.Add(new NaviItem() { Title = "测试", Icon = iconset.tags_fill, Content = new Views.TestView() });
            }

            sign = new NaviItem() { Title = "登录", Icon = iconset.home_fill, Content = new Views.SignView() };

            Current = sign;
            Version version = System.Reflection.Assembly.GetAssembly(typeof(FirstDraft.Controls.IconButton)).GetName().Version;

            WindowsTitle = $"sign in/up {version.ToString()} 缓慢而坚定地前进";

            Items = new ObservableCollection<NaviItem>(NaviItems);
        }


        public RelayCommand GoSignCommand => new RelayCommand(() =>
        {
            Current = sign;
        });

        public RelayCommand GoHomeCommand => new RelayCommand(() =>
        {
            Current = NaviItems[0];
        });


        private NaviItem current;
        /// <summary>
        /// 当前页面
        /// </summary>
        public NaviItem Current
        {
            get { return current; }
            set { SetProperty(ref current, value); }
        }

        public ObservableCollection<NaviItem> Items { get; set; } = new ObservableCollection<NaviItem>();
    }

    public class NaviItem : ObservableObject
    {

        private bool isSelected;

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                SetProperty(ref isSelected, value);
            }
        }

        public string Icon { get; set; }

        public string Title { get; set; }

        public string ToolTip { get; set; }

        public FrameworkElement Content { get; set; }
    }
}
