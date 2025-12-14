using FirstDraft.Mvvms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// UartControl.xaml 的交互逻辑
    /// </summary>
    public partial class UartControl : UserControl
    {
        public UartControl()
        {
            InitializeComponent();
        }

        public static UartSource UartSource { get; set; } = new UartSource();
        public static UartConfig Default { get; set; } = new UartConfig();
    }


    public class UartConfig
    {
        public string PortName { get; set; } = "COM1";
        public int BaudRate { get; set; } = 9600;
        public byte DataBits { get; set; } = 8;
        public System.IO.Ports.StopBits StopBits { get; set; } = StopBits.One;
        public System.IO.Ports.Parity Parity { get; set; } = Parity.None;

        public bool RtsEnable { get; set; } = false;
        public bool DtrEnable { get; set; } = false;
        public System.IO.Ports.Handshake Handshake { get; set; } = Handshake.None;

        public int ReceivedBytesThreshold { get; set; } = 1;
        public int WriteBufferSize { get; set; } = 2048;
        public int ReadBufferSize { get; set; } = 4096;

        public UartConfig()
        {

        }
        public UartConfig(string portName, int baudRate,
            System.IO.Ports.Parity parity, byte dataBits, System.IO.Ports.StopBits stopBits,
            System.IO.Ports.Handshake handshake = Handshake.None,
            bool rts = false, bool dtr = false)
        {
            this.PortName = portName;
            this.BaudRate = baudRate;
            this.DataBits = dataBits;
            this.Parity = parity;
            this.StopBits = stopBits;
            this.Handshake = handshake;
            this.RtsEnable = rts;
            this.DtrEnable = dtr;
        }
    }


    public class UartSource
    {
        public Dictionary<Parity, string> Parities { get; set; } = new Dictionary<Parity, string>();
        public ObservableCollection<string> PortNames { get; set; } = new ObservableCollection<string>();

        public Dictionary<StopBits, string> StopBitses { get; set; } = new Dictionary<StopBits, string>();
        public Dictionary<Handshake, string> Handshakes { get; set; } = new Dictionary<Handshake, string>();

        public List<byte> DataBitses { get; set; } = new List<byte>();

        public List<int> BaudRates { get; set; }

        string GetHandshake(Handshake handshake)
        {
            switch (handshake)
            {
                case Handshake.XOnXOff:
                    return "XONXOFF";
                case Handshake.RequestToSend:
                    return "RTS";
                case Handshake.RequestToSendXOnXOff:
                    return "RTS/XONOFF";
                default:
                    break;
            }
            return handshake.ToString();
        }

        string GetParity(Parity parity)
        {
            if (CultureInfo.CurrentCulture.Name.StartsWith("zh"))
            {
                switch (parity)
                {
                    case Parity.None:
                        return "无校验";
                    case Parity.Odd:
                        return "奇校验";
                    case Parity.Even:
                        return "偶校验";
                    case Parity.Mark:
                        return "标记校验";
                    case Parity.Space:
                        return "空格校验";
                    default:
                        break;
                }
            }

            return parity.ToString();
        }

        string GetStopBits(StopBits stopBits)
        {
            switch (stopBits)
            {
                case StopBits.None:
                    return "0";
                case StopBits.One:
                    return "1";
                case StopBits.Two:
                    return "2";
                case StopBits.OnePointFive:
                    return "1.5";
                default:
                    break;
            }
            return "0";
        }

        public UartSource()
        {
            UpdatePortName();

            foreach (Parity item in Enum.GetValues(typeof(Parity)))
            {
                Parities.Add(item, GetParity(item));
            }
            foreach (StopBits item in Enum.GetValues(typeof(StopBits)))
            {
                StopBitses.Add(item, GetStopBits(item));
            }
            foreach (Handshake item in Enum.GetValues(typeof(Handshake)))
            {
                Handshakes.Add(item, GetHandshake(item));
            }

            for (byte i = 5; i <= 8; i++)
            {
                DataBitses.Add(i);

            }
            BaudRates = new List<int>
            {
                110, 300, 600, 1200, 2400, 4800, 9600,
                14400, 19200, 38400, 57600, 115200, 128000,
                230400, 256000,460800, 921600, 1000000, 2000000
            };

            UpdatePortNameCommand = new RelayCommand(UpdatePortName);
        }

        public RelayCommand UpdatePortNameCommand { get; private set; }

        public void UpdatePortName()
        {
            var org = PortNames.ToList();

            string[] ports = SerialPort.GetPortNames();

            foreach (var item in org)
            {
                if (!ports.Contains(item))
                {
                    PortNames.Remove(item);
                }
            }

            foreach (string port in ports)
            {
                if (!PortNames.Contains(port))
                    PortNames.Add(port);
            }
        }

    }
}
