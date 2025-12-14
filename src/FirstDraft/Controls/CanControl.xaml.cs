using FirstDraft.Converters;
using FirstDraft.Mvvms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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
    /// CanControl.xaml 的交互逻辑
    /// </summary>
    public partial class CanControl : UserControl
    {
        public CanControl()
        {
            InitializeComponent();
        }

        public static CanSource CanSource { get; set; } = new CanSource();
        public static ChannelConfig Default { get; set; } = new ChannelConfig();
    }


    /// <summary>
    /// CAN 设备通道配置信息
    /// </summary>
    public class ChannelConfig
    {
        /// <summary>
        /// CAN 设备通道唯一 ID
        /// </summary>
        public UInt32 Id { get; set; }

        /// <summary>
        /// CAN 设备类型，具体见 CanCategory 枚举
        /// </summary>
        public CanCategory Categroy { get; set; } = CanCategory.USBCAN_2;


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
        public ChannelBaudRate BaudRate { get; set; } = ChannelBaudRate._1000Kbps;

        /// <summary>
        /// 数据波特率
        /// </summary>
        public ChannelDataBaudRate DataBaudRate { get; set; } = ChannelDataBaudRate.None;

        /// <summary>
        /// 启用CANFD
        /// </summary>
        public Boolean EnableCANFD { get; set; } = false;

        /// <summary>
        /// 启用终端电阻
        /// </summary>
        public Boolean EnableInternalResistance { get; set; } = false;

        public bool Equals(ChannelConfig cc)
        {
            return this.Categroy == cc.Categroy && this.CanIndex == cc.CanIndex && this.ChannelIndex == cc.ChannelIndex;
        }

    }

    public class CanSource
    {
        public List<CanCategory> CanCategories { get; } = new List<CanCategory>();
        public List<ChannelBaudRate> BaudRates { get; } = new List<ChannelBaudRate>();
        public List<byte> ChannelIndexes { get; } = new List<byte>();
        public ObservableCollection<byte> CanIndexes { get; } = new ObservableCollection<byte>();
        public List<ChannelDataBaudRate> DataBaudRates { get; } = new List<ChannelDataBaudRate>();
        public CanSource()
        {
            foreach (CanCategory item in Enum.GetValues(typeof(CanCategory)))
            {
                CanCategories.Add(item);
            }
            foreach (ChannelBaudRate item in Enum.GetValues(typeof(ChannelBaudRate)))
            {
                BaudRates.Add(item);
            }
            for (byte i = 0; i < 16; i++)
            {
                CanIndexes.Add(i);
            }

            for (byte i = 0; i < 4; i++)
            {
                ChannelIndexes.Add(i);
            }

            foreach (ChannelDataBaudRate item in Enum.GetValues(typeof(ChannelDataBaudRate)))
            {
                DataBaudRates.Add(item);
            }

        }
    }



    #region 枚举定义
    public enum CanCategory
    {
        /// <summary>
        /// 周立功厂商
        /// </summary>
        USBCAN_2 = 100,
        USBCANFD_200U = 101,
        USBCANFD_400U = 102,

        //ZLG_USBCAN_2 = 100,
        //ZLG_USBCANFD_200U = 101,
    }

    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum ChannelBaudRate : UInt32
    {
        [Description("10K")]
        _10Kbps = 10,
        [Description("50K")]
        _50Kbps = 50,
        [Description("100K")]
        _100Kbps = 100,
        [Description("125K")]
        _125Kbps = 125,
        [Description("250k")]
        _250Kbps = 250,
        [Description("500k")]
        _500Kbps = 500,
        [Description("1M")]
        _1000Kbps = 1000,
    };

    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum ChannelDataBaudRate : UInt32
    {
        [Description("None")]
        None = 0,
        [Description("1M")]
        _1000Kbps = 1000,
        [Description("2M")]
        _2000Kbps = 2000,
        [Description("4M")]
        _4000Kbps = 4000,
        [Description("5M")]
        _5000Kbps = 5000,
    }

    #endregion
}
