using System.Windows.Controls;
using SuperSimpleLyncKiosk.ViewModels;

namespace SuperSimpleLyncKiosk
{
    /// <summary>
    /// Interaction logic for Call.xaml
    /// </summary>
    public partial class Call : UserControl
    {
        public Call()
        {
            InitializeComponent();
            DataContext = new IncomingCallViewModel();
        }
    }
}
