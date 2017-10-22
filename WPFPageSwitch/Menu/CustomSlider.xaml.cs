using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFPageSwitch.Menu
{
    /// <summary>
    /// Interaction logic for CustomSlider.xaml
    /// </summary>
    public partial class CustomSlider : UserControl
    {
        public event EventHandler<RoutedPropertyChangedEventArgs<double>> SliderValueChanged;
        public CustomSlider()
        {
            InitializeComponent();
        }

        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(double), typeof(CustomSlider), new UIPropertyMetadata(0d));


        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double), typeof(CustomSlider), new UIPropertyMetadata(0d));


        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(CustomSlider), new UIPropertyMetadata(0d));

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SliderValueChanged?.Invoke(sender, e);
        }
    }
}
