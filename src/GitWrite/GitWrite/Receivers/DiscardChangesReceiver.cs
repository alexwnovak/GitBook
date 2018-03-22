using GitModel;
using GitWrite.Messages;

namespace GitWrite.Receivers
{
   public class DiscardChangesReceiver : MessageReceiver<DiscardChangesMessage>
   {
      private readonly ICommitFileWriter _commitFileWriter;

      public DiscardChangesReceiver( ICommitFileWriter commitFileWriter )
      {
         _commitFileWriter = commitFileWriter;
      }

      protected override void OnReceive( DiscardChangesMessage message )
      {
         _commitFileWriter.ToFile( message.FilePath, CommitDocument.Empty );
         Send<ExitApplicationMessage>();
      }
   }
}
