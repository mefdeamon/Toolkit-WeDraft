using FirstDraft;
using MeiMvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace FirstDraft.ApplyDemo
{
    internal class MainWindowModel : NotifyPropertyChanged
    {

        public string WindowsTitle { get; set; }



        public MainWindowModel()
        {
            NaviItems = new List<NaviItem>();
            NaviItems.Add(new NaviItem() { Title = "欢迎 FirstDraft(F D)", Icon = ServiceProvider.Get<IconSet>().welcome, Content = new Views.WelcomeView() });
            NaviItems.Add(new NaviItem() { Title = "按钮(Button)", Icon = ServiceProvider.Get<IconSet>().rect_fill, Content = new Views.ApplyButtonView() });
            NaviItems.Add(new NaviItem() { Title = "图标按钮(IconButton)", Icon = ServiceProvider.Get<IconSet>().tag_fill, Content = new Views.ApplyIconButtonView() });
            NaviItems.Add(new NaviItem() { Title = "切换按钮(ToggleButton)", Icon = ServiceProvider.Get<IconSet>().togglebutton, Content = new Views.ApplyToggleButtonView() });
            NaviItems.Add(new NaviItem() { Title = "单选按钮(RadioButton)", Icon = ServiceProvider.Get<IconSet>().radiobutton, Content = new Views.ApplyRadioButtonView() });
            NaviItems.Add(new NaviItem() { Title = "复选框(CheckBox)", Icon = ServiceProvider.Get<IconSet>().checkbox_fill, Content = new Views.ApplyCheckBoxView() });
            NaviItems.Add(new NaviItem() { Title = "文本框(TextBox)", Icon = ServiceProvider.Get<IconSet>().textbox, Content = new Views.ApplyTextBoxView() });
            NaviItems.Add(new NaviItem() { Title = "下拉选项框(ComboBox)", Icon = ServiceProvider.Get<IconSet>().combobox, Content = new Views.ApplyComboBoxView() });
            NaviItems.Add(new NaviItem() { Title = "图标切换按钮(IconToggleButton)", Icon = ServiceProvider.Get<IconSet>().tag_fill, Content = new Views.ApplyIconToggleButtonView() });
            NaviItems.Add(new NaviItem() { Title = "图标单选按钮(IconRadioButton)", Icon = ServiceProvider.Get<IconSet>().tag_fill, Content = new Views.ApplyIconRadioButtonView() });
            NaviItems.Add(new NaviItem() { Title = "图标集(IconSet)", Icon = ServiceProvider.Get<IconSet>().tags_fill, Content = new Views.IconSetView() });

            Current = NaviItems[0];
            SearchText = "";
            Version version = System.Reflection.Assembly.GetAssembly(typeof(FirstDraft.Controls.IconButton)).GetName().Version;

            WindowsTitle = $"First Draft Apply {version.ToString()} 缓慢而坚定地前进";
        }

        /// <summary>
        /// 所有的导航页面
        /// </summary>
        private List<NaviItem> NaviItems;

        private NaviItem current;
        /// <summary>
        /// 当前页面
        /// </summary>
        public NaviItem Current
        {
            get { return current; }
            set { Set(ref current, value); }
        }

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
                RaisePropertyChanged(nameof(SearchText));
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
                Items = new ObservableCollection<NaviItem>(NaviItems);
                return;
            }

            // 根据输入字符，检索原始数据中匹配的图标，并更新界面的绑定数据源
            Items = new ObservableCollection<NaviItem>(NaviItems.Where(t => t.Title.ToLowerInvariant().Contains(SearchText.ToLowerInvariant())));
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
                Set(ref items, value);
            }
        }
    }


    public class NaviItem : NotifyPropertyChanged
    {

        private bool isSelected;

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                Set(ref isSelected, value);
            }
        }

        public string Icon { get; set; }

        public string Title { get; set; }

        public string ToolTip { get; set; }

        public FrameworkElement Content { get; set; }
    }


}
