using System;
using GitWrite.Messages;

namespace GitWrite.Receivers
{
   public class AcceptChangesReceiver : MessageReceiver<AcceptChangesMessage>
   {
      protected override void OnReceive( AcceptChangesMessage message )
      {
      }
   }
}
