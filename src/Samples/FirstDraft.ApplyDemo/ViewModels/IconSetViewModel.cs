using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;

namespace FirstDraft.ApplyDemo.ViewModels
{
    public class IconSetViewModel : ObservableObject
    {

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public IconSetViewModel()
        {
            SearchCommand = new RelayCommand(() =>
            {
                MessageBox.Show(SearchText);
            });

            ChooseCommand = new RelayCommand<IconModel>(t =>
            {
                SelectedIcon = t;
            });



            // 初始化数据源
            var list = FindDititsInfo(App.Current.Services.GetService<IconSet>());
            list.AddRange(FindDititsInfo(new XiaoJuziIconSet()));
            OriginalIcons = list;

            // 初始化界面
            SearchText = "";
        }

        /// <summary>
        /// 原始数据
        /// </summary>
        IEnumerable<IconModel> OriginalIcons;


        public Boolean HasText => !string.IsNullOrEmpty(searchText);

        public RelayCommand ClearTextCommand => new RelayCommand(() => SearchText = "");


        private string searchText;

        /// <summary>
        /// 当前搜索文本
        /// </summary>
        public string SearchText
        {
            get { return searchText; }
            set
            {
                searchText = value;
                OnPropertyChanged(nameof(SearchText));
                OnPropertyChanged(nameof(HasText));
                OnSearchTextChanged();
            }
        }

        /// <summary>
        /// 当搜索文本发生变化时，修改搜索匹配的图标集合
        /// </summary>
        private void OnSearchTextChanged()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                SearchedIcons = new ObservableCollection<IconModel>(OriginalIcons);
            }
            else
            {
                // 根据输入字符，检索原始数据中匹配的图标，并更新界面的绑定数据源
                SearchedIcons = new ObservableCollection<IconModel>(OriginalIcons.Where(t => t.IconName.ToLowerInvariant().Contains(SearchText.ToLowerInvariant())));
            }

            UpdateView();
        }


        private ObservableCollection<IconModel> searchedIcons;

        /// <summary>
        /// 当前搜索匹配的图标集合
        /// </summary>
        public ObservableCollection<IconModel> SearchedIcons

        {
            get { return searchedIcons; }
            set
            {
                SetProperty(ref searchedIcons, value);
            }
        }

        private IconModel selectedIcon;

        /// <summary>
        /// 当前选中的图标
        /// </summary>
        public IconModel SelectedIcon
        {
            get { return selectedIcon; }
            set { selectedIcon = value; OnPropertyChanged(); }
        }


        /// <summary>
        /// 搜索命令
        /// </summary>
        public RelayCommand SearchCommand { get; private set; }


        /// <summary>
        /// 选中命令
        /// </summary>
        public RelayCommand<IconModel> ChooseCommand { get; private set; }


        public RelayCommand ViewModuleCommand => new RelayCommand(() =>
        {
            UpdateView(40, true);
        });


        #region 更新界面大小



        private Boolean isViewModule = true;
        public Boolean IsViewModule
        {
            get { return isViewModule; }
            set
            {
                SetProperty(ref isViewModule, value);
            }
        }

        private Boolean isViewCompact = true;
        public Boolean IsViewCompact
        {
            get { return isViewCompact; }
            set
            {
                SetProperty(ref isViewCompact, value);
            }
        }

        private Boolean isViewComfy;
        public Boolean IsViewComfy
        {
            get { return isViewComfy; }
            set
            {
                SetProperty(ref isViewComfy, value);
            }
        }

        public RelayCommand UpdateViewCommand => new RelayCommand(UpdateView);

        private void UpdateView()
        {
            if (isViewModule)
            {
                UpdateView(32, true);
            }
            if (isViewCompact)
            {
                UpdateView(16, true);
            }
            if (isViewComfy)
            {
                UpdateView(12, false);
            }
        }

        private void UpdateView(int iconSize, Boolean showName)
        {
            foreach (var item in SearchedIcons)
            {
                item.IconSize = iconSize;
                item.ShowName = showName;
            }
        }



        #endregion



        /// <summary>
        /// 获取 model 对象中的属性信息
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="model">对象</param>
        /// <returns></returns>
        List<IconModel> FindDititsInfo<T>(T model)
        {
            List<IconModel> icons = new List<IconModel>();
            var newType = model.GetType();
            foreach (var item in newType.GetRuntimeProperties())
            {
                IconModel icon = new IconModel();
                icon.IconName = item.Name;
                icon.IconType = newType.Name;
                icon.IconData = item.GetValue(model).ToString();
                icons.Add(icon);
            }
            return icons;
        }

    }

    public class IconModel : ObservableObject
    {
        public string IconName { get; set; }
        public string IconType { get; set; }

        public string IconData { get; set; }

        private int iconSize;
        public int IconSize
        {
            get { return iconSize; }
            set { SetProperty(ref iconSize, value); }
        }

        private Boolean showName;
        public Boolean ShowName
        {
            get { return showName; }
            set { SetProperty(ref showName, value); }
        }

    }
}
