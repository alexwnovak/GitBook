using System.Linq;
using System.Runtime.InteropServices;
using GitModel;
using GitWrite.Services;
using GitWrite.ViewModels;
using Xunit;
using Moq;
using FluentAssertions;

namespace GitWrite.UnitTests.ViewModels
{
   public class CommitViewModelTestsOld
   {
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
         viewServiceMock.Setup( vs => vs.ConfirmDiscard() ).Returns( DialogResult.Cancel );

         var viewModel = new CommitViewModel( null, commitFileReaderMock.Object, Mock.Of<ICommitFileWriter>(), viewServiceMock.Object );
         viewModel.InitializeCommand.Execute( null );
         viewModel.CommitModel.Subject = "Something different";
         viewModel.DiscardCommand.Execute( null );

         viewServiceMock.Verify( vs => vs.CloseViewAsync( false ), Times.Never() );
      }

      [Fact]
      public void DiscardChanges_ContinuingWithDiscard_DismissesTheView()
      {
         var commitFileReaderMock = new Mock<ICommitFileReader>();
         commitFileReaderMock.Setup( cfr => cfr.FromFile( It.IsAny<string>() ) ).Returns( CommitDocument.Empty );

         var viewServiceMock = new Mock<IViewService>();
         viewServiceMock.Setup( vs => vs.ConfirmDiscard() ).Returns( DialogResult.Discard );

         var viewModel = new CommitViewModel( null, commitFileReaderMock.Object, Mock.Of<ICommitFileWriter>(), viewServiceMock.Object );
         viewModel.InitializeCommand.Execute( null );
         viewModel.CommitModel.Subject = "Something different";
         viewModel.DiscardCommand.Execute( null );

         viewServiceMock.Verify( vs => vs.CloseViewAsync( false ), Times.Once() );
      }

      [Fact]
      public void DiscardChanges_ContinuingWithDiscard_ClearsTheCommitDetails()
      {
         var commitFileReaderMock = new Mock<ICommitFileReader>();
         commitFileReaderMock.Setup( cfr => cfr.FromFile( It.IsAny<string>() ) ).Returns( CommitDocument.Empty );

         var commitFileWriterMock = new Mock<ICommitFileWriter>();

         var viewServiceMock = new Mock<IViewService>();
         viewServiceMock.Setup( vs => vs.ConfirmDiscard() ).Returns( DialogResult.Discard );

         var viewModel = new CommitViewModel( "File.txt", commitFileReaderMock.Object, commitFileWriterMock.Object, viewServiceMock.Object );
         viewModel.InitializeCommand.Execute( null );
         viewModel.CommitModel.Subject = "Something different";
         viewModel.DiscardCommand.Execute( null );

         commitFileWriterMock.Verify( cfw => cfw.ToFile( "File.txt", CommitDocument.Empty ), Times.Once() );
      }

      [Fact]
      public void DiscardChanges_SavingChangesAfterall_WritesChanges()
      {
         var commitFileReaderMock = new Mock<ICommitFileReader>();
         commitFileReaderMock.Setup( cfr => cfr.FromFile( It.IsAny<string>() ) ).Returns( CommitDocument.Empty );

         var commitFileWriterMock = new Mock<ICommitFileWriter>();

         var viewServiceMock = new Mock<IViewService>();
         viewServiceMock.Setup( vs => vs.ConfirmDiscard() ).Returns( DialogResult.Save );

         var viewModel = new CommitViewModel( "File.txt", commitFileReaderMock.Object, commitFileWriterMock.Object, viewServiceMock.Object );
         viewModel.InitializeCommand.Execute( null );
         viewModel.CommitModel.Subject = "Something different";
         viewModel.DiscardCommand.Execute( null );

         commitFileWriterMock.Verify( cfw => cfw.ToFile( "File.txt", It.Is<CommitDocument>(
            cd => cd.Subject == "Something different" ) ), Times.Once() );
      }

   }
}
