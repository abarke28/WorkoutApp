using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WorkoutApp.utils
{
    public static class TextFocuser
    {
        public static bool GetSelectAllOnFocus(DependencyObject obj)
        {
            return (bool)obj.GetValue(SelectAllOnFocusProperty);
        }

        public static void SetSelectAllOnFocus(DependencyObject obj, bool value)
        {
            obj.SetValue(SelectAllOnFocusProperty, value);
        }

        public static readonly DependencyProperty SelectAllOnFocusProperty =
            DependencyProperty.RegisterAttached("SelectAllOnFocus", typeof(bool), typeof(TextFocuser), new PropertyMetadata(false, SelectAllOnFocusChanged));
        
        private static void SelectAllOnFocusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue) return;

            if (d is TextBox textBox)
            {
                if ((bool)e.NewValue) 
                {
                    textBox.GotFocus += SelectAll;
                    textBox.PreviewMouseDown += IgnoreMouse;
                }
                else
                {
                    textBox.GotFocus -= SelectAll;
                    textBox.PreviewMouseDown -= IgnoreMouse;
                }
            }
        }

        public static void SelectAll(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is TextBox textBox)
            {
                textBox.SelectAll();
            }
        }

        public static void IgnoreMouse(object sender, MouseButtonEventArgs e)
        {
            var frameworkElement = sender as FrameworkElement;
            if (frameworkElement == null || frameworkElement.IsKeyboardFocusWithin) return;
            e.Handled = true;
            frameworkElement.Focus();
        }
    }
}
