using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FirstDraft.ApplyDemo
{
    /// <summary>
    /// RewardWindow.xaml 的交互逻辑
    /// </summary>
    public partial class RewardWindow : FdWindow
    {
        public RewardWindow()
        {
            InitializeComponent();
        }

        static RewardWindow _instance;

        public static void ShowInstance()
        {
            if (_instance == null)
            {
                _instance = new RewardWindow();
                _instance.Owner = Application.Current.MainWindow;
                _instance.Closing += (S, E) => _instance = null;
            }

            _instance.ShowAndFocus();
        }
    }
}
