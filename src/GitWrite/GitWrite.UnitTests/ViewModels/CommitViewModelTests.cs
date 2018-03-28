using GitModel;
using GitWrite.ViewModels;
using Moq;
using Xunit;

namespace GitWrite.UnitTests.ViewModels
{
   public class CommitViewModelTests
   {
      [Fact]
      public void AcceptCommand_Executing_SavesCommitDetails()
      {
         var commitDocument = new CommitDocument();
         var commitFileWriterMock = new Mock<ICommitFileWriter>();

         var viewModel = new CommitViewModel( "File.txt", commitDocument, commitFileWriterMock.Object );
         viewModel.AcceptCommand.Execute( null );

         commitFileWriterMock.Verify( cfw => cfw.ToFile( "File.txt", commitDocument ), Times.Once() );
      }
   }
}
