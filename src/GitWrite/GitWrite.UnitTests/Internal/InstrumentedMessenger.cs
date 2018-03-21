using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;

namespace GitWrite.UnitTests.Internal
{
   internal class InstrumentedMessenger : Messenger
   {
      private readonly List<object> _sentMessages = new List<object>();
      public IEnumerable<object> SentMessages => _sentMessages;

      public override void Send<TMessage>( TMessage message )
      {
         _sentMessages.Add( message );
         base.Send( message );
      }
   }
}
