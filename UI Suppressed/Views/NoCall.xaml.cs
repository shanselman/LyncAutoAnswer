/* Copyright (C) 2012 Modality Systems - All Rights Reserved
 * You may use, distribute and modify this code under the
 * terms of the Microsoft Public License, a copy of which 
 * can be seen at: http://www.microsoft.com/en-us/openness/licenses.aspx
 * 
 * http://www.LyncAutoAnswer.com
*/

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

        private void Grid_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {

        }
    }
}
