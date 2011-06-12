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
        List<Path> starOutlines;
        List<Path> starFills;
        List<Path> starHalfFills;

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
                starFill.Fill = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0xff, 0));
                starFill.SetValue(Grid.ColumnProperty, column);
                starFill.Visibility = Visibility.Collapsed;
                LayoutRoot.Children.Add(starFill);
                this.starFills.Add(starFill);
                
                Path halfStarFill = (Path)XamlReader.Load(halfStarPath);
                halfStarFill.Fill = new SolidColorBrush(Color.FromArgb(0xff, 0, 0xff, 0));
                halfStarFill.SetValue(Grid.ColumnProperty, column);
                halfStarFill.Visibility = Visibility.Collapsed;
                LayoutRoot.Children.Add(halfStarFill);
                this.starHalfFills.Add(halfStarFill);

                Path starOutline = (Path)XamlReader.Load(starPath);

                //Binding b = new Binding();
                //b.Path = new PropertyPath(UserControl.ForegroundProperty);
                //b.Source = this;
                //BindingOperations.SetBinding(star, Path.StrokeProperty, b);
                //Binding b = new Binding("Foreground") { ElementName="Root" };
                //BindingOperations.SetBinding(star, Path.StrokeProperty, b);
                starOutline.Stroke = new SolidColorBrush(Color.FromArgb(0xff, 0x40, 0x40, 0x80));
                starOutline.StrokeThickness = 3;
                starOutline.StrokeLineJoin = PenLineJoin.Round;                
                starOutline.SetValue(Grid.ColumnProperty, column);
                this.starOutlines.Add(starOutline);
                LayoutRoot.Children.Add(starOutline);

                
            }
            this.MouseMove += new MouseEventHandler(StarRatingControl_MouseMove);
        }

        void StarRatingControl_MouseMove(object sender, MouseEventArgs e)
        {
            var mousePos = e.GetPosition(this);

            double starRatingWidth = this.LayoutRoot.ColumnDefinitions.First().ActualWidth * NumberOfStars;
            double value = (mousePos.X / starRatingWidth) * (NumberOfStars * 2);
            Debug.WriteLine("Value = {0}", value);
            for (int star = 0; star < NumberOfStars; star++)
            {
                if (value >= (star + 1) * 2)
                {
                    starFills[star].Visibility = Visibility.Visible;
                    starHalfFills[star].Visibility = Visibility.Collapsed;
                }
                else if (value > 1 + star * 2)
                {
                    starFills[star].Visibility = Visibility.Collapsed;
                    starHalfFills[star].Visibility = Visibility.Visible;
                }
                else
                {
                    starFills[star].Visibility = Visibility.Collapsed;
                    starHalfFills[star].Visibility = Visibility.Collapsed;
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
        }
        #endregion

        #region RatingProperty
        public static readonly DependencyProperty RatingProperty =
    DependencyProperty.Register(
        "Rating", typeof(int),
        typeof(StarRatingControl), new PropertyMetadata(0, new PropertyChangedCallback(OnRatingChanged)));

        public int Rating
        {
            get { return (int)GetValue(RatingProperty); }
            set { SetValue(RatingProperty, value); }
        }

        private static void OnRatingChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {

        }
        #endregion


    }
}
