using FirstDraft.Controls;

namespace FirstDraft.ApplyDemo.Views
{
    /// <summary>
    /// Interaction logic for ApplyButtonView.xaml
    /// </summary>
    public partial class ApplyButtonView : AnimatableBaseView
    {
        public ApplyButtonView()
        {
            InitializeComponent();
            LoadedAnimateDirection = AttachedProperties.AnimationDirection.FromRight;
        }
    }
}
