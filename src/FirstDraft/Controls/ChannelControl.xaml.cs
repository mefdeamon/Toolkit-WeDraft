using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FirstDraft.Controls
{
    /// <summary>
    /// ChannelControl.xaml 的交互逻辑
    /// </summary>
    public partial class ChannelControl : UserControl
    {
        public ChannelControl()
        {
            InitializeComponent();
        }

        public static CanSource CanSource { get; set; } = new CanSource();
        public static CanConfig Default { get; set; } = new CanConfig() { Channels = new ObservableCollection<ChannelConfig>() { new ChannelConfig() } };

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is CanConfig channel)
            {
                if (sender is Button btn)
                {
                    if (int.TryParse(btn.Tag.ToString(), out var id))
                    {
                        var item = channel.Channels.Where(t => t.Id == id).FirstOrDefault();
                        if (item != null)
                        {
                            channel.Channels.Remove(item);
                        }
                    }
                }
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is CanConfig channel)
            {
                var c = new ChannelConfig();
                if (channel.Channels.Count > 0)
                {
                    c.Id = channel.Channels.Max(t => t.Id) + 1;
                }
                channel.Channels.Add(c);
            }
        }
    }

    public class CanConfig
    {
        public ObservableCollection<ChannelConfig> Channels { get; set; } = new ObservableCollection<ChannelConfig>();

    }

}
