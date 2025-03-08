using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FirstDraft.ApplyDemo.Parsec.ViewModels
{
    public class SettingsViewModel : ObservableObject
    {
        public SettingsViewModel()
        {
            InitNavi();
        }

        #region 导航

        /// <summary>
        /// 所有的导航页面
        /// </summary>
        private readonly Dictionary<string, NaviItem> _NaviItems = new Dictionary<string, NaviItem>();

        private void InitNavi()
        {
            AddNavi(new NaviItem(typeof(Views.Settings.ClientView), "Client", ""));
            AddNavi(new NaviItem(typeof(Views.Settings.HostView), "Host", ""));
            AddNavi(new NaviItem(typeof(Views.Settings.ApprovedAppsView), "Approved Apps", ""));
            AddNavi(new NaviItem(typeof(Views.Settings.NetworkHotkeysView), "Network Hotkeys", ""));
            AddNavi(new NaviItem(typeof(Views.Settings.GamepadView), "Gamepad", ""));
            AddNavi(new NaviItem(typeof(Views.Settings.ExperimentalView), "Experimental", ""));
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
}
