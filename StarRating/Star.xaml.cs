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

namespace MarkHeath.StarRating
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
    typeof(Star), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0x80))));

        public Brush StarFillBrush
        {
            get { return (Brush)GetValue(StarFillBrushProperty); }
            set { SetValue(StarFillBrushProperty, value); }
        }

        #endregion

        #region HalfFillBrushProperty
        public static readonly DependencyProperty HalfFillBrushProperty = DependencyProperty.Register(
    "HalfFillBrush", typeof(Brush),
    typeof(Star), new PropertyMetadata(null));

        public Brush HalfFillBrush
        {
            get { return (Brush)GetValue(HalfFillBrushProperty); }
            set { SetValue(HalfFillBrushProperty, value); }
        }

        #endregion

        #region StrokeThicknessProperty
        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(
    "StrokeThickness", typeof(double),
    typeof(Star), new PropertyMetadata(2.0));

        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        #endregion

        #region StrokeLineJoinProperty
        public static readonly DependencyProperty StrokeLineJoinProperty = DependencyProperty.Register(
    "StrokeLineJoin", typeof(PenLineJoin),
    typeof(Star), new PropertyMetadata(PenLineJoin.Round));

        public PenLineJoin StrokeLineJoin
        {
            get { return (PenLineJoin)GetValue(StrokeLineJoinProperty); }
            set { SetValue(StrokeLineJoinProperty, value); }
        }

        #endregion
    }
}
