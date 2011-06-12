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
using System.Windows.Data;

namespace SilverlightStarRatingControl
{
    public partial class BindToPropertyTest : UserControl
    {
        public BindToPropertyTest()
        {
            InitializeComponent();
            Ellipse e = new Ellipse();
            //e.Fill = new SolidColorBrush(Colors.Blue);
            Binding b = new Binding();
            b.ElementName = "LayoutRoot";
            b.Path = new PropertyPath("Parent.Foreground");
            BindingOperations.SetBinding(e, Ellipse.FillProperty, b);
            this.LayoutRoot.Children.Add(e);
        }
    }
}
