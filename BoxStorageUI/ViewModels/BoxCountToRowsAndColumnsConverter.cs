using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace BoxStorageUI.ViewModels
{
    class BoxCountToRowsAndColumnsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int itemCount && parameter is int desiredColumns && desiredColumns > 0)
            {
                int rows = (int)Math.Ceiling((double)itemCount / desiredColumns);
                return new GridLength(rows, GridUnitType.Auto);
            }

            return new GridLength(1, GridUnitType.Star);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
