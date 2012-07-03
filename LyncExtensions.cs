using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Lync.Model;
using Microsoft.Lync.Model.Conversation;
using Microsoft.Lync.Model.Conversation.AudioVideo;

namespace SuperSimpleLyncKiosk
{
    public static class LyncExtensions
    {
        public static Task StartAsync(this VideoChannel videoChannel)
        {
            return Task.Factory.FromAsync(videoChannel.BeginStart, videoChannel.EndStart, null);
        }

        public static void AutoAnswerIncomingVideoCalls(this ConversationManager conversationManager)
        {
            AutoAnswerIncomingVideoCalls(conversationManager, () => true);
        }

        public static void AutoAnswerIncomingVideoCalls(this ConversationManager conversationManager, Func<bool> predicate)
        {
            conversationManager.ConversationAdded += (s, e) => ConversationManager_ConversationAdded(s, e, predicate);
        }

        public static void AnswerVideo(this Conversation conversation)
        {
            if (conversation.State == ConversationState.Terminated)
            {
                return;
            }

            var av = (AVModality)conversation.Modalities[ModalityTypes.AudioVideo];
            if (av.CanInvoke(ModalityAction.Connect))
            {
                av.Accept();

                // Get ready to be connected, then WE can start OUR video
                av.ModalityStateChanged += AVModality_ModalityStateChanged;
            }
        }

        private static void ConversationManager_ConversationAdded(object sender, ConversationManagerEventArgs e, Func<bool> autoAnswerPredicate)
        {
            var lync = LyncClient.GetClient();
            var incomingAV = false;
            var sb = new StringBuilder();
            var av = e.Conversation.Modalities[ModalityTypes.AudioVideo];
            var im = e.Conversation.Modalities[ModalityTypes.InstantMessage];

            // Is this an audio/video invitation?
            if (av.State == ModalityState.Notified)
            {
                if (lync.DeviceManager.ActiveAudioDevice != null)
                {
                    sb.Append("Incoming call from ");
                    incomingAV = true;
                }
                else
                {
                    av.Reject(ModalityDisconnectReason.NotAcceptableHere);
                }
            }
            if (im.State == ModalityState.Connected)
            {
                sb.Append("Incoming IM from ");
            }

            sb.Append(String.Join(", ", e.Conversation.Participants.Select(i => i.Contact.Uri)));
            Debug.WriteLine(sb.ToString());

            //eventArgs.Conversation.ParticipantAdded += Conversation_ParticipantAdded;
            //eventArgs.Conversation.StateChanged += Conversation_ConversationChangedEvent;
            //eventArgs.Conversation.ActionAvailabilityChanged += Conversation_ActionAvailabilityChanged;

            if (incomingAV && autoAnswerPredicate())
            {
                e.Conversation.AnswerVideo();
            }
        }

        private static void AVModality_ModalityStateChanged(object sender, ModalityStateChangedEventArgs e)
        {
            if (e.NewState == ModalityState.Connected)
            {
                // We can't start video until it's connected
                var vc = ((AVModality)sender).VideoChannel;
                vc.StateChanged += VideoChannel_StateChanged;
            }
        }

        private static void VideoChannel_StateChanged(object sender, ChannelStateChangedEventArgs e)
        {
            if (e.NewState != ChannelState.Receive)
            {
                return;
            }

            var vc = (VideoChannel)sender;
            if (vc.CanInvoke(ChannelAction.Start))
            {
                vc.StartAsync().ContinueWith(t =>
                {
                    // Go looking around for the IM Window (there had better just be the one we just started)
                    // and force it to the foreground
                    IntPtr childHandle = UnsafeNativeMethods.FindWindowEx(IntPtr.Zero, IntPtr.Zero, "IMWindowClass", null);
                    UnsafeNativeMethods.SetForegroundWindow(childHandle);

                    // Try to get the video to go full screen by pressing F5
                    WindowsInput.InputSimulator.SimulateKeyPress(WindowsInput.VirtualKeyCode.F5);
                });
            }
            else
            {
                Debug.WriteLine("CanInvoke said NO!");
            }
        }
    }

    public static class UnsafeNativeMethods
    {
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
    }
}
