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
    /// Interaction logic for ApplyRadioButtonView.xaml
    /// </summary>
    public partial class ApplyRadioButtonView : Controls.AnimatableUserControl
    {
        public ApplyRadioButtonView()
        {
            InitializeComponent();
            LoadedAnimateDirection = AttachedProperties.AnimationDirection.Right;
            UnloadAnimateDirection = AttachedProperties.AnimationDirection.Left;

        }
    }
}
