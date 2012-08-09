using System.Windows.Controls;
using SuperSimpleLyncKiosk.ViewModels;

namespace SuperSimpleLyncKiosk
{
    public partial class NoCall : UserControl
    {
        public NoCall()
        {
            InitializeComponent();
            DataContext = new NoCallViewModel();
        }
    }
}
