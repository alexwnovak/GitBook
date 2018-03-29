using GitModel;
using GitWrite.Services;
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
         var commitDocument = new CommitDocument( "Subject", null );
         var commitFileWriterMock = new Mock<ICommitFileWriter>();

         var viewModel = new CommitViewModel( "File.txt", commitDocument, commitFileWriterMock.Object, Mock.Of<IViewService>() );
         viewModel.AcceptCommand.Execute( null );

         commitFileWriterMock.Verify( cfw => cfw.ToFile( "File.txt", commitDocument ), Times.Once() );
      }

      [Fact]
      public void AcceptCommand_Executing_DismissesTheView()
      {
         var commitDocument = new CommitDocument( "Subject", null );
         var viewServiceMock = new Mock<IViewService>();

         var viewModel = new CommitViewModel( null, commitDocument, Mock.Of<ICommitFileWriter>(), viewServiceMock.Object );
         viewModel.AcceptCommand.Execute( null );

         viewServiceMock.Verify( vs => vs.CloseView(), Times.Once() );
      }

      [Fact]
      public void AcceptCommand_HasNoSubject_DisplaysHint()
      {
         var viewServiceMock = new Mock<IViewService>();

         var viewModel = new CommitViewModel( null, new CommitDocument(), Mock.Of<ICommitFileWriter>(), viewServiceMock.Object );
         viewModel.AcceptCommand.Execute( null );

         viewServiceMock.Verify( vs => vs.DisplaySubjectHint(), Times.Once() );
      }

      [Fact]
      public void DiscardCommand_Executing_ClearsTheCommitDetails()
      {
         var commitDocument = new CommitDocument();
         var commitFileWriterMock = new Mock<ICommitFileWriter>();

         var viewModel = new CommitViewModel( "File.txt", commitDocument, commitFileWriterMock.Object, Mock.Of<IViewService>() );
         viewModel.DiscardCommand.Execute( null );

         commitFileWriterMock.Verify( cfw => cfw.ToFile( "File.txt", CommitDocument.Empty ), Times.Once() );
      }

      [Fact]
      public void DiscardCommand_Executing_DismissesTheView()
      {
         var viewServiceMock = new Mock<IViewService>();

         var viewModel = new CommitViewModel( null, null, Mock.Of<ICommitFileWriter>(), viewServiceMock.Object );
         viewModel.DiscardCommand.Execute( null );

         viewServiceMock.Verify( vs => vs.CloseView(), Times.Once() );
      }
   }
}
