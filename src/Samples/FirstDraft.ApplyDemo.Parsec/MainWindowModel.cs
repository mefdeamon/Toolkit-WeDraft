using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows;

namespace FirstDraft.ApplyDemo.Parsec
{
    public class MainWindowModel : ObservableObject
    {

        public MainWindowModel()
        {
            InitNavi();
        }
        public WindowUiConfig UiConfig { get; set; } = new WindowUiConfig();


        #region 导航

        /// <summary>
        /// 所有的导航页面
        /// </summary>
        private readonly Dictionary<string, NaviItem> _NaviItems = new Dictionary<string, NaviItem>();

        private void InitNavi()
        {
            AddNavi(new NaviItem(typeof(Views.ComputersView), "Computers", "M876.544 547.84l3.072 0-731.136 0 0-434.176q0-43.008 21.504-66.048t65.536-23.04l558.08 0q46.08 0 65.024 26.112t17.92 67.072l0 430.08zM800.768 96.256l-579.584 0 0 323.584 579.584 1.024 0-324.608zM563.2 484.352q0-14.336-19.456-16.384l-62.464 0q-19.456 0-19.456 16.384 0 5.12 5.12 10.24t13.312 6.144l61.44-1.024q9.216 0 15.36-5.12t6.144-10.24zM859.136 606.208q11.264 0 15.36 7.168t12.288 20.48q4.096 7.168 15.36 25.6t24.064 39.424 24.064 39.424 15.36 24.576q8.192 13.312 13.824 25.088t-7.68 28.16q-26.624 31.744-46.08 38.912t-29.696 7.168l-38.912 0-89.088 0-121.856 0-138.24 0-137.216 0q-67.584 0-121.344-0.512t-87.552-0.512l-38.912 0q-8.192 0-16.896-4.096t-17.92-10.752-16.896-14.336-13.824-14.848q-7.168-9.216-9.216-16.384t-0.512-12.8 5.632-10.752 8.192-12.288q4.096-6.144 13.824-22.016t20.992-35.328l24.576-38.912q11.264-19.456 19.456-32.768t13.824-19.968 11.776-8.704 13.312-1.536 18.432 0.512l661.504 0zM909.312 795.648q0-7.168-6.144-11.776t-13.312-4.608l-747.52 0q-7.168 0-14.336 4.608t-7.168 11.776 6.144 12.288 13.312 5.12l748.544 0q7.168 0 13.824-5.12t6.656-12.288z"));
            AddNavi(new NaviItem(typeof(Views.SettingsView), "Settings", "M926.666667 425.886667a85.006667 85.006667 0 0 1-79.593334-52.7l-0.253333-0.606667a85 85 0 0 1 18.86-93.573333c19.726667-19.46 20.986667-51.273333 2.873333-72.42a468.32 468.32 0 0 0-52.466666-52.24 53.333333 53.333333 0 0 0-72.48 3.333333 85.366667 85.366667 0 0 1-146.153334-60.506667c0.186667-27.686667-21.38-51.08-49.1-53.253333a468.506667 468.506667 0 0 0-74 0.08c-27.753333 2.24-49.28 25.7-49.013333 53.4a85 85 0 0 1-52.56 79.653333l-0.126667 0.053334a85.033333 85.033333 0 0 1-93.486666-18.953334c-19.433333-19.726667-51.226667-21.006667-72.38-2.913333a467.96 467.96 0 0 0-52.246667 52.42c-18 21.186667-16.626667 52.973333 3.126667 72.366667a85.333333 85.333333 0 0 1-27.066667 139.793333 84.993333 84.993333 0 0 1-33.04 6.52 53.513333 53.513333 0 0 0-36.32 13.946667A52.913333 52.913333 0 0 0 44 475.366667a468.386667 468.386667 0 0 0 0.04 74.033333c2.226667 27.76 25.68 49.293333 53.38 49.033333a85.366667 85.366667 0 0 1 60.7 146.186667c-19.74 19.446667-21.033333 51.253333-2.94 72.42a468.1 468.1 0 0 0 52.373333 52.246667 53.486667 53.486667 0 0 0 55.026667 8.606666 52.346667 52.346667 0 0 0 17.333333-11.7 85.366667 85.366667 0 0 1 146.266667 60.466667c-0.213333 27.693333 21.333333 51.1 49.053333 53.333333a468.426667 468.426667 0 0 0 74 0 53.013333 53.013333 0 0 0 16.1-3.866666c19.553333-8.1 33.16-27.446667 32.96-49.493334a84.993333 84.993333 0 0 1 52.666667-79.613333h0.04a84.993333 84.993333 0 0 1 93.513333 18.993333c19.433333 19.746667 51.246667 21.06 72.42 2.973334a467.533333 467.533333 0 0 0 52.266667-52.353334c18-21.166667 16.666667-52.953333-3.06-72.366666a84.993333 84.993333 0 0 1-19.033333-93.6c0.053333-0.12 0.1-0.246667 0.153333-0.366667A85.006667 85.006667 0 0 1 926.666667 598c27.686667 0.226667 51.106667-21.333333 53.333333-49.026667a467.633333 467.633333 0 0 0 0-74c-2.193333-27.766667-25.62-49.326667-53.333333-49.086666z m-296.4 37.106666c27.013333 65.206667-4.066667 140.226667-69.273334 167.24s-140.226667-4.066667-167.24-69.273333 4.066667-140.233333 69.273334-167.24 140.24 4.066667 167.246666 69.273333z"));
            Items = new List<NaviItem>(_NaviItems.Values.ToList());
            Current = _NaviItems.Values.FirstOrDefault();
        }

        private void AddNavi(NaviItem naviItem) => _NaviItems.Add(naviItem.NaviId, naviItem);

        private NaviItem? current = null;
        /// <summary>
        /// 当前页面
        /// </summary>
        public NaviItem? Current
        {
            get { return current; }
            set { SetProperty(ref current, value); }
        }

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

        public List<NaviItem> Items { get; set; } = new List<NaviItem>();

        #endregion

    }


    public class WindowUiConfig : ObservableObject
    {
        public WindowUiConfig()
        {
            Sources = new Dictionary<string, Uri>();
            //Sources.Add("明亮", new Uri("/FirstDraft;component/Themes/Ui.Light.xaml", UriKind.Relative));
            //Sources.Add("幽暗", new Uri("/FirstDraft;component/Themes/Ui.Dark.xaml", UriKind.Relative));
            Sources.Add("明亮", new Uri("/Ui.Light.xaml", UriKind.Relative));
            Sources.Add("幽暗", new Uri("/Ui.Dark.xaml", UriKind.Relative));
            Theme = Sources["幽暗"];
        }

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
        private Boolean isDarkTheme = true;

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
    }

    public class NaviItem : ObservableObject
    {
        public string NaviId { get; set; }
        public Type NaviType { get; set; }

        public NaviItem(Type type, string title, string icon)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (type.FullName == null) throw new ArgumentNullException("type.FullName");
            var obj = Activator.CreateInstance(type);
            if (obj != null && obj is FrameworkElement fe)
            {
                this.Content = fe;
            }
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

        public FrameworkElement? Content { get; set; }


        public RelayCommand GoCommand => new RelayCommand(() =>
        {
            App.Current.Services.GetService<MainWindowModel>()?.Goto(NaviId);
        });
    }
}
