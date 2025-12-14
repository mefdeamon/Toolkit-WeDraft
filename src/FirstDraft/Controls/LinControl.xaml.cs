using FirstDraft.Converters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// LinControl.xaml 的交互逻辑
    /// </summary>
    public partial class LinControl : UserControl
    {
        public LinControl()
        {
            InitializeComponent();
        }

        public static LinSource LinSource { get; set; } = new LinSource();
        public static LinConfig Default { get; set; } = new LinConfig();

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is LinConfig channel)
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
            if (this.DataContext is LinConfig channel)
            {
                var c = new LChannelConfig();
                if (channel.Channels.Count > 0)
                {
                    c.Id = channel.Channels.Max(t => t.Id) + 1;
                }
                channel.Channels.Add(c);
            }
        }
    }


    public class LinConfig
    {
        public ObservableCollection<LChannelConfig> Channels { get; set; } = new ObservableCollection<LChannelConfig>();
    }


    /// <summary>
    /// CAN 设备通道配置信息
    /// </summary>
    public class LChannelConfig
    {
        /// <summary>
        /// CAN 设备通道唯一 ID
        /// </summary>
        public UInt32 Id { get; set; }

        /// <summary>
        /// CAN 设备类型，具体见 CanCategory 枚举
        /// </summary>
        public CanCategory Categroy { get; set; } = CanCategory.USBCANFD_200U;

        /// <summary>
        /// CAN 设备索引 0 1 2 3 4 ...
        /// </summary>
        public Byte CanIndex { get; set; }

        /// <summary>
        /// 通道索引 0 1 2 3 ...
        /// </summary>
        public Byte ChannelIndex { get; set; }

        /// <summary>
        /// 通道波特率
        /// </summary>
        public LinBaud LibBaud { get; set; } = LinBaud._19200bps;

        /// <summary>
        /// 启用终端电阻
        /// </summary>
        public Boolean EnableInternalResistance { get; set; } = false;


        /// <summary>
        /// True 
        /// </summary>
        public Boolean EnableMaster { get; set; } = false;

        /// <summary>
        ///  // 0-从站 1-主站
        /// </summary>
        public byte LinMode => EnableMaster ? (byte)1 : (byte)0;
        /// <summary>
        ///  // 1-经典校验 2-增强校验 3-自动
        /// </summary>
        public CheckSumMode CheckSumMode { get; set; } = CheckSumMode.Automatic;

        public bool Equals(ChannelConfig cc)
        {
            return this.Categroy == cc.Categroy && this.CanIndex == cc.CanIndex && this.ChannelIndex == cc.ChannelIndex;
        }

    }

    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum CheckSumMode : Byte
    {
        [Description("经典")]
        Classic = 1,
        [Description("增强")]
        Enhanced = 2,
        [Description("自动")]
        Automatic = 3
    }

    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum LinBaud : UInt32
    {
        [Description("1000bps")]
        _1000bps = 1000,
        [Description("1200bps")]
        _1200bps = 1200,
        [Description("2400bps")]
        _2400bps = 2400,
        [Description("4800bps")]
        _4800bps = 4800,
        [Description("9600bps")]
        _9600bps = 9600,
        [Description("10417bps")]
        _10417bps = 10417,
        [Description("19200bps")]
        _19200bps = 19200,
        [Description("20000bps")]
        _20000bps = 20000,
    };

    public class LinSource
    {
        public List<CanCategory> CanCategories { get; } = new List<CanCategory>();
        public List<LinBaud> LinBauds { get; } = new List<LinBaud>();
        public List<byte> ChannelIndexes { get; } = new List<byte>();
        public List<byte> CanIndexes { get; } = new List<byte>();
        public List<CheckSumMode> CheckSumModes { get; } = new List<CheckSumMode>();
        public LinSource()
        {
            foreach (CanCategory item in Enum.GetValues(typeof(CanCategory)))
            {
                CanCategories.Add(item);
            }
            foreach (LinBaud item in Enum.GetValues(typeof(LinBaud)))
            {
                LinBauds.Add(item);
            }
            for (byte i = 0; i < 16; i++)
            {
                CanIndexes.Add(i);
                if (i < 4)
                {
                    ChannelIndexes.Add(i);
                }
            }
            foreach (CheckSumMode item in Enum.GetValues(typeof(CheckSumMode)))
            {
                CheckSumModes.Add(item);
            }

        }
    }


}
