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
using System.Windows.Shapes;

namespace FirstDraft.Controls
{
    /// <summary>
    /// FdMessageBox.xaml 的交互逻辑
    /// </summary>
    public partial class FdMessageBox : FdWindow
    {
        public FdMessageBox()
        {
            InitializeComponent();
        }

        public static bool Ask(string msg, string title = "", string icon = "", WindowStartupLocation location = WindowStartupLocation.CenterScreen, Window owner = null)
        {
            FdMessageBox fdbox = new FdMessageBox();
            fdbox.textMessage.Text = msg;
            fdbox.WindowStartupLocation = location;
            fdbox.ask.Visibility = Visibility.Visible;
            if (owner != null)
            {
                fdbox.Owner = owner;
            }

            if (!string.IsNullOrEmpty(title))
            {
                fdbox.textTitle.Text = title;
            }
            if (!string.IsNullOrEmpty(icon))
            {
                fdbox.icon.Icon = StreamGeometry.Parse(icon);
                fdbox.icon.Visibility = Visibility.Visible;
            }
            else
            {
                fdbox.icon.Visibility = Visibility.Collapsed;
            }


            return fdbox.ShowDialog() == true;
        }

        public static bool Show(string msg, string title = "", string icon = "", WindowStartupLocation location = WindowStartupLocation.CenterScreen, Window owner = null)
        {
            bool result = false;
            Application.Current.Dispatcher.Invoke(() =>
            {
                FdMessageBox fdbox = new FdMessageBox();
                fdbox.textMessage.Text = msg;
                fdbox.WindowStartupLocation = location;
                fdbox.info.Visibility = Visibility.Visible;
                fdbox.buttonNO.Visibility = Visibility.Collapsed;
                if (owner != null)
                {
                    fdbox.Owner = owner;
                }
                if (!string.IsNullOrEmpty(title))
                {
                    fdbox.textTitle.Text = title;
                }
                if (!string.IsNullOrEmpty(icon))
                {
                    fdbox.icon.Icon = StreamGeometry.Parse(icon);
                    fdbox.icon.Visibility = Visibility.Visible;
                }
                else
                {
                    fdbox.icon.Visibility = Visibility.Collapsed;
                }


                result = fdbox.ShowDialog() == true;
            });
            return result;
        }

        public static bool Warn(string msg, string title = "", string icon = "", WindowStartupLocation location = WindowStartupLocation.CenterScreen, Window owner = null)
        {
            bool result = false;
            Application.Current.Dispatcher.Invoke(() =>
            {
                FdMessageBox fdbox = new FdMessageBox();
                fdbox.textMessage.Text = msg;
                fdbox.WindowStartupLocation = location;
                fdbox.warn.Visibility = Visibility.Visible;
                fdbox.buttonNO.Visibility = Visibility.Collapsed;
                if (owner != null)
                {
                    fdbox.Owner = owner;
                }
                if (!string.IsNullOrEmpty(title))
                {
                    fdbox.textTitle.Text = title;
                }
                if (!string.IsNullOrEmpty(icon))
                {
                    fdbox.icon.Icon = StreamGeometry.Parse(icon);
                    fdbox.icon.Visibility = Visibility.Visible;
                }
                else
                {
                    fdbox.icon.Visibility = Visibility.Collapsed;
                }


                result = fdbox.ShowDialog() == true;
            });
            return result;
        }


        bool result = false;

        private void Yes_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void No_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
