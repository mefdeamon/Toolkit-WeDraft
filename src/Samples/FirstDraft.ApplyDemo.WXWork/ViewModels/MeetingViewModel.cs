using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstDraft.ApplyDemo.WXWork.ViewModels
{
    public class MeetingViewModel : ObservableObject
    {
        public MeetingViewModel()
        {
            InitNavi();


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
            AddNavi(new NaviItem(typeof(Views.Meeting.MeetingHomeView), "主页", App.Current.Services.GetService<IconSet>().home_fill));
            AddNavi(new NaviItem(typeof(Views.Meeting.MeetingBookView), "预定", App.Current.Services.GetService<IconSet>().home_fill));
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

    }
}
