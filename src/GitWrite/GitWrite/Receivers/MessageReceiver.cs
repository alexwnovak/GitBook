using GalaSoft.MvvmLight.Messaging;

namespace GitWrite.Receivers
{
   public abstract class MessageReceiver<T> : IMessageReceiver where T : MessageBase
   {
      private IMessenger _messenger;

      protected MessageReceiver() => _messenger = Messenger.Default;

      public void SetMessenger( IMessenger messenger ) => _messenger = messenger;

      public void Register() => _messenger.Register<T>( this, OnReceive );

      protected void Send<TMessageType>() where TMessageType : MessageBase, new() =>
         _messenger.Send( new TMessageType() );

      protected void Send<TMessageType>( TMessageType message ) where TMessageType : MessageBase =>
         _messenger.Send( message );

      protected abstract void OnReceive( T message );
   }
}
