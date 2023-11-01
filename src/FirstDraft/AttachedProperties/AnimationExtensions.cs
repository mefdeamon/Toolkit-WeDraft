using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace FirstDraft.AttachedProperties
{
    /// <summary>
    /// 动画划动方法
    /// </summary>
    public enum AnimationDirection
    {
        /// <summary>
        /// 无方向动画
        /// </summary>
        None = 0,
        /// <summary>
        /// 左测滑入
        /// </summary>
        Left = 1,
        /// <summary>
        /// 右测滑入
        /// </summary>
        Right = 2,
        /// <summary>
        /// 顶部滑入
        /// </summary>
        Top = 3,
        /// <summary>
        /// 底部滑入
        /// </summary>
        Bottom = 4
    }

    /// <summary>
    /// 给 <see cref="FrameworkElement"/> 添加动画的扩展类
    /// Helpers to animation framework elements in specific ways
    /// </summary>
    public static class FrameworkElementAnimationExtensions
    {
        /// <summary>
        /// 滑入动画
        /// </summary>
        /// <param name="element">动画元素</param>
        /// <param name="direction">划动的方向</param>
        /// <param name="firstLoad">是否第一次加载</param>
        /// <param name="seconds">动画时长</param>
        /// <param name="keepMargin">动画期间是否保持元素的宽度相同</param>
        /// <param name="size">动画的宽度/高度。如果未指定，则使用元素大小</param>
        /// <returns></returns>
        public static async Task SlideAndFadeInAsync(this FrameworkElement element, AnimationDirection direction, bool firstLoad, float seconds = 0.2f, bool keepMargin = true, int size = 0)
        {
            // 创建一个故事板
            var sb = new Storyboard();

            // 添加一个动画：滑入
            switch (direction)
            {
                // 左侧滑入动画
                case AnimationDirection.Left:
                    sb.AddSlideFromLeft(seconds, size == 0 ? element.ActualWidth : size, keepMargin: keepMargin);
                    break;
                // 右侧滑入动画
                case AnimationDirection.Right:
                    sb.AddSlideFromRight(seconds, size == 0 ? element.ActualWidth : size, keepMargin: keepMargin);
                    break;
                // 顶部滑入动画
                case AnimationDirection.Top:
                    sb.AddSlideFromTop(seconds, size == 0 ? element.ActualHeight : size, keepMargin: keepMargin);
                    break;
                // 底部滑入动画
                case AnimationDirection.Bottom:
                    sb.AddSlideFromBottom(seconds, size == 0 ? element.ActualHeight : size, keepMargin: keepMargin);
                    break;
                default:
                    break;
            }

            // 添加一个动画：淡入
            sb.AddFadeIn(seconds);

            // 开始动画
            sb.Begin(element);

            // 展示页面
            if (seconds != 0 || firstLoad)
                element.Visibility = Visibility.Visible;

            // 等待结束
            await Task.Delay((int)(seconds * 1000));
        }


        /// <summary>
        /// 滑出动画
        /// </summary>
        /// <param name="element">动画元素</param>
        /// <param name="direction">划动的方向</param>
        /// <param name="size">是否第一次加载</param>
        /// <param name="seconds">动画时长</param>
        /// <param name="keepMargin">动画的宽度/高度。如果未指定，则使用元素大小</param>
        /// <returns></returns>
        public static async Task SlideAndFadeOutAsync(this FrameworkElement element, AnimationDirection direction, float seconds = 0.2f, bool keepMargin = true, int size = 0)
        {
            // 创建一个故事板
            var sb = new Storyboard();

            // 添加一个动画：滑出
            switch (direction)
            {
                // 左侧滑入动画
                case AnimationDirection.Left:
                    sb.AddSlideToLeft(seconds, size == 0 ? element.ActualWidth : size, keepMargin: keepMargin);
                    break;
                // 右侧滑入动画
                case AnimationDirection.Right:
                    sb.AddSlideToRight(seconds, size == 0 ? element.ActualWidth : size, keepMargin: keepMargin);
                    break;
                // 顶部滑入动画
                case AnimationDirection.Top:
                    sb.AddSlideToTop(seconds, size == 0 ? element.ActualHeight : size, keepMargin: keepMargin);
                    break;
                // 底部滑入动画
                case AnimationDirection.Bottom:
                    sb.AddSlideToBottom(seconds, size == 0 ? element.ActualHeight : size, keepMargin: keepMargin);
                    break;
                default:
                    break;
            }

            // 添加一个动画：淡出
            sb.AddFadeOut(seconds);

            // 开始动画
            sb.Begin(element);

            // 仅当我们正在设置动画时才使页面可见
            if (seconds != 0)
                element.Visibility = Visibility.Visible;

            // 等待结束
            await Task.Delay((int)(seconds * 1000));

            // 隐藏上一个元素
            if (element.Opacity == 0)
                element.Visibility = Visibility.Hidden;
        }
    }


    /// <summary>
    /// 给 <see cref="Storyboard"/> 添加动画的扩展类
    /// </summary>
    public static class StoryboardExtensions
    {

        #region 左边滑入滑出动画  Sliding To/From Left

        /// <summary>
        /// 添加一个从左边滑入的动画到故事板 <see cref="Storyboard"/> 上
        /// </summary>
        /// <param name="storyboard">承载动画的故事板</param>
        /// <param name="seconds">动画时长</param>
        /// <param name="offset">从左边开始的距离</param>
        /// <param name="decelerationRatio">减速率</param>
        /// <param name="keepMargin">动画期间是否保持元素的宽度相同</param>
        public static void AddSlideFromLeft(this Storyboard storyboard, float seconds, double offset, float decelerationRatio = 0.9f, bool keepMargin = true)
        {
            // 创建一个左侧边缘滑入的动画 
            var animation = new ThicknessAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(seconds)),
                From = new Thickness(-offset, 0, keepMargin ? offset : 0, 0),
                To = new Thickness(0),
                DecelerationRatio = decelerationRatio
            };

            // 设置动画目标属性的名称 Margin
            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

            // 将动画添加到当前故事板上
            storyboard.Children.Add(animation);
        }

        /// <summary>
        /// 添加一个从左边滑出的动画到故事板（Storyboard）上
        /// </summary>
        /// <param name="storyboard">承载动画的故事板</param>
        /// <param name="seconds">动画时长</param>
        /// <param name="offset">从左边开始的距离</param>
        /// <param name="decelerationRatio">减速率</param>
        /// <param name="keepMargin">动画期间是否保持元素的宽度相同</param>
        public static void AddSlideToLeft(this Storyboard storyboard, float seconds, double offset, float decelerationRatio = 0.9f, bool keepMargin = true)
        {
            // 创建一个左侧边缘滑出的动画 
            var animation = new ThicknessAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(seconds)),
                From = new Thickness(0),
                To = new Thickness(-offset, 0, keepMargin ? offset : 0, 0),
                DecelerationRatio = decelerationRatio
            };

            // 设置动画目标属性的名称 Margin
            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

            // 将动画添加到当前故事板上
            storyboard.Children.Add(animation);
        }

        #endregion

        #region 右边滑入滑出动画  Sliding To/From Right

        /// <summary>
        /// 添加一个从右边滑入的动画到故事板 <see cref="Storyboard"/> 上
        /// </summary>
        /// <param name="storyboard">承载动画的故事板</param>
        /// <param name="seconds">动画时长</param>
        /// <param name="offset">从左边开始的距离</param>
        /// <param name="decelerationRatio">减速率</param>
        /// <param name="keepMargin">动画期间是否保持元素的宽度相同</param>
        public static void AddSlideFromRight(this Storyboard storyboard, float seconds, double offset, float decelerationRatio = 0.9f, bool keepMargin = true)
        {
            // 创建一个右侧边缘滑入的动画 
            var animation = new ThicknessAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(seconds)),
                From = new Thickness(keepMargin ? offset : 0, 0, -offset, 0),
                To = new Thickness(0),
                DecelerationRatio = decelerationRatio
            };

            // 设置动画目标属性的名称 Margin
            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

            // 将动画添加到当前故事板上
            storyboard.Children.Add(animation);
        }

        /// <summary>
        /// 添加一个从右边滑出的动画到故事板 <see cref="Storyboard"/> 上
        /// </summary>
        /// <param name="storyboard">承载动画的故事板</param>
        /// <param name="seconds">动画时长</param>
        /// <param name="offset">从左边开始的距离</param>
        /// <param name="decelerationRatio">减速率</param>
        /// <param name="keepMargin">动画期间是否保持元素的宽度相同</param>
        public static void AddSlideToRight(this Storyboard storyboard, float seconds, double offset, float decelerationRatio = 0.9f, bool keepMargin = true)
        {
            // 创建一个右侧边缘滑出的动画 
            var animation = new ThicknessAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(seconds)),
                From = new Thickness(0),
                To = new Thickness(keepMargin ? offset : 0, 0, -offset, 0),
                DecelerationRatio = decelerationRatio
            };

            // 设置动画目标属性的名称 Margin
            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

            // 将动画添加到当前故事板上
            storyboard.Children.Add(animation);
        }

        #endregion

        #region 顶部滑入滑出动画  Sliding To/From Top

        /// <summary>
        /// 添加一个从顶部滑入的动画到故事板 <see cref="Storyboard"/> 上
        /// </summary>
        /// <param name="storyboard">承载动画的故事板</param>
        /// <param name="seconds">动画时长</param>
        /// <param name="offset">从左边开始的距离</param>
        /// <param name="decelerationRatio">减速率</param>
        /// <param name="keepMargin">动画期间是否保持元素的宽度相同</param>
        public static void AddSlideFromTop(this Storyboard storyboard, float seconds, double offset, float decelerationRatio = 0.9f, bool keepMargin = true)
        {
            // 创建一个顶部边缘滑入的动画 
            var animation = new ThicknessAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(seconds)),
                From = new Thickness(0, -offset, 0, keepMargin ? offset : 0),
                To = new Thickness(0),
                DecelerationRatio = decelerationRatio
            };

            // 设置动画目标属性的名称 Margin
            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

            // 将动画添加到当前故事板上
            storyboard.Children.Add(animation);
        }

        /// <summary>
        /// 添加一个从顶部滑出的动画到故事板 <see cref="Storyboard"/> 上
        /// </summary>
        /// <param name="storyboard">承载动画的故事板</param>
        /// <param name="seconds">动画时长</param>
        /// <param name="offset">从左边开始的距离</param>
        /// <param name="decelerationRatio">减速率</param>
        /// <param name="keepMargin">动画期间是否保持元素的宽度相同</param>
        public static void AddSlideToTop(this Storyboard storyboard, float seconds, double offset, float decelerationRatio = 0.9f, bool keepMargin = true)
        {
            // 创建一个顶部边缘滑出的动画 
            var animation = new ThicknessAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(seconds)),
                From = new Thickness(0),
                To = new Thickness(0, -offset, 0, keepMargin ? offset : 0),
                DecelerationRatio = decelerationRatio
            };

            // 设置动画目标属性的名称 Margin
            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

            // 将动画添加到当前故事板上
            storyboard.Children.Add(animation);
        }

        #endregion

        #region 底部滑入滑出动画  Sliding To/From Bottom

        /// <summary>
        /// 添加一个从底部滑入的动画到故事板 <see cref="Storyboard"/> 上
        /// </summary>
        /// <param name="storyboard">承载动画的故事板</param>
        /// <param name="seconds">动画时长</param>
        /// <param name="offset">从左边开始的距离</param>
        /// <param name="decelerationRatio">减速率</param>
        /// <param name="keepMargin">动画期间是否保持元素的宽度相同</param>
        public static void AddSlideFromBottom(this Storyboard storyboard, float seconds, double offset, float decelerationRatio = 0.9f, bool keepMargin = true)
        {
            // 创建一个顶部边缘滑入的动画 
            var animation = new ThicknessAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(seconds)),
                From = new Thickness(0, keepMargin ? offset : 0, 0, -offset),
                To = new Thickness(0),
                DecelerationRatio = decelerationRatio
            };

            // 设置动画目标属性的名称 Margin
            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

            // 将动画添加到当前故事板上
            storyboard.Children.Add(animation);
        }

        /// <summary>
        /// 添加一个从底部滑出的动画到故事板 <see cref="Storyboard"/> 上
        /// </summary>
        /// <param name="storyboard">承载动画的故事板</param>
        /// <param name="seconds">动画时长</param>
        /// <param name="offset">从左边开始的距离</param>
        /// <param name="decelerationRatio">减速率</param>
        /// <param name="keepMargin">动画期间是否保持元素的宽度相同</param>
        public static void AddSlideToBottom(this Storyboard storyboard, float seconds, double offset, float decelerationRatio = 0.9f, bool keepMargin = true)
        {
            // 创建一个底部边缘滑出的动画 
            var animation = new ThicknessAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(seconds)),
                From = new Thickness(0),
                To = new Thickness(0, keepMargin ? offset : 0, 0, -offset),
                DecelerationRatio = decelerationRatio
            };

            // 设置动画目标属性的名称 Margin
            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

            // 将动画添加到当前故事板上
            storyboard.Children.Add(animation);
        }

        #endregion

        #region 淡入淡出效果   Fade In/Out

        /// <summary>
        /// 添加一个淡入的动画到故事板 <see cref="Storyboard"/> 上
        /// </summary>
        /// <param name="storyboard">承载动画的故事板</param>
        /// <param name="seconds">动画时长</param>
        public static void AddFadeIn(this Storyboard storyboard, float seconds)
        {
            // 创建一个渐变出现的动画
            var animation = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(seconds)),
                From = 0,
                To = 1,
            };

            // 设置动画目标属性的名称 Opacity
            Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));

            // 将动画添加到当前故事板上
            storyboard.Children.Add(animation);
        }


        /// <summary>
        /// 添加一个淡出的动画到故事板 <see cref="Storyboard"/> 上
        /// </summary>
        /// <param name="storyboard">承载动画的故事板</param>
        /// <param name="seconds">动画时长</param>
        public static void AddFadeOut(this Storyboard storyboard, float seconds)
        {
            // 创建一个渐变消失的动画
            var animation = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(seconds)),
                From = 1,
                To = 0,
            };

            // 设置动画目标属性的名称 Opacity
            Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));

            // 将动画添加到当前故事板上
            storyboard.Children.Add(animation);
        }

        #endregion

    }
}
