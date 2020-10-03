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
        }
    }


    /// <summary>
    /// Конвертер просто делит на 3 высоту
    /// </summary>
    public class HeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is double)
                return (((double)value) / 3)-5;

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {


            return Binding.DoNothing;
        }
    }

}
