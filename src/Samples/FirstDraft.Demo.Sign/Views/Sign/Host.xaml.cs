using FirstDraft.Controls;
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

namespace FirstDraft.Demo.Sign.Views.Sign
{
    /// <summary>
    /// Host.xaml 的交互逻辑
    /// </summary>
    public partial class Host : UserControl
    {
        public Host()
        {
            InitializeComponent();
        }
        public FrameworkElement CurrentViewContent
        {
            get { return (FrameworkElement)GetValue(CurrentViewContentProperty); }
            set { SetValue(CurrentViewContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentViewContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentViewContentProperty =
            DependencyProperty.Register("CurrentViewContent", typeof(FrameworkElement), typeof(Host), new UIPropertyMetadata(default(FrameworkElement), null, CurrentViewPropertyChanged));

        private static object CurrentViewPropertyChanged(DependencyObject d, object baseValue)
        {
            var oldFrame = (d as Host).OldFrame;
            var newFrame = (d as Host).NewFrame;

            var oldFrameContent = oldFrame.Content;
            var newFrameContent = newFrame.Content;

            if (newFrameContent is AnimatableUserControl animatableBaseView)
            {
                oldFrame.Content = newFrameContent;
                newFrame.Content = null;
            }
            // First time
            else
            {
                newFrame.Content = baseValue;
                return baseValue;
            }

            Task.Delay((int)(300)).ContinueWith((t) =>
            {
                // set new content
                Application.Current.Dispatcher.Invoke(() => newFrame.Content = baseValue);
            });

            return baseValue;
        }
    }
}
