using System;
using System.Collections.Generic;
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
    /// TimePickBox.xaml 的交互逻辑
    /// </summary>
    public partial class TimePickBox : UserControl
    {
        public TimePickBox()
        {
            InitializeComponent();
        }

        private void sumbit_Click(object sender, RoutedEventArgs e)
        {
            popup.IsOpen = false;
        }
    }

    public class TimePickerDataContext : NotifyPropertyChanged
    {
        public TimePickerDataContext()
        {
            Hours = new List<String>();
            for (Byte i = 0; i < 24; i++)
            {
                Hours.Add(i.ToString("00"));
            }
            Minutes = new List<String>();
            for (Byte i = 0; i < 60; i++)
            {
                Minutes.Add(i.ToString("00"));
            }
            Seconds = new List<String>();
            for (Byte i = 0; i < 60; i++)
            {
                Seconds.Add(i.ToString("00"));
            }
        }

        public List<String> Hours { get; set; }
        public List<String> Minutes { get; set; }
        public List<String> Seconds { get; set; }


        private String hour = "12";

        public String Hour
        {
            get { return hour; }
            set
            {
                hour = value;
                RaisePropertyChanged(nameof(Hour));
                RaisePropertyChanged(nameof(TimeText));
                TimeChanged?.Invoke();
            }
        }

        private String minute = "00";

        public String Minute
        {
            get { return minute; }
            set
            {
                minute = value;
                RaisePropertyChanged(nameof(Minute));
                RaisePropertyChanged(nameof(TimeText));
                TimeChanged?.Invoke();
            }
        }

        private String second = "00";

        public String Second
        {
            get { return second; }
            set
            {
                second = value;
                RaisePropertyChanged(nameof(Second));
                RaisePropertyChanged(nameof(TimeText));
                TimeChanged?.Invoke();
            }
        }

        public void SetTime(string time)
        {
            // TODO 对数据的检查
            var times = time.Trim().Split(':');
            if (times.Length == 3)
            {
                Hour = times[0];
                Minute = times[1];
                Second = times[2];
            }
        }

        public void SetTime(TimeSpan time)
        {
            // TODO 对数据的检查
            SetTime(time.ToString(@"hh\:mm\:ss"));
        }

        public String TimeText => Time.ToString(@"hh\:mm\:ss");

        /// <summary>
        /// 通知子类修改
        /// </summary>
        public event Action TimeChanged;

        public TimeSpan Time => new TimeSpan(int.Parse(hour), int.Parse(minute), int.Parse(second));

        public override string ToString()
        {
            return $"{hour}:{minute}:{second}";
        }

    }

}
