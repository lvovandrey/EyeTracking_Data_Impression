using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace DataImpression.View
{
    /// <summary>
    /// Логика взаимодействия для FAOIDistributedComplexColumnChart.xaml
    /// </summary>
    public partial class FAOIDistributedComplexColumnChart : UserControl
    {
        public FAOIDistributedComplexColumnChart()
        {
            InitializeComponent();
            Zoom = 1;
        }

        public static readonly DependencyProperty ZoomProperty = DependencyProperty.Register("Zoom",
           typeof(double), typeof(FAOIDistributedComplexColumnChart));

        public double Zoom
        {
            get { return (double)GetValue(ZoomProperty); }
            set { SetValue(ZoomProperty, value); }
        }

        private void THIS_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                e.Handled = true;
                if (e.Delta > 0)
                    Zoom *= 1.1;
                else
                    Zoom *= 0.9;
            }
        }
    }


    /// <summary>
    /// Конвертер просто делит на 3 высоту и домножает ее на зум
    /// </summary>
    public class HeightConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value[0] is double && value[1] is double)
                return (((double)value[0] * (double)value[1]) / 3)-10;

            return Binding.DoNothing;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

}
