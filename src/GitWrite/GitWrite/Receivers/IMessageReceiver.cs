using GalaSoft.MvvmLight.Messaging;

namespace GitWrite.Receivers
{
   public interface IMessageReceiver
   {
      void Register();
      void SetMessenger( IMessenger messenger );
   }
}
