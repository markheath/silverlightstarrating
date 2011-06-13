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

namespace MarkHeath.StarRating
{
    public partial class StarRatingControl : UserControl
    {
        private List<Star> stars;
        private bool isHovering;

        public StarRatingControl()
        {
            InitializeComponent();

            CreateStars();

            this.MouseEnter += new MouseEventHandler(StarRatingControl_MouseEnter);
            this.MouseMove += new MouseEventHandler(StarRatingControl_MouseMove);
            this.MouseLeave += new MouseEventHandler(StarRatingControl_MouseLeave);
            this.MouseLeftButtonDown += new MouseButtonEventHandler(StarRatingControl_MouseLeftButtonDown);
        }

        private void CreateStars()
        {
            stars = new List<Star>();
            LayoutRoot.ColumnDefinitions.Clear();
            LayoutRoot.Children.Clear();
            for (int column = 0; column < NumberOfStars; column++)
            {
                ColumnDefinition cd = new ColumnDefinition();
                cd.Width = new GridLength(34, GridUnitType.Star); // GridLength.Auto;
                LayoutRoot.ColumnDefinitions.Add(cd);

                Star star = new Star();
                star.StarFillBrush = this.StarFillBrush;
                star.SetValue(Grid.ColumnProperty, column);
                LayoutRoot.Children.Add(star);
                this.stars.Add(star);
            }
        }

        void StarRatingControl_MouseEnter(object sender, MouseEventArgs e)
        {
            var mousePos = e.GetPosition(this.LayoutRoot);
            HandleMouseOver(mousePos);
        }

        private int GetRatingFromPosition(Point mousePos)
        {
            int maxRating = NumberOfStars * 2;
            double starRatingWidth = this.LayoutRoot.ColumnDefinitions[0].ActualWidth * NumberOfStars;
            double percent = mousePos.X / starRatingWidth;
            double value = 0.75 + (percent * maxRating);
            int rating = (int)value;
            if (rating < 0) rating = 0;
            if (rating > maxRating) rating = maxRating;
            return rating;
        }

        void StarRatingControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.IsEnabled)
            {
                this.Rating = GetRatingFromPosition(e.GetPosition(this.LayoutRoot));
            }
        }

        void StarRatingControl_MouseLeave(object sender, MouseEventArgs e)
        {
            IsHovering = false;
        }

        private bool IsInBounds(Point p)
        {
            double maxX = this.LayoutRoot.ColumnDefinitions[0].ActualWidth * NumberOfStars;
            double maxY = this.LayoutRoot.ColumnDefinitions[0].ActualWidth; // actual height of a star doesn't give us the right thing
            return (p.Y >= 0) &&
                (p.Y < maxY) &&
                (p.X >= 0) &&
                (p.X < maxX);
        }

        void StarRatingControl_MouseMove(object sender, MouseEventArgs e)
        {
            var mousePos = e.GetPosition(LayoutRoot);
            HandleMouseOver(mousePos);
        }

        private void HandleMouseOver(Point mousePos)
        {
            if (this.IsEnabled) // no hover ratings if not enabled
            {
                this.IsHovering = IsInBounds(mousePos);
                if (this.IsHovering)
                {
                    this.HoverRating = GetRatingFromPosition(mousePos);
                }
            }
            /*else
            {
                Debug.WriteLine("Point not over stars {0}", mousePos);
            }*/
        }

        private bool IsHovering
        {
            get { return isHovering; }
            set
            {
                if (isHovering != value)
                {
                    this.isHovering = value;
                    if (!isHovering)
                    {
                        DrawStarRating(Rating, this.StarFillBrush);
                    }
                }
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
            StarRatingControl starRating = (StarRatingControl)sender;
            starRating.CreateStars();
            starRating.RefreshStarRating();
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
    typeof(StarRatingControl), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0)),
        new PropertyChangedCallback(StarFillBrushChanged)));

        public Brush StarFillBrush
        {
            get { return (Brush)GetValue(StarFillBrushProperty); }
            set { SetValue(StarFillBrushProperty, value); }
        }

        private static void StarFillBrushChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var starRating = (StarRatingControl)sender;
            starRating.RefreshStarRating();
        }
        #endregion

        #region HoverFillBrushProperty
        public static readonly DependencyProperty HoverFillBrushProperty = DependencyProperty.Register(
    "HoverFillBrush", typeof(Brush),
    typeof(StarRatingControl), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0x80)),
        new PropertyChangedCallback(HoverFillBrushChanged)));

        public Brush HoverFillBrush
        {
            get { return (Brush)GetValue(HoverFillBrushProperty); }
            set { SetValue(HoverFillBrushProperty, value); }
        }

        private static void HoverFillBrushChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var starRating = (StarRatingControl)sender;
            starRating.RefreshStarRating();
        }
        #endregion

        private void RefreshStarRating()
        {
            if (isHovering)
            {
                DrawStarRating(this.HoverRating, this.HoverFillBrush);
            }
            else
            {
                DrawStarRating(this.Rating, this.StarFillBrush);
            }
        }

        private void DrawStarRating(int value, Brush fillBrush)
        {
            //Debug.WriteLine("Value = {0}", value);
            for (int star = 0; star < NumberOfStars; star++)
            {
                if (value >= (star + 1) * 2)
                {
                    stars[star].StarFillBrush = fillBrush;
                    stars[star].HalfFillBrush = null;
                }
                else if (value >= 1 + star * 2)
                {
                    stars[star].StarFillBrush = null;
                    stars[star].HalfFillBrush = fillBrush;
                }
                else
                {
                    stars[star].StarFillBrush = null;
                    stars[star].HalfFillBrush = null;
                }
                stars[star].Foreground = (IsHovering) ? new SolidColorBrush(Color.FromArgb(0xff, 0x80, 0x80, 0x80)) : this.Foreground;
            }
        }
    }
}
