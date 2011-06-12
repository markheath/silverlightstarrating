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
using System.Windows.Markup;
using System.Windows.Data;
using System.Diagnostics;

namespace SilverlightStarRatingControl
{
    public partial class StarRatingControl : UserControl
    {
        private List<Path> starOutlines;
        private List<Path> starFills;
        private List<Path> starHalfFills;
        private bool isHovering;

        public StarRatingControl()
        {
            InitializeComponent();

            starOutlines = new List<Path>();
            starFills = new List<Path>();
            starHalfFills = new List<Path>();
            LayoutRoot.ColumnDefinitions.Clear();
            LayoutRoot.Children.Clear();
            for (int column = 0; column < NumberOfStars; column++)
            {
                ColumnDefinition cd = new ColumnDefinition();
                cd.Width = GridLength.Auto;
                LayoutRoot.ColumnDefinitions.Add(cd);

                string defaultNamespace = "xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"";
                string starPath = "<Path " + defaultNamespace + " Data=\"M 2,12 l 10,0 l 5,-10 l 5,10 l 10,0 l -7,10 l 2,10 l -10,-5 l -10,5 l 2,-10 Z\" />";
                string halfStarPath = "<Path " + defaultNamespace + " Data=\"M 2,12 l 10,0 l 5,-10 v 25 l -10,5 l 2,-10 Z\" />";

                Path starFill = (Path)XamlReader.Load(starPath);
                starFill.Fill = this.StarFillBrush;
                starFill.SetValue(Grid.ColumnProperty, column);
                starFill.Visibility = Visibility.Collapsed;
                LayoutRoot.Children.Add(starFill);
                this.starFills.Add(starFill);
                
                Path halfStarFill = (Path)XamlReader.Load(halfStarPath);
                halfStarFill.Fill = this.StarFillBrush;
                halfStarFill.SetValue(Grid.ColumnProperty, column);
                halfStarFill.Visibility = Visibility.Collapsed;
                LayoutRoot.Children.Add(halfStarFill);
                this.starHalfFills.Add(halfStarFill);

                Path starOutline = (Path)XamlReader.Load(starPath);

                starOutline.Stroke = new SolidColorBrush(Color.FromArgb(0xff, 0x40, 0x40, 0x80));
                starOutline.StrokeThickness = 3;
                starOutline.StrokeLineJoin = PenLineJoin.Round;                
                starOutline.SetValue(Grid.ColumnProperty, column);
                this.starOutlines.Add(starOutline);
                LayoutRoot.Children.Add(starOutline);                
            }

            this.MouseEnter += new MouseEventHandler(StarRatingControl_MouseEnter);
            this.MouseMove += new MouseEventHandler(StarRatingControl_MouseMove);
            this.MouseLeave += new MouseEventHandler(StarRatingControl_MouseLeave);
            this.MouseLeftButtonDown += new MouseButtonEventHandler(StarRatingControl_MouseLeftButtonDown);
        }

        void StarRatingControl_MouseEnter(object sender, MouseEventArgs e)
        {
            Debug.WriteLine("MOUSE ENTER");
        }

        private int GetRatingFromPosition(Point mousePos)
        {
            int maxRating = NumberOfStars * 2;
            double starRatingWidth = this.LayoutRoot.ColumnDefinitions[0].ActualWidth * NumberOfStars;
            double value = 0.5 + (mousePos.X / starRatingWidth) * (maxRating);
            int rating = (int)value;
            if (rating < 0) rating = 0;
            if (rating > maxRating) rating = maxRating;
            return rating;
        }

        void StarRatingControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Rating = GetRatingFromPosition(e.GetPosition(this.LayoutRoot));            
        }

        void StarRatingControl_MouseLeave(object sender, MouseEventArgs e)
        {
            isHovering = false;
            DrawStarRating(Rating, this.StarFillBrush);
        }

        void StarRatingControl_MouseMove(object sender, MouseEventArgs e)
        {
            var mousePos = e.GetPosition(LayoutRoot);
            if (mousePos.Y < this.starOutlines[0].ActualHeight)
            {
                this.HoverRating = GetRatingFromPosition(e.GetPosition(this.LayoutRoot));
            }
        }

        #region NumberOfStarsProperty
        public static readonly DependencyProperty NumberOfStarsProperty =
            DependencyProperty.Register(
                "NumberOfStars", typeof(int),
                typeof(StarRatingControl), new PropertyMetadata(5, new PropertyChangedCallback(OnNumberOfStarsChanged)));

        public int NumberOfStars
        {
            get { return (int)GetValue(NumberOfStarsProperty); }
            set { SetValue(NumberOfStarsProperty, value); }
        }

        private static void OnNumberOfStarsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
        }
        #endregion

        #region RatingProperty
        public static readonly DependencyProperty RatingProperty = DependencyProperty.Register(
            "Rating", typeof(int),
            typeof(StarRatingControl), new PropertyMetadata(0, new PropertyChangedCallback(OnRatingChanged)));

        public int Rating
        {
            get { return (int)GetValue(RatingProperty); }
            set { SetValue(RatingProperty, value); }
        }

        private static void OnRatingChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var src = (StarRatingControl)sender;
            src.DrawStarRating((int)args.NewValue, src.StarFillBrush);
        }
        #endregion

        #region HoverRatingProperty
        public static readonly DependencyProperty HoverRatingProperty = DependencyProperty.Register(
            "HoverRating", typeof(int),
            typeof(StarRatingControl), new PropertyMetadata(0, new PropertyChangedCallback(OnHoverRatingChanged)));

        public int HoverRating
        {
            get { return (int)GetValue(HoverRatingProperty); }
            set { SetValue(HoverRatingProperty, value); }
        }

        private static void OnHoverRatingChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var src = (StarRatingControl)sender;
            src.DrawStarRating((int)args.NewValue, src.HoverFillBrush);
        }
        #endregion

        #region StarFillBrushProperty
        public static readonly DependencyProperty StarFillBrushProperty = DependencyProperty.Register(
    "StarFillBrush", typeof(Brush),
    typeof(StarRatingControl), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(0xFF,0xFF,0xFF,0)),
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

        #region HoverFillBrushProperty
        public static readonly DependencyProperty HoverFillBrushProperty = DependencyProperty.Register(
    "HoverFillBrush", typeof(Brush),
    typeof(StarRatingControl), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(0xFF, 0xCF, 0xCF, 0)),
        new PropertyChangedCallback(HoverFillBrushChanged)));

        public Brush HoverFillBrush
        {
            get { return (Brush)GetValue(HoverFillBrushProperty); }
            set { SetValue(HoverFillBrushProperty, value); }
        }

        private static void HoverFillBrushChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {

        }
        #endregion

        private void DrawStarRating(int value, Brush fillBrush)
        {            
            Debug.WriteLine("Value = {0}", value);
            for (int star = 0; star < NumberOfStars; star++)
            {
                if (value >= (star + 1) * 2)
                {
                    starFills[star].Visibility = Visibility.Visible;
                    starFills[star].Fill = fillBrush;
                    starHalfFills[star].Visibility = Visibility.Collapsed;
                }
                else if (value >= 1 + star * 2)
                {
                    starFills[star].Visibility = Visibility.Collapsed;
                    starHalfFills[star].Visibility = Visibility.Visible;
                    starHalfFills[star].Fill = fillBrush;
                }
                else
                {
                    starFills[star].Visibility = Visibility.Collapsed;
                    starHalfFills[star].Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}
