using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;

namespace GitWrite.UnitTests.Internal
{
   internal class InstrumentedMessenger : Messenger
   {
      private readonly List<MessageBase> _sentMessages = new List<MessageBase>();
      public IEnumerable<MessageBase> SentMessages => _sentMessages;

      public new void Send<TMessage>( TMessage message ) where TMessage : MessageBase
      {
         _sentMessages.Add( message );
         base.Send( message );
      }
   }
}
