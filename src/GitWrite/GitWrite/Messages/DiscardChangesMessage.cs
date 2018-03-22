using GalaSoft.MvvmLight.Messaging;

namespace GitWrite.Messages
{
   public class DiscardChangesMessage : MessageBase
   {
      public string FilePath { get; }

      public DiscardChangesMessage( string filePath ) => FilePath = filePath;
   }
}
