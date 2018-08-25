using AutoFixture.Xunit2;
using Moq;
using Xunit;
using DeepEqual.Syntax;
using GitModel;
using GitWrite.ViewModels;
using GitWrite.UnitTests.Infrastructure;
using GitWrite.Services;

namespace GitWrite.UnitTests.ViewModels
{
   public class CommitViewModelTests
   {
      [Theory, AutoMoqData]
      public void AcceptCommand_CommitDetailsAreNotBlank_SavesCommitDetails(
         [Frozen] Mock<ICommitFileReader> commitFileReaderMock,
         [Frozen] Mock<ICommitFileWriter> commitFileWriterMock,
         CommitDocument commitDocument,
         CommitViewModel sut )
      {
         commitFileReaderMock.Setup( cfr => cfr.FromFile( It.IsAny<string>() ) ).Returns( commitDocument );

         sut.InitializeCommand.Execute( null );
         sut.AcceptCommand.Execute( null );

         commitFileWriterMock.Verify( cfw => cfw.ToFile(
            sut.CommitFilePath,
            It.Is<CommitDocument>( cd => cd.IsDeepEqual( commitDocument, null ) ) ), Times.Once() );
      }

      [Theory, AutoMoqData]
      public void AcceptCommand_CommitDetailsAreNotBlank_DismissesTheView(
         [Frozen] Mock<ICommitFileReader> commitFileReaderMock,
         [Frozen] Mock<IViewService> viewServiceMock,
         CommitDocument commitDocument,
         CommitViewModel sut )
      {
         commitFileReaderMock.Setup( cfr => cfr.FromFile( It.IsAny<string>() ) ).Returns( commitDocument );

         sut.InitializeCommand.Execute( null );
         sut.AcceptCommand.Execute( null );

         viewServiceMock.Verify( vs => vs.CloseViewAsync( true ), Times.Once() );
      }

      [Theory, AutoMoqData]
      public void AcceptCommand_HasNoSubject_DisplaysHint(
         [Frozen] Mock<ICommitFileReader> commitFileReaderMock,
         [Frozen] Mock<IViewService> viewServiceMock,
         CommitViewModel sut )
      {
         commitFileReaderMock.Setup( cfr => cfr.FromFile( It.IsAny<string>() ) ).Returns( CommitDocument.Empty );

         sut.InitializeCommand.Execute( null );
         sut.AcceptCommand.Execute( null );

         viewServiceMock.Verify( vs => vs.DisplaySubjectHint(), Times.Once() );
      }

      [Theory, AutoMoqData]
      public void DiscardCommand_AbandoningChanges_ClearsTheCommitDetails(
         [Frozen] Mock<ICommitFileWriter> commitFileWriterMock,
         CommitViewModel sut )
      {
         sut.DiscardCommand.Execute( null );

         commitFileWriterMock.Verify( cfw => cfw.ToFile( sut.CommitFilePath, CommitDocument.Empty ), Times.Once() );
      }

      [Theory, AutoMoqData]
      public void DiscardCommand_AbandoningChanges_DismissesTheView(
         [Frozen] Mock<IViewService> viewServiceMock,
         CommitViewModel sut )
      {
         sut.DiscardCommand.Execute( null );

         viewServiceMock.Verify( vs => vs.CloseViewAsync( false ), Times.Once() );
      }

      [Theory, AutoMoqData]
      public void DiscardCommand_AbortingTheDiscard_DoesNotExit(
         [Frozen] Mock<ICommitFileReader> commitFileReaderMock,
         [Frozen] Mock<IViewService> viewServiceMock,
         CommitViewModel sut )
      {
         commitFileReaderMock.Setup( cfr => cfr.FromFile( It.IsAny<string>() ) ).Returns( CommitDocument.Empty );
         viewServiceMock.Setup( vs => vs.ConfirmDiscard() ).Returns( DialogResult.Cancel );

         sut.InitializeCommand.Execute( null );
         sut.CommitModel.Subject = "Something different";
         sut.DiscardCommand.Execute( null );

         viewServiceMock.Verify( vs => vs.CloseViewAsync( false ), Times.Never() );
      }

      [Theory, AutoMoqData]
      public void DiscardCommand_ContinuingWithDiscard_DismissesTheView(
         [Frozen] Mock<ICommitFileReader> commitFileReaderMock,
         [Frozen] Mock<IViewService> viewServiceMock,
         CommitViewModel sut )
      {
         commitFileReaderMock.Setup( cfr => cfr.FromFile( It.IsAny<string>() ) ).Returns( CommitDocument.Empty );
         viewServiceMock.Setup( vs => vs.ConfirmDiscard() ).Returns( DialogResult.Discard );

         sut.InitializeCommand.Execute( null );
         sut.CommitModel.Subject = "Something different";
         sut.DiscardCommand.Execute( null );

         viewServiceMock.Verify( vs => vs.CloseViewAsync( false ), Times.Once() );
      }

      [Theory, AutoMoqData]
      public void DiscardCommand_ContinuingWithDiscard_ClearsTheCommitDetails(
         [Frozen] Mock<ICommitFileReader> commitFileReaderMock,
         [Frozen] Mock<ICommitFileWriter> commitFileWriterMock,
         [Frozen] Mock<IViewService> viewServiceMock,
         CommitViewModel sut )
      {
         commitFileReaderMock.Setup( cfr => cfr.FromFile( It.IsAny<string>() ) ).Returns( CommitDocument.Empty );
         viewServiceMock.Setup( vs => vs.ConfirmDiscard() ).Returns( DialogResult.Discard );

         sut.InitializeCommand.Execute( null );
         sut.CommitModel.Subject = "Something different";
         sut.DiscardCommand.Execute( null );

         commitFileWriterMock.Verify( cfw => cfw.ToFile( sut.CommitFilePath, CommitDocument.Empty ), Times.Once() );
      }

      [Theory, AutoMoqData]
      public void DiscardCommand_SavingChangesAfterall_WritesChanges(
         [Frozen] Mock<ICommitFileReader> commitFileReaderMock,
         [Frozen] Mock<ICommitFileWriter> commitFileWriterMock,
         [Frozen] Mock<IViewService> viewServiceMock,
         CommitViewModel sut )
      {
         commitFileReaderMock.Setup( cfr => cfr.FromFile( It.IsAny<string>() ) ).Returns( CommitDocument.Empty );
         viewServiceMock.Setup( vs => vs.ConfirmDiscard() ).Returns( DialogResult.Save );

         sut.InitializeCommand.Execute( null );
         sut.CommitModel.Subject = "Something different";
         sut.DiscardCommand.Execute( null );

         commitFileWriterMock.Verify( cfw => cfw.ToFile( sut.CommitFilePath, It.Is<CommitDocument>(
            cd => cd.Subject == "Something different" ) ), Times.Once() );
      }
   }
}
