using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace ControllerSimulation.App.Views.Converters
{
    public sealed class Decimalplace : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            double decimalValue;
            try
            {
                decimalValue = (double)value;
            }
            catch
            {
                return (string.Empty);
            }
            return (decimalValue.ToString("N1"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
