/* Copyright (C) 2012 Modality Systems - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the Microsoft Public License, a copy of which 
 * can be seen at: http://www.microsoft.com/en-us/openness/licenses.aspx
 * 
 * http://www.LyncAutoAnswer.com
*/

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
                        return Brushes.ForestGreen;
                    case "FreeIdle":
                        return Brushes.Goldenrod;
                    case "Busy":
                        return Brushes.Firebrick;
                    case "BusyIdle":
                        return Brushes.Firebrick;
                    case "DoNotDisturb":
                        return Brushes.Maroon;
                    case "TemporarilyAway":
                        return Brushes.Goldenrod;
                    case "Away":
                        return Brushes.Goldenrod;
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
