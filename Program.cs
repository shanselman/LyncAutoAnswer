using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Lync.Model;
using Microsoft.Lync.Model.Conversation.AudioVideo;
using System.Threading.Tasks;
using Microsoft.Lync.Model.Conversation;

namespace LyncAutoAnswerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var lync = LyncClient.GetClient();
            var conversationmgr = lync.ConversationManager;
            conversationmgr.ConversationAdded += (_, cmea) =>
                {
                    Boolean IncomingAV = false;
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
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
                    //string callerName = data.Conversation.Participants[1].Contact.GetContactInformation(ContactInformationType.DisplayName).ToString();
                    string callerName = String.Join(", ", cmea.Conversation.Participants.Select(i => i.Contact.Uri));

                    Console.WriteLine(sb.ToString());

                    //cmea.Conversation.ParticipantAdded += Conversation_ParticipantAdded;
                    //cmea.Conversation.StateChanged += Conversation_ConversationChangedEvent;
                    //cmea.Conversation.ActionAvailabilityChanged += Conversation_ActionAvailabilityChanged;
                    if (IncomingAV == true)
                    {
                        InitiateAVStream(cmea.Conversation);
                    }




                    //var convo = cmea.Conversation;

                    //Console.Write("Conversation added with ");
                    //Console.WriteLine(String.Join(", ", convo.Participants.Select(i => i.Contact.Uri)));

                    //if (convo.Modalities.ContainsKey(ModalityTypes.AudioVideo)
                    //    && convo.Modalities[ModalityTypes.AudioVideo].State != ModalityState.Disconnected)
                    //{
                    //    //convo.Modalities[ModalityTypes.AudioVideo].Accept();
                    //    var video = (AVModality)convo.Modalities[ModalityTypes.AudioVideo];
                    //    video.Accept();
                    //}








                    //var conversation = connectionManagerEventArgs.Conversation;
                    //var video = (AVModality)conversation.Modalities[ModalityTypes.AudioVideo];
                    //video.ModalityStateChanged += (s, modalityChangedSentEventArgs) =>
                    //    {
                    //        if (modalityChangedSentEventArgs.NewState == ModalityState.Notified)
                    //        {
                    //            Console.WriteLine(modalityChangedSentEventArgs.NewState.ToString());
                    //        }
                    //    };

                    ////var chat = (InstantMessageModality)conversation.Modalities[ModalityTypes.InstantMessage];
                    ////chat.ModalityStateChanged += (sender, modalityChangedSentEventArgs) =>
                    ////{
                    ////    Console.WriteLine(modalityChangedSentEventArgs.NewState.ToString());
                    ////    if (modalityChangedSentEventArgs.NewState == ModalityState.Connected)
                    ////    {
                    ////        chat.InstantMessagePropertyChanged += (_3, propchanged) =>
                    ////        {
                    ////            Console.WriteLine(propchanged.ToString());
                    ////        };
                    ////        chat.InstantMessageReceived += (_2, messageSentEventArgs) =>
                    ////        {
                    ////            if (messageSentEventArgs.Contents[InstantMessageContentType.PlainText].Contains("password"))
                    ////            {
                    //                var convo = conversation;
                    //                var av = (AVModality)convo.Modalities[ModalityTypes.AudioVideo];
                    //                var startVideoTask = Task.Factory.FromAsync(av.VideoChannel.BeginStart, av.VideoChannel.EndStart, null);
                    //                startVideoTask.ContinueWith(t =>
                    //                {
                    //                    // Video is now started
                    //                });
                    ////            }
                    ////        };
                    ////    }
                    ////};
                };

            Console.ReadLine();
        }

        private static void InitiateAVStream(Conversation pConversation)
        {
            if (pConversation.State == ConversationState.Terminated)
            {
                return;
            }

            if (pConversation.Modalities[ModalityTypes.AudioVideo].CanInvoke(ModalityAction.Connect))
            {
                //pConversation.Modalities[ModalityTypes.AudioVideo].ModalityStateChanged += _AVModality_ModalityStateChanged;
                //pConversation.Modalities[ModalityTypes.AudioVideo].ActionAvailabilityChanged += _AVModality_ActionAvailabilityChanged;

                //Accept the notification. If Lync UI is enabled, incoming call notification is closed.
                pConversation.Modalities[ModalityTypes.AudioVideo].Accept();

                //Connect the AV modality and begin to send and received AV stream.
                object[] asyncState = { pConversation.Modalities[ModalityTypes.AudioVideo], "CONNECT" };
                pConversation.Modalities[ModalityTypes.AudioVideo].BeginConnect(ModalityCallback, asyncState);
            }
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
