/* Copyright (C) 2012 Modality Systems - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the Microsoft Public License, a copy of which 
 * can be seen at: http://www.microsoft.com/en-us/openness/licenses.aspx
 * 
 * http://www.LyncAutoAnswer.com
*/

using System;
using System.Windows;

namespace SuperSimpleLyncKiosk.Extensions
{

    public class ControlExtensions
    {
        #region Dependency Properties

        public static readonly DependencyProperty CurrentVisualStateProperty = DependencyProperty.RegisterAttached("CurrentVisualState", typeof(String), typeof(ControlExtensions), new PropertyMetadata(OnCurrentVisualStatePropertyChanged));

        #endregion

        #region Public Methods

        public static string GetCurrentVisualState(DependencyObject obj)
        {
            return (string)obj.GetValue(CurrentVisualStateProperty);
        }

        public static void SetCurrentVisualState(DependencyObject obj, string value)
        {
            obj.SetValue(CurrentVisualStateProperty, value);
        }

        #endregion

        #region Event Handlers

        private static void OnCurrentVisualStatePropertyChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            var newState = (string)args.NewValue;
            if (string.IsNullOrWhiteSpace(newState)) return; // Ignore null or empty state settings

            FrameworkElement element = sender as FrameworkElement;
            if (element == null) throw new ArgumentException("CurrentVisualState is only supported on the FrameworkElement type");

            if (!VisualStateManager.GoToState(element, newState, true))
                if (!VisualStateManager.GoToElementState(element, newState, true))
                    throw new ArgumentException(string.Format("The state '{0}' could not be found on the element '{1}'", newState, element));
        }

        #endregion
    }
}

