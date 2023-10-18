using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FirstDraft;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FirstDraft.ApplyDemo.WXWork
{

    public class MainWindowModel : ObservableObject
    {

        public string WindowsTitle { get; set; }


        public MainWindowModel()
        {
            InitNavi();

            WindowsTitle = $"缓慢而坚定地前进";
            SearchText = "";

            Task.Run(() =>
            {
                _NaviItems.Values.ToList().ForEach(t => t.InitContent());
                Current = _NaviItems.Values.FirstOrDefault();
            });
        }

        #region 导航

        /// <summary>
        /// 所有的导航页面
        /// </summary>
        private Dictionary<string, NaviItem> _NaviItems = new Dictionary<string, NaviItem>();

        private void InitNavi()
        {
            //AddNavi(new NaviItem(typeof(Views.MessagesView), "Messages", App.Current.Services.GetService<IconSet>().home_fill));
            //AddNavi(new NaviItem(typeof(Views.EmailsView), "Emails", App.Current.Services.GetService<IconSet>().home_fill));
            //AddNavi(new NaviItem(typeof(Views.DocumentView), "Document", App.Current.Services.GetService<IconSet>().home_fill));
            //AddNavi(new NaviItem(typeof(Views.CalenderView), "Calender", App.Current.Services.GetService<IconSet>().home_fill));
            //AddNavi(new NaviItem(typeof(Views.MeetingView), "Meeting", App.Current.Services.GetService<IconSet>().home_fill));
            //AddNavi(new NaviItem(typeof(Views.WorkspaceView), "Workspace", App.Current.Services.GetService<IconSet>().home_fill));
            //AddNavi(new NaviItem(typeof(Views.ContactsView), "Contacts", App.Current.Services.GetService<IconSet>().home_fill));


            AddNavi(new NaviItem(typeof(Views.MessagesView), "消息", App.Current.Services.GetService<IconSet>().home_fill));
            AddNavi(new NaviItem(typeof(Views.EmailsView), "邮件", App.Current.Services.GetService<IconSet>().email_fill));
            AddNavi(new NaviItem(typeof(Views.DocumentView), "文档", App.Current.Services.GetService<IconSet>().file_fill));
            AddNavi(new NaviItem(typeof(Views.CalenderView), "日程", App.Current.Services.GetService<IconSet>().overview_fill));
            AddNavi(new NaviItem(typeof(Views.MeetingView), "会议", App.Current.Services.GetService<IconSet>().volume));
            AddNavi(new NaviItem(typeof(Views.WorkspaceView), "工作台", App.Current.Services.GetService<IconSet>().all_fill));
            AddNavi(new NaviItem(typeof(Views.ContactsView), "通信录", App.Current.Services.GetService<IconSet>().cube_fill));
        }

        private void AddNavi(NaviItem naviItem)
        {
            _NaviItems.Add(naviItem.NaviId, naviItem);
        }

        private NaviItem current;
        /// <summary>
        /// 当前页面
        /// </summary>
        public NaviItem Current
        {
            get { return current; }
            set { SetProperty(ref current, value); }
        } 
        #endregion

        #region 检索范围

        private string searchText;

        /// <summary>
        /// 检索匹配
        /// </summary>
        public string SearchText
        {
            get { return searchText; }
            set
            {
                searchText = value;
                OnPropertyChanged(nameof(SearchText));
                OnSearchTextChanged();
            }
        }

        public RelayCommand SearchCommand => new RelayCommand(() =>
        {
            OnSearchTextChanged();
        });

        /// <summary>
        /// 当搜索文本发生变化时，修改搜索匹配的导航页
        /// </summary>
        private void OnSearchTextChanged()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                Items = new ObservableCollection<NaviItem>(_NaviItems.Values);
                return;
            }

            // 根据输入字符，检索原始数据中匹配的图标，并更新界面的绑定数据源
            Items = new ObservableCollection<NaviItem>(_NaviItems.Values.Where(t => t.Title.ToLowerInvariant().Contains(SearchText.ToLowerInvariant())));
        }


        private ObservableCollection<NaviItem> items;

        /// <summary>
        /// 当前搜索匹配的导航页
        /// </summary>
        public ObservableCollection<NaviItem> Items

        {
            get { return items; }
            set
            {
                SetProperty(ref items, value);
            }
        }

        #endregion

        public void Goto(string key)
        {
            if (_NaviItems.ContainsKey(key))
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Current = _NaviItems[key];
                });
            }
        }
    }


    public class NaviItem : ObservableObject
    {
        public NaviItem(FrameworkElement content, string title, string icon)
        {
            this.Content = content;
            this.Title = title;
            this.Icon = icon;
        }
        public string NaviId { get; set; }
        public Type NaviType { get; set; }

        public NaviItem(Type type, string title, string icon)
        {
            this.NaviType = type;
            this.NaviId = type.FullName;
            this.Title = title;
            this.Icon = icon;
        }

        private bool isSelected;

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                SetProperty(ref isSelected, value);
                if (isSelected)
                {
                    if (Content == null)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            Content = Activator.CreateInstance(NaviType) as FrameworkElement;
                        });
                    }
                }
            }
        }

        public void InitContent()
        {
            if (Content == null)
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Content = Activator.CreateInstance(NaviType) as FrameworkElement;
                });
        }

        public string Icon { get; set; }

        public string Title { get; set; }

        public string? ToolTip { get; set; }

        public FrameworkElement Content { get; set; }



        //public RelayCommand BackCommand => new RelayCommand(() =>
        //{
        //    App.Current.Services.GetService<MainWindowModel>().Goto(BackViewType.FullName);
        //});

        public RelayCommand GoCommand => new RelayCommand(() =>
        {
            App.Current.Services.GetService<MainWindowModel>().Goto(NaviId);
        });
    }

}
