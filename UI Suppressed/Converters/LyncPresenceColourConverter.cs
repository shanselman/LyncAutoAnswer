using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace SuperSimpleLyncKiosk.Converters
{
    class LyncPresenceColourConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return Brushes.Gray;

            if (value is string)
            {
                switch ((string)value)
                {
                    case "Free":
                        return Brushes.LimeGreen;
                    case "FreeIdle":
                        return Brushes.Yellow;
                    case "Busy":
                        return Brushes.Red;
                    case "BusyIdle":
                        return Brushes.Red;
                    case "DoNotDisturb":
                        return Brushes.Red;
                    case "TemporarilyAway":
                        return Brushes.Yellow;
                    case "Away":
                        return Brushes.Yellow;
                    case "Offline":
                        return Brushes.Gray;
                    default:
                        return Brushes.Gray;
                }
            }
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType == typeof(string))
            {
                return (string)value;
            }
            throw new NotImplementedException();
        }


    }
}
