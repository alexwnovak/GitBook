namespace GitWrite.Messaging
{
   public interface IMessageReceiver
   {
      void Receive( object message );
   }
}
