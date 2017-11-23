using GalaSoft.MvvmLight.Messaging;
using GitModel;

namespace GitWrite.Messages
{
   public class WriteCommitDocumentMessage : MessageBase
   {
      public string FilePath
      {
         get;
      }

      public CommitDocument CommitDocument
      {
         get;
      }

      public WriteCommitDocumentMessage( string filePath, CommitDocument commitDocument )
      {
         FilePath = filePath;
         CommitDocument = commitDocument;
      }
   }
}
