using System;
using GalaSoft.MvvmLight.Messaging;
using GitWrite.Receivers;
using FluentAssertions.Execution;

namespace GitWrite.UnitTests.Internal
{
   internal class ReceiverHarness<TReceiver> where TReceiver : IMessageReceiver
   {
      private readonly InstrumentedMessenger _messenger = new InstrumentedMessenger();

      public ReceiverHarness( TReceiver receiver )
      {
         receiver.SetMessenger( _messenger );
         receiver.Register();
      }

      public void Send<TMessage>( TMessage message ) where TMessage : MessageBase
      {
         _messenger.Send( message );
      }

      public void VerifySend<TMessage>( Func<TMessage, bool> predicate ) where TMessage : MessageBase
      {
         foreach ( var thing in _messenger.SentMessages )
         {
            if ( predicate( (TMessage) thing ) )
            {
               return;
            }
         }

         throw new AssertionFailedException( $"Receiver harness did not send message of type {typeof( TMessage ).Name}" );
      }
   }
}
