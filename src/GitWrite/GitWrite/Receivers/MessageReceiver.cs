using GalaSoft.MvvmLight.Messaging;

namespace GitWrite.Receivers
{
   public abstract class MessageReceiver<T> where T : MessageBase
   {
      protected MessageReceiver()
      {
         Messenger.Default.Register<T>( this, OnReceive );
      }

      protected void Send<TMessageType>() where TMessageType : new()
      {
         Send( new TMessageType() );
      }

      protected void Send<TMessageType>( TMessageType message )
      {
         Messenger.Default.Send( message );
      } 

      protected abstract void OnReceive( T message );
   }
}
