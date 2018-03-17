using GitModel;
using GitWrite.Messages;

namespace GitWrite.Receivers
{
   public class AcceptChangesReceiver : MessageReceiver<AcceptChangesMessage>
   {
      private readonly ICommitFileWriter _commitFileWriter;

      public AcceptChangesReceiver( ICommitFileWriter commitFileWriter ) => _commitFileWriter = commitFileWriter;

      protected override void OnReceive( AcceptChangesMessage message )
      {
         _commitFileWriter.ToFile( message.FilePath, message.CommitDocument );
         Send<ExitApplicationMessage>();
      }
   }
}
