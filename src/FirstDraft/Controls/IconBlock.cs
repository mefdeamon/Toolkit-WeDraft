using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FirstDraft.Controls
{
    public class IconBlock
    {

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.RegisterAttached("Icon", typeof(Geometry), typeof(IconBlock), new PropertyMetadata(default(Geometry)));

        public static void SetIcon(DependencyObject obj, Geometry value) => obj.SetValue(IconProperty, value);
        public static Geometry GetIcon(DependencyObject obj) => (Geometry)obj.GetValue(IconProperty);



        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.RegisterAttached("Size", typeof(double), typeof(IconBlock), new PropertyMetadata(double.NaN));

        public static void SetSize(DependencyObject obj, double value) => obj.SetValue(SizeProperty, value);

        public static double GetSize(DependencyObject obj) => (double)obj.GetValue(SizeProperty);



        public static Brush GetFill(DependencyObject obj)
        {
            return (Brush)obj.GetValue(FillProperty);
        }

        public static void SetFill(DependencyObject obj, Brush value)
        {
            obj.SetValue(FillProperty, value);
        }

        public static readonly DependencyProperty FillProperty =
            DependencyProperty.RegisterAttached("Fill", typeof(Brush), typeof(IconBlock), new PropertyMetadata(default(Brush)));



    }

    public class BoderAtt
    {
        public static CornerRadius GetCornerRadius(DependencyObject obj) => (CornerRadius)obj.GetValue(CornerRadiusProperty);

        public static void SetCornerRadius(DependencyObject obj, CornerRadius value) => obj.SetValue(CornerRadiusProperty, value);

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.RegisterAttached("CornerRadius", typeof(CornerRadius), typeof(BoderAtt), new PropertyMetadata(default(CornerRadius)));
    }

}
