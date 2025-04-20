using FirstDraft.AttachedProperties;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FirstDraft.Controls
{
    public class AnimatableUserControl : UserControl
    {

        #region 公共属性

        /// <summary>
        /// 加载页面动画方向 默认值为None表示只有淡出效果，没有滑动效果
        /// </summary>
        public AnimationDirection LoadedAnimateDirection { get; set; } = AnimationDirection.None;

        /// <summary>
        /// 卸载页面动画方向 默认值为None表示只有淡出效果，没有滑动效果
        /// </summary>
        public AnimationDirection UnloadAnimateDirection { get; set; } = AnimationDirection.None;

        /// <summary>
        /// 标识在加载时，是否需要使用动画退出
        /// 用于将页面移动到另外的一个Frame容器上
        /// 需要结合AnimatableHost使用，待开发
        /// </summary>
        public bool ShouldAnimationOut { get; private set; } = false;

        /// <summary>
        /// 滑动时间
        /// </summary>
        public float SlideSeconds { get; set; } = 0.5f;

        #endregion

        #region 构造函数

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public AnimatableUserControl()
        {
            // 如果处于设计时，跳过动画逻辑
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            // 如果需要以动画的方式进入，首先将该页影藏
            if (LoadedAnimateDirection != AnimationDirection.None)
            { Visibility = Visibility.Collapsed; }

            // 监听页面加载
            Loaded += BasePage_LoadedAsync;
        }


        #endregion

        #region 根据页面属性，实现动画加载和卸载

        /// <summary>
        /// 一旦页面加载，执行必要的动画
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BasePage_LoadedAsync(object sender, System.Windows.RoutedEventArgs e)
        {
            //if (ShouldAnimationOut)
            //{
            //    // Animation out the page
            //    await AnimateOutAsync();
            //}
            //// Otherwise...
            //else
            {
                // Animate the page in
                await AnimateInAsync();
            }
        }

        /// <summary>
        /// 以动画的方式进入
        /// </summary>
        /// <returns></returns>
        public async Task AnimateInAsync()
        {
            // 如果处于设计时，跳过动画逻辑
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }
            // 开始动画
            await this.SlideAndFadeInAsync(LoadedAnimateDirection, false, SlideSeconds, size: (int)Application.Current.MainWindow.Width);
        }

        /// <summary>
        /// 以动画的方式退出
        /// </summary>
        /// <returns></returns>
        public async Task AnimateOutAsync()
        {
            // 开始动画
            await this.SlideAndFadeOutAsync(UnloadAnimateDirection, SlideSeconds);
        }

        #endregion
    }
}
