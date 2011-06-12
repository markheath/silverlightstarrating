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
    }
}
