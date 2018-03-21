using GitModel;
using GitWrite.Messages;
using GitWrite.Receivers;
using GitWrite.UnitTests.Internal;
using Xunit;
using Moq;

namespace GitWrite.UnitTests.Receivers
{
   public class AcceptChangesReceiverTests
   {
      [Fact]
      public void OnReceive_ChangesAreAccepted_ChangesAreWrittenToDisk()
      {
         var commitFileWriterMock = new Mock<ICommitFileWriter>();
         var receiver = new AcceptChangesReceiver( commitFileWriterMock.Object );

         var commitDocument = new CommitDocument( "Subject", new [] { "Line1", "Line2" } );
         var message = new AcceptChangesMessage( "File path", commitDocument );

         var harness = new ReceiverHarness<AcceptChangesReceiver>( receiver );
         harness.Send( message );
         
         commitFileWriterMock.Verify( cfw => cfw.ToFile( "File path", commitDocument ), Times.Once() );
      }

      [Fact]
      public void OnReceive_ChangesAreAccepted_TheApplicationExits()
      {
         var commitFileWriterMock = new Mock<ICommitFileWriter>();
         var receiver = new AcceptChangesReceiver( commitFileWriterMock.Object );

         var commitDocument = new CommitDocument( "Subject", new[] { "Line1", "Line2" } );
         var message = new AcceptChangesMessage( "File path", commitDocument );

         var harness = new ReceiverHarness<AcceptChangesReceiver>( receiver );
         harness.Send( message );

         harness.VerifySend<ExitApplicationMessage>();
      }
   }
}
