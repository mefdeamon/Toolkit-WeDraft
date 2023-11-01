using FirstDraft.Controls;

namespace FirstDraft.ApplyDemo.Views
{
    /// <summary>
    /// Interaction logic for ApplyButtonView.xaml
    /// </summary>
    public partial class ApplyButtonView : AnimatableUserControl
    {
        public ApplyButtonView()
        {
            InitializeComponent();
            LoadedAnimateDirection = AttachedProperties.AnimationDirection.Top;
        }
    }
}
