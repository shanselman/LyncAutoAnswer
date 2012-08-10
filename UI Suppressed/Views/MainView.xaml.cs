using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SuperSimpleLyncKiosk.ViewModels;

namespace SuperSimpleLyncKiosk.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();


            ResizeMode = ResizeMode.NoResize;
            WindowState = WindowState.Maximized;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                DataContext = new MainViewModel();
            }
            catch (ApplicationException ex)
            {
                MessageBox.Show(ex.ToString(), "FATAL ERROR");
                Application.Current.Shutdown(1);
            }
        }
    }
}


