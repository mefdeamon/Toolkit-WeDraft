using FirstDraft;
using MeiMvvm;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;

namespace FirstDraft.ApplyDemo
{
    internal class MainWindowModel : NotifyPropertyChanged
    {

        public string WindowsTitle { get; set; }

        public List<NaviItem> Items { get; set; }

        private NaviItem current;

        public NaviItem Current
        {
            get { return current; }
            set { Set(ref current, value); }
        }

        public MainWindowModel()
        {
            Items = new List<NaviItem>();
            Items.Add(new NaviItem() { Title = "欢迎 FirstDraft(F D)",  Icon = ServiceProvider.Get<IconSet>().welcome, Content = new Views.WelcomeView() });
            Items.Add(new NaviItem() { Title = "按钮(Button)", Icon = ServiceProvider.Get<IconSet>().rect_fill, Content = new Views.ApplyButtonView() });
            Items.Add(new NaviItem() { Title = "图标按钮(IconButton)", Icon = ServiceProvider.Get<IconSet>().tag_fill, Content = new Views.ApplyIconButtonView() });
            Items.Add(new NaviItem() { Title = "切换按钮(ToggleButton)", Icon = ServiceProvider.Get<IconSet>().togglebutton, Content = new Views.ApplyToggleButtonView() });
            Items.Add(new NaviItem() { Title = "单选按钮(RadioButton)", Icon = ServiceProvider.Get<IconSet>().radiobutton, Content = new Views.ApplyRadioButtonView() }); 
            Items.Add(new NaviItem() { Title = "复选框(CheckBox)", Icon = ServiceProvider.Get<IconSet>().checkbox_fill, Content = new Views.ApplyCheckBoxView() }); 
            Items.Add(new NaviItem() { Title = "文本框(TextBox)", Icon = ServiceProvider.Get<IconSet>().textbox, Content = new Views.ApplyTextBoxView() }); 
            Items.Add(new NaviItem() { Title = "下拉选项框(ComboBox)", Icon = ServiceProvider.Get<IconSet>().combobox, Content = new Views.ApplyComboBoxView() });
            Items.Add(new NaviItem() { Title = "图标集(IconSet)", Icon = ServiceProvider.Get<IconSet>().tags_fill, Content = new Views.IconSetView() });

            Current = Items[0];

            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

            WindowsTitle = $"First Draft Apply {version.ToString()}";
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
