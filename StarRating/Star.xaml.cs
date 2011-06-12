using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SilverlightStarRatingControl
{
    public partial class Star : UserControl
    {
        public Star()
        {
            InitializeComponent();
            this.SizeChanged += new SizeChangedEventHandler(Star_SizeChanged);
        }

        void Star_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double scaleX = e.NewSize.Width / 34;
            double scaleY = e.NewSize.Height / 34;
            scaleTransform.ScaleX = scaleTransform.ScaleY = Math.Min(scaleX, scaleY);
        }

        #region StarFillBrushProperty
        public static readonly DependencyProperty StarFillBrushProperty = DependencyProperty.Register(
    "StarFillBrush", typeof(Brush),
    typeof(Star), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(0xFF, 0xC0, 0xC0, 0xC0)),
        new PropertyChangedCallback(StarFillBrushChanged)));

        public Brush StarFillBrush
        {
            get { return (Brush)GetValue(StarFillBrushProperty); }
            set { SetValue(StarFillBrushProperty, value); }
        }

        private static void StarFillBrushChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {

        }
        #endregion

        #region HalfFillBrushProperty
        public static readonly DependencyProperty HalfFillBrushProperty = DependencyProperty.Register(
    "HalfFillBrush", typeof(Brush),
    typeof(Star), new PropertyMetadata(null,
        new PropertyChangedCallback(HalfFillBrushChanged)));

        public Brush HalfFillBrush
        {
            get { return (Brush)GetValue(HalfFillBrushProperty); }
            set { SetValue(HalfFillBrushProperty, value); }
        }

        private static void HalfFillBrushChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {

        }
        #endregion
    }
}
