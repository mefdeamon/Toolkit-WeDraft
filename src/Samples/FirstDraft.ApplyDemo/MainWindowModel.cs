using FirstDraft;
using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using CommunityToolkit.Mvvm.Input;

namespace FirstDraft.ApplyDemo
{
    internal class MainWindowModel : ObservableObject
    {

        public string WindowsTitle { get; set; }



        public MainWindowModel()
        {
            var iconSet = App.Current.Services.GetService<IconSet>();
            NaviItems = new List<NaviItem>();
            NaviItems.Add(new NaviItem() { Title = "欢迎 FirstDraft(F D)", Icon =iconSet.welcome, Content = new Views.WelcomeView() });
            NaviItems.Add(new NaviItem() { Title = "按钮(Button)", Icon =iconSet.rect_fill, Content = new Views.ApplyButtonView() });
            NaviItems.Add(new NaviItem() { Title = "切换按钮(ToggleButton)", Icon =iconSet.togglebutton, Content = new Views.ApplyToggleButtonView() });
            NaviItems.Add(new NaviItem() { Title = "单选按钮(RadioButton)", Icon =iconSet.radiobutton, Content = new Views.ApplyRadioButtonView() });
            NaviItems.Add(new NaviItem() { Title = "复选框(CheckBox)", Icon =iconSet.checkbox_fill, Content = new Views.ApplyCheckBoxView() });
            NaviItems.Add(new NaviItem() { Title = "文本框(TextBox)", Icon =iconSet.textbox, Content = new Views.ApplyTextBoxView() });
            NaviItems.Add(new NaviItem() { Title = "下拉选项框(ComboBox)", Icon =iconSet.combobox, Content = new Views.ApplyComboBoxView() });
            NaviItems.Add(new NaviItem() { Title = "列表框(ListBox)", Icon =iconSet.list, Content = new Views.ApplyListBoxView() });
            NaviItems.Add(new NaviItem() { Title = "表格(DataGrid)", Icon =iconSet.datagrid, Content = new Views.ApplyDataGridView() });
            NaviItems.Add(new NaviItem() { Title = "日期(DatePicker)", Icon =iconSet.datepicker, Content = new Views.ApplyDatePickerView() });
            NaviItems.Add(new NaviItem() { Title = "滑动条(Slider)", Icon =iconSet.slider_fill, Content = new Views.ApplySliderView() });
            NaviItems.Add(new NaviItem() { Title = "提示框(ToolTip)", Icon =iconSet.tooltip, Content = new Views.ApplyToolTipView() });
            NaviItems.Add(new NaviItem() { Title = "工具栏(ToolBar)", Icon =iconSet.toolbar, Content = new Views.ApplyToolBarView() });
            NaviItems.Add(new NaviItem() { Title = "密码框(PasswordBox)", Icon =iconSet.passwrod_fill, Content = new Views.ApplyPasswordBoxView() });
            NaviItems.Add(new NaviItem() { Title = "加载动画(ContentControl)", Icon =iconSet.animate, Content = new Views.ApplyLoadingAnimateControlView() });
            NaviItems.Add(new NaviItem() { Title = "图标(IconControl)", Icon =iconSet.tags_fill, Content = new Views.ApplyIconControlView() });
            NaviItems.Add(new NaviItem() { Title = "图标按钮(IconButton)", Icon =iconSet.rect_fill, Content = new Views.ApplyIconButtonView() });
            NaviItems.Add(new NaviItem() { Title = "图标切换按钮(IconToggleButton)", Icon =iconSet.togglebutton, Content = new Views.ApplyIconToggleButtonView() });
            NaviItems.Add(new NaviItem() { Title = "图标单选按钮(IconRadioButton)", Icon =iconSet.radiobutton, Content = new Views.ApplyIconRadioButtonView() });
            NaviItems.Add(new NaviItem() { Title = "图标重复按钮(IconRepeatButton)", Icon =iconSet.rect_fill, Content = new Views.ApplyIconRepeatButtonView() });
            NaviItems.Add(new NaviItem() { Title = "图标文本框(IconTextBox)", Icon =iconSet.textbox, Content = new Views.ApplyIconTextBoxView() });
            NaviItems.Add(new NaviItem() { Title = "IP地址框(IpAddressBox)", Icon =iconSet.ipaddressbox, Content = new Views.ApplyIpAddressBoxView() });
            NaviItems.Add(new NaviItem() { Title = "日期时间选择框(DateTimePickBox)", Icon =iconSet.datetimepickbox, Content = new Views.ApplyDateTimePickBoxView() });
            NaviItems.Add(new NaviItem() { Title = "图标集(IconSet)", Icon =iconSet.tags_fill, Content = new Views.IconSetView() });

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
            set { SetProperty(ref current, value); }
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
                SetProperty(ref items, value);
            }
        }


        public RelayCommand OpenRewardCommand => new RelayCommand(() =>
        {
            RewardWindow.ShowInstance();
        });
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
