using System.Windows;
using GitWrite.Messages;

namespace GitWrite.Receivers
{
   public class ShutdownRequestedMessageReceiver : MessageReceiver<ShutdownRequestedMessage>
   {
      protected override void OnReceive( ShutdownRequestedMessage message )
      {
         Application.Current.Shutdown();
      }
   }
}
