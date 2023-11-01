using FirstDraft.Controls;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FirstDraft.ApplyDemo.Views
{
    /// <summary>
    /// Interaction logic for ApplyCheckBoxView.xaml
    /// </summary>
    public partial class ApplyCheckBoxView : AnimatableUserControl
    {
        public ApplyCheckBoxView()
        {
            InitializeComponent();
            LoadedAnimateDirection = AttachedProperties.AnimationDirection.Bottom;
        }
    }
}
