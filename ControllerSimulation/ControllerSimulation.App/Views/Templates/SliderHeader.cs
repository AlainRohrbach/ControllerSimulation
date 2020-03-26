using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ControllerSimulation.App.Views.Templates
{
    public class SliderHeader : DependencyObject
    {
        public string Label { get; set; }   

        public string Glyph { get; set; }

        public string Suffix { get; set; }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(SliderHeader), new PropertyMetadata(0));
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
    }
}
