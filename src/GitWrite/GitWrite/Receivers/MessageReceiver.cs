using GalaSoft.MvvmLight.Messaging;

namespace GitWrite.Receivers
{
   public abstract class MessageReceiver<T> where T : MessageBase
   {
      protected MessageReceiver()
      {
         Messenger.Default.Register<T>( this, OnReceive );
      }

      protected abstract void OnReceive( T message );
   }
}
