using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DataImpression.ViewModel.AvalonDockHelpers
{

    /// <summary>
    /// Конвертер проверяет является ли VM-ка DocumentViewVM. Сам не очень понимаю - это что такая заглушка от ошибок?
    /// </summary>
    public class ActiveDocumentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is DocumentViewVM)
                return value;

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is DocumentViewVM)
                return value;

            return Binding.DoNothing;
        }
    }
}
