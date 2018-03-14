using GalaSoft.MvvmLight.Messaging;
using GitModel;

namespace GitWrite.Messages
{
   public class AcceptChangesMessage : MessageBase
   {
      public CommitDocument CommitDocument
      {
         get;
      }

      public AcceptChangesMessage( CommitDocument commitDocument )
      {
         CommitDocument = commitDocument;
      }
   }
}
