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
    /// DateTimePickBox.xaml 的交互逻辑
    /// </summary>
    public partial class DateTimePickBox : UserControl
    {
        public DateTimePickBox()
        {
            InitializeComponent();
        }

        private void sumbit_Click(object sender, RoutedEventArgs e)
        {
            popup.IsOpen = false;
        }

        private void copy_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                try
                {
                    Clipboard.SetText(PART_TIME.Content as string);
                }
                catch (Exception)
                {
                }
            }));
        }
    }

    public class DateTimePickerDataContext : NotifyPropertyChanged
    {
        public DateTimePickerDataContext()
        {
            TimeDc = new TimePickerDataContext();
            TimeDc.TimeChanged += TimeDc_TimeChanged; ;
        }

        private void TimeDc_TimeChanged()
        {
            RaisePropertyChanged(nameof(DateTimeText));
        }

        public TimePickerDataContext TimeDc { get; set; }


        private DateTime dcDateTime = DateTime.Now;

        public DateTime DcDateTime
        {
            get { return dcDateTime; }
            set
            {
                dcDateTime = value;
                RaisePropertyChanged(nameof(DcDateTime));
                RaisePropertyChanged(nameof(DateTimeText));
            }
        }

        public void SetDateTime(DateTime time)
        {
            DcDateTime = time;
            TimeDc.SetTime(time.ToString("HH:mm:ss"));
        }

        public String DateTimeText => DcDateTime.ToString("yyyy-MM-dd") + " " + TimeDc.TimeText;


        public DateTime DateTime => DateTime.Parse(DateTimeText);

    }

}
