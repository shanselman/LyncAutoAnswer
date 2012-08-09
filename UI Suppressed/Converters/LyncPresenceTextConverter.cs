using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace SuperSimpleLyncKiosk.Converters
{
    class LyncPresenceTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return "Unknown";
            
            if (value is string)
            {
                switch ((string)value)
                {
                    case "Free":
                        return "Available";
                    case "FreeIdle":
                        return "Inactive";
                    case "Busy":
                        return "Busy";
                    case "BusyIdle":
                        return "Busy";
                    case "DoNotDisturb":
                        return "Do Not Disturb";
                    case "TemporarilyAway":
                        return "Be Right Back";
                    case "Away":
                        return "Away";
                    case "Offline":
                        return "Offline";
                    default:
                        return "Unknown";
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
