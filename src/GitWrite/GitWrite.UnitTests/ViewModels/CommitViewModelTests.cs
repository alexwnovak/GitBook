using System.Linq;
using GitModel;
using GitWrite.Services;
using GitWrite.ViewModels;
using Xunit;
using Moq;
using FluentAssertions;

namespace GitWrite.UnitTests.ViewModels
{
   public class CommitViewModelTests
   {
      [Fact]
      public void AcceptCommand_CommitDetailsAreNotBlank_SavesCommitDetails()
      {
         var commitDocument = new CommitDocument( "Subject", new [] { "Body" } );

         var commitFileReaderMock = new Mock<ICommitFileReader>();
         commitFileReaderMock.Setup( cfr => cfr.FromFile( It.IsAny<string>() ) ).Returns( commitDocument );

         var commitFileWriterMock = new Mock<ICommitFileWriter>();

         var viewModel = new CommitViewModel( "File.txt", commitFileReaderMock.Object, commitFileWriterMock.Object, Mock.Of<IViewService>() );
         viewModel.InitializeCommand.Execute( null );
         viewModel.AcceptCommand.Execute( null );

         commitFileWriterMock.Verify( cfw => cfw.ToFile( "File.txt",
            It.Is<CommitDocument>( cd => cd.Subject == "Subject" && cd.Body[0] == "Body" ) ), Times.Once() );
      }

      [Fact]
      public void AcceptCommand_CommitDetailsAreNotBlank_DismissesTheView()
      {
         var commitFileReaderMock = new Mock<ICommitFileReader>();
         commitFileReaderMock.Setup( cfr => cfr.FromFile( It.IsAny<string>() ) ).Returns( new CommitDocument( "Subject", new string[1] ) );

         var viewServiceMock = new Mock<IViewService>();

         var viewModel = new CommitViewModel( null, commitFileReaderMock.Object, Mock.Of<ICommitFileWriter>(), viewServiceMock.Object );
         viewModel.InitializeCommand.Execute( null );
         viewModel.AcceptCommand.Execute( null );

         viewServiceMock.Verify( vs => vs.CloseViewAsync( true ), Times.Once() );
      }

      [Fact]
      public void AcceptCommand_HasNoSubject_DisplaysHint()
      {
         var commitFileReaderMock = new Mock<ICommitFileReader>();
         commitFileReaderMock.Setup( cfr => cfr.FromFile( It.IsAny<string>() ) ).Returns( CommitDocument.Empty );

         var viewServiceMock = new Mock<IViewService>();

         var viewModel = new CommitViewModel( null, commitFileReaderMock.Object, Mock.Of<ICommitFileWriter>(), viewServiceMock.Object );
         viewModel.InitializeCommand.Execute( null );
         viewModel.AcceptCommand.Execute( null );

         viewServiceMock.Verify( vs => vs.DisplaySubjectHint(), Times.Once() );
      }

      [Fact]
      public void DiscardCommand_AbandoningChanges_ClearsTheCommitDetails()
      {
         var commitFileWriterMock = new Mock<ICommitFileWriter>();

         var viewModel = new CommitViewModel( "File.txt", Mock.Of<ICommitFileReader>(), commitFileWriterMock.Object, Mock.Of<IViewService>() );
         viewModel.DiscardCommand.Execute( null );

         commitFileWriterMock.Verify( cfw => cfw.ToFile( "File.txt", CommitDocument.Empty ), Times.Once() );
      }

      [Fact]
      public void DiscardCommand_AbandoningChanges_DismissesTheView()
      {
         var viewServiceMock = new Mock<IViewService>();

         var viewModel = new CommitViewModel( null, null, Mock.Of<ICommitFileWriter>(), viewServiceMock.Object );
         viewModel.DiscardCommand.Execute( null );

         viewServiceMock.Verify( vs => vs.CloseViewAsync( false ), Times.Once() );
      }

      [Fact]
      public void DiscardChanges_AbortingTheDiscard_DoesNotExit()
      {
         var commitFileReaderMock = new Mock<ICommitFileReader>();
         commitFileReaderMock.Setup( cfr => cfr.FromFile( It.IsAny<string>() ) ).Returns( CommitDocument.Empty );

         var viewServiceMock = new Mock<IViewService>();
         viewServiceMock.Setup( vs => vs.ConfirmDiscard() ).Returns( ExitReason.Cancel );

         var viewModel = new CommitViewModel( null, commitFileReaderMock.Object, Mock.Of<ICommitFileWriter>(), viewServiceMock.Object );
         viewModel.InitializeCommand.Execute( null );
         viewModel.CommitModel.Subject = "Something different";
         viewModel.DiscardCommand.Execute( null );

         viewServiceMock.Verify( vs => vs.CloseViewAsync( false ), Times.Never() );
      }
   }
}
