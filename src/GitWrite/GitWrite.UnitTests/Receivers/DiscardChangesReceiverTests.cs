using GitModel;
using GitWrite.Messages;
using GitWrite.Receivers;
using GitWrite.UnitTests.Internal;
using Moq;
using Xunit;

namespace GitWrite.UnitTests.Receivers
{
   public class DiscardChangesReceiverTests
   {
      [Fact]
      public void OnReceive_ChangesAreDiscarded_EmptyCommitFileIsWritten()
      {
         var commitFileWriterMock = new Mock<ICommitFileWriter>();
         var receiver = new DiscardChangesReceiver( commitFileWriterMock.Object );

         var harness = new ReceiverHarness<DiscardChangesReceiver>( receiver );
         harness.Send( new DiscardChangesMessage( "file" ) );

         commitFileWriterMock.Verify( cfw => cfw.ToFile( "file", CommitDocument.Empty ), Times.Once() );
      }

      [Fact]
      public void OnReceive_ChangesAreDiscarded_ExitsApplication()
      {
         var receiver = new DiscardChangesReceiver( Mock.Of<ICommitFileWriter>() );

         var message = new DiscardChangesMessage( null );
         var harness = new ReceiverHarness<DiscardChangesReceiver>( receiver );
         harness.Send( message );

         harness.VerifySend<ExitApplicationMessage>();
      }
   }
}
