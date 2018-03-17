using GitWrite.Messages;

namespace GitWrite.Receivers
{
   public class DiscardChangesReceiver : MessageReceiver<DiscardChangesMessage>
   {
      protected override void OnReceive( DiscardChangesMessage message )
      {
      }
   }
}
