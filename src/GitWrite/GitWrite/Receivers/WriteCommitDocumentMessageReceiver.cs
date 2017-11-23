using GitModel;
using GitWrite.Messages;

namespace GitWrite.Receivers
{
   public class WriteCommitDocumentMessageReceiver : MessageReceiver<WriteCommitDocumentMessage>
   {
      protected override void OnReceive( WriteCommitDocumentMessage message )
      {
         var commitFileWriter = new CommitFileWriter();
         commitFileWriter.ToFile( message.FilePath, message.CommitDocument );
      }
   }
}
