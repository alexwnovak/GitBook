using GalaSoft.MvvmLight.Messaging;
using GitModel;

namespace GitWrite.Messages
{
   public class AcceptChangesMessage : MessageBase
   {
      public string FilePath { get; }
      public CommitDocument CommitDocument { get; }

      public AcceptChangesMessage( string filePath, CommitDocument commitDocument )
      {
         FilePath = filePath;
         CommitDocument = commitDocument;
      }
   }
}
