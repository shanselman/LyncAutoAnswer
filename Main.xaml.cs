using System.Windows;
using Microsoft.Lync.Model;

namespace SuperSimpleLyncKiosk
{
    public partial class Main : Window
    {
        public Main()
        {
            InitializeComponent();
            this.WindowState = System.Windows.WindowState.Maximized;

            var lync = LyncClient.GetClient();

            lync.ConversationManager.AutoAnswerIncomingVideoCalls(() => Properties.Settings.Default.autoAnswer);
        }
    }
}
