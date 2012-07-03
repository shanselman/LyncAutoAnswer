using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Lync.Model;
using Microsoft.Lync.Model.Conversation.AudioVideo;
using System.Threading.Tasks;
using Microsoft.Lync.Model.Conversation;
using System.Runtime.InteropServices;
using System.Diagnostics;

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
