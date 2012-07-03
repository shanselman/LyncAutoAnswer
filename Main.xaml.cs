using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using Microsoft.Lync.Model;
using Microsoft.Lync.Model.Conversation;
using Microsoft.Lync.Model.Conversation.AudioVideo;

namespace SuperSimpleLyncKiosk
{
    public static class UnsafeNativeMethods
    {
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
    }

    public partial class Main : Window
    {
        public Main()
        {
            InitializeComponent();
            this.WindowState = System.Windows.WindowState.Maximized;

            var lync = LyncClient.GetClient();
            var conversationmgr = lync.ConversationManager;
            conversationmgr.ConversationAdded += (_, cmea) =>
            {
                bool IncomingAV = false;
                StringBuilder sb = new StringBuilder();
            
                //Is this an audio/video invitation?
                if (cmea.Conversation.Modalities[ModalityTypes.AudioVideo].State == ModalityState.Notified)
                {
                    if (lync.DeviceManager.ActiveAudioDevice != null)
                    {
                        sb.Append("Incoming call from ");
                        IncomingAV = true;
                    }
                    else
                    {
                        cmea.Conversation.Modalities[ModalityTypes.AudioVideo].Reject(ModalityDisconnectReason.NotAcceptableHere);
                    }
                }
                if (cmea.Conversation.Modalities[ModalityTypes.InstantMessage].State == ModalityState.Connected)
                {
                    sb.Append("Incoming IM from ");
                }
                sb.Append(String.Join(", ", cmea.Conversation.Participants.Select(i => i.Contact.Uri)));

                Debug.WriteLine(sb.ToString());

                //cmea.Conversation.ParticipantAdded += Conversation_ParticipantAdded;
                //cmea.Conversation.StateChanged += Conversation_ConversationChangedEvent;
                //cmea.Conversation.ActionAvailabilityChanged += Conversation_ActionAvailabilityChanged;
                
                if (IncomingAV == true && Properties.Settings.Default.autoAnswer == true)
                {
                    InitiateAVStream(cmea.Conversation);
                }
            };
        }

        private static void InitiateAVStream(Conversation pConversation)
        {
            if (pConversation.State == ConversationState.Terminated) { return; }

            if (pConversation.Modalities[ModalityTypes.AudioVideo].CanInvoke(ModalityAction.Connect))
            {
                var video = (AVModality)pConversation.Modalities[ModalityTypes.AudioVideo];
                video.Accept();

                //Get ready to be connected, then WE can start OUR video
                video.ModalityStateChanged += AVModality_ModalityStateChanged;
            }
        }
        
        static void AVModality_ModalityStateChanged(object sender, ModalityStateChangedEventArgs e)
        {
            VideoChannel vc = null;
            switch (e.NewState)
            {
                //we can't start video until it's connected
                case ModalityState.Connected:
                    if (vc == null)
                    {
                        vc = ((AVModality)sender).VideoChannel;
                        vc.StateChanged += new EventHandler<ChannelStateChangedEventArgs>(VideoChannel_StateChanged);
                    }
                    break;
            }
        }

        static void VideoChannel_StateChanged(object sender, ChannelStateChangedEventArgs e)
        {
            VideoChannel vc = (VideoChannel)sender;

            //Are we receiving? Let's try to send!
            if (e.NewState == ChannelState.Receive)
            {
                if (vc.CanInvoke(ChannelAction.Start))
                {
                    vc.BeginStart(videoCallBack, vc);
                }
                else {  Debug.WriteLine("CanInvoke said NO!"); }

                //Go looking around for the IM Window (there had better just be the one we just started)
                // and force it to the foreground
                IntPtr childHandle = UnsafeNativeMethods.FindWindowEx(IntPtr.Zero, IntPtr.Zero, "IMWindowClass", null);
                UnsafeNativeMethods.SetForegroundWindow(childHandle);

                //Try to get the video to go full screen by pressing F5
                WindowsInput.InputSimulator.SimulateKeyPress(WindowsInput.VirtualKeyCode.F5);
            }
        }


        private static void videoCallBack(IAsyncResult ar)
        {
            ((VideoChannel)ar.AsyncState).EndStart(ar);
        }

        private static void ModalityCallback(IAsyncResult ar)
        {
            Object[] asyncState = (Object[])ar.AsyncState;
            try
            {
                if (ar.IsCompleted == true)
                {
                    if (asyncState[1].ToString() == "RETRIEVE")
                    {
                        ((AVModality)asyncState[0]).EndRetrieve(ar);
                    }
                    if (asyncState[1].ToString() == "HOLD")
                    {
                        ((AVModality)asyncState[0]).EndHold(ar);
                    }
                    if (asyncState[1].ToString() == "CONNECT")
                    {
                        ((AVModality)asyncState[0]).EndConnect(ar);
                    }
                    if (asyncState[1].ToString() == "FORWARD")
                    {
                        ((AVModality)asyncState[0]).EndForward(ar);
                    }
                }
            }
            catch (LyncClientException) { }
        }
    }
}
