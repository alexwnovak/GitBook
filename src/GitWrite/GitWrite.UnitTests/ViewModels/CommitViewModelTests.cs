//using System;
//using System.Threading.Tasks;
//using GalaSoft.MvvmLight.Messaging;
//using Xunit;
//using Moq;
//using FluentAssertions;
//using GitModel;
//using GitWrite.Messages;
//using GitWrite.Services;
//using GitWrite.ViewModels;

//namespace GitWrite.UnitTests.ViewModels
//{
//   public class CommitViewModelTests
//   {
//      [Fact]
//      public void Constructor_IncomingSubjectIsNull_ViewModelShortMessageMatches()
//      {
//         var commitDocument = new CommitDocument
//         {
//            Subject = null
//         };

//         var commitViewModel = new CommitViewModel( null, null, null, commitDocument, null, Mock.Of<IMessenger>() );

//         commitViewModel.ShortMessage.Should().BeNull();
//      }

//      [Fact]
//      public void Constructor_CommitDocumentHasShortMessage_ViewModelReadsShortMessage()
//      {
//         const string shortMessage = "Short commit message";

//         var commitDocument = new CommitDocument
//         {
//            Subject = shortMessage
//         };

//         var commitViewModel = new CommitViewModel( null, null, null, commitDocument, null, Mock.Of<IMessenger>() );

//         commitViewModel.ShortMessage.Should().Be( shortMessage );
//      }

//      [Fact]
//      public void Constructor_HasSingleLineBody_ViewModelReadsTheLongMessage()
//      {
//         const string longMessage = "Long message here";

//         var commitDocument = new CommitDocument
//         {
//            Body = new[] { longMessage }
//         };

//         var commitViewModel = new CommitViewModel( null, null, null, commitDocument, null, Mock.Of<IMessenger>() );

//         commitViewModel.ExtraCommitText.Should().Be( longMessage );
//      }

//      [Fact]
//      public void Constructor_IncomingSubjectIsNull_IsNotAmending()
//      {
//         var commitDocument = new CommitDocument
//         {
//            Subject = null
//         };

//         var commitViewModel = new CommitViewModel( null, null, null, commitDocument, null, Mock.Of<IMessenger>() );

//         commitViewModel.IsAmending.Should().BeFalse();
//      }

//      [Fact]
//      public void Constructor_HasIncomingShortMessage_IsAmending()
//      {
//         var commitDocument = new CommitDocument
//         {
//            Subject = "Not empty"
//         };

//         var commitViewModel = new CommitViewModel( null, null, null, commitDocument, null, Mock.Of<IMessenger>() );

//         commitViewModel.IsAmending.Should().BeTrue();
//      }

//      [Fact]
//      public void ViewLoaded_DoesNotHaveExtraNotes_DoesNotSendExpansionMessage()
//      {
//         var messengerMock = new Mock<IMessenger>();

//         var commitViewModel = new CommitViewModel( null, null, null, null, null, messengerMock.Object );

//         commitViewModel.ViewLoaded();

//         messengerMock.Verify( m => m.Send( It.IsAny<ExpansionRequestedMessage>() ), Times.Never() );
//      }

//      [Fact]
//      public void ViewLoaded_HasExtraNotes_SendsExpansionMessage()
//      {
//         var messengerMock = new Mock<IMessenger>();

//         var commitViewModel = new CommitViewModel( null, null, null, null, null, messengerMock.Object )
//         {
//            ExtraCommitText = "Extra notes"
//         };

//         commitViewModel.ViewLoaded();

//         messengerMock.Verify( m => m.Send( It.IsAny<ExpansionRequestedMessage>() ), Times.Once() );
//      }

//      [Fact]
//      public void PasteCommand_ClipboardHasOneLineOfText_SetsShortMessage()
//      {
//         const string clipboardText = "Some text";

//         // Setup

//         var clipboardServiceMock = new Mock<IClipboardService>();
//         clipboardServiceMock.Setup( cs => cs.GetText() ).Returns( clipboardText );

//         // Test

//         var viewModel = new CommitViewModel( null, null, clipboardServiceMock.Object, null, null, Mock.Of<IMessenger>() );

//         viewModel.PasteCommand.Execute( null );

//         // Assert

//         viewModel.ShortMessage.Should().Be( clipboardText );
//      }

//      [Fact]
//      public void PasteCommand_ClipboardHasTwoLinesWithNoBlankLine_SetsBothMessages()
//      {
//         string clipboardText = $"First line{Environment.NewLine}Second line";

//         // Setup

//         var clipboardServiceMock = new Mock<IClipboardService>();
//         clipboardServiceMock.Setup( cs => cs.GetText() ).Returns( clipboardText );

//         // Test

//         var viewModel = new CommitViewModel( null, null, clipboardServiceMock.Object, null, null, Mock.Of<IMessenger>() );

//         viewModel.PasteCommand.Execute( null );

//         // Assert

//         viewModel.ShortMessage.Should().Be( "First line" );
//         viewModel.ExtraCommitText.Should().Be( "Second line" );
//      }

//      [Fact]
//      public void PasteCommand_ClipboardHasOneLineEndingWithLineBreak_SetsShortMessage()
//      {
//         string clipboardText = $"First line{Environment.NewLine}";

//         // Setup

//         var clipboardServiceMock = new Mock<IClipboardService>();
//         clipboardServiceMock.Setup( cs => cs.GetText() ).Returns( clipboardText );

//         // Test

//         var viewModel = new CommitViewModel( null, null, clipboardServiceMock.Object, null, null, Mock.Of<IMessenger>() );

//         viewModel.PasteCommand.Execute( null );

//         // Assert

//         viewModel.ShortMessage.Should().Be( "First line" );
//         viewModel.ExtraCommitText.Should().BeNull();
//      }

//      [Fact]
//      public void PasteCommand_ClipboardHasOneLineEndingWithTwoLineBreaks_SetsShortMessage()
//      {
//         string clipboardText = $"First line{Environment.NewLine}{Environment.NewLine}";

//         // Setup

//         var clipboardServiceMock = new Mock<IClipboardService>();
//         clipboardServiceMock.Setup( cs => cs.GetText() ).Returns( clipboardText );

//         // Test

//         var viewModel = new CommitViewModel( null, null, clipboardServiceMock.Object, null, null, Mock.Of<IMessenger>() );

//         viewModel.PasteCommand.Execute( null );

//         // Assert

//         viewModel.ShortMessage.Should().Be( "First line" );
//         viewModel.ExtraCommitText.Should().BeNull();
//      }

//      [Fact]
//      public void PasteCommand_ClipboardHasBothMessagesSeparatedByBlankLine_SetsBothMessages()
//      {
//         string clipboardText = $"Short message{Environment.NewLine}{Environment.NewLine}Secondary notes";

//         // Setup

//         var clipboardServiceMock = new Mock<IClipboardService>();
//         clipboardServiceMock.Setup( cs => cs.GetText() ).Returns( clipboardText );

//         // Test

//         var viewModel = new CommitViewModel( null, null, clipboardServiceMock.Object, null, null, Mock.Of<IMessenger>() );

//         viewModel.PasteCommand.Execute( null );

//         // Assert

//         viewModel.ShortMessage.Should().Be( "Short message" );
//         viewModel.ExtraCommitText.Should().Be( "Secondary notes" );
//      }

//      [Fact]
//      public void PasteCommand_ClipboardHasBothMessagesEndingWithLineBreaks_TrimsEndLineBreaks()
//      {
//         string clipboardText = $"Short message{Environment.NewLine}Secondary notes{Environment.NewLine}{Environment.NewLine}";

//         // Setup

//         var clipboardServiceMock = new Mock<IClipboardService>();
//         clipboardServiceMock.Setup( cs => cs.GetText() ).Returns( clipboardText );

//         // Test

//         var viewModel = new CommitViewModel( null, null, clipboardServiceMock.Object, null, null, Mock.Of<IMessenger>() );

//         viewModel.PasteCommand.Execute( null );

//         // Assert

//         viewModel.ShortMessage.Should().Be( "Short message" );
//         viewModel.ExtraCommitText.Should().Be( "Secondary notes" );
//      }

//      [Fact]
//      public async Task PasteCommand_ClipboardHasBothMessagesAndExtraNotesSpanMultipleLines_SetsBothMessage()
//      {
//         const string shortMessage = "First line message";
//         string extraMessage = $"Secondary notes, first line{Environment.NewLine}{Environment.NewLine}Second line{Environment.NewLine}Third line";
//         string clipboardText = $"{shortMessage}{Environment.NewLine}{Environment.NewLine}{extraMessage}";

//         // Setup

//         await Task.Yield();

//         var clipboardServiceMock = new Mock<IClipboardService>();
//         clipboardServiceMock.Setup( cs => cs.GetText() ).Returns( clipboardText );

//         // Test

//         var viewModel = new CommitViewModel( null, null, clipboardServiceMock.Object, null, null, Mock.Of<IMessenger>() );

//         viewModel.PasteCommand.Execute( null );

//         // Assert

//         viewModel.ShortMessage.Should().Be( shortMessage );
//         viewModel.ExtraCommitText.Should().Be( extraMessage );
//      }

//      [Fact]
//      public void Title_HappyPath_TitleContainsBranchName()
//      {
//         const string branchName = "master";

//         // Setup

//         var gitServiceMock = new Mock<IGitService>();
//         gitServiceMock.Setup( gs => gs.GetCurrentBranchName() ).Returns( branchName );

//         // Test

//         var viewModel = new CommitViewModel( null, null, null, null, gitServiceMock.Object, Mock.Of<IMessenger>() );
//         string title = viewModel.Title;

//         // Assert

//         title.Should().Contain( branchName );
//      }

//      [Fact]
//      public void AbortCommand_ExpandedFlagIsSet_SendsCollapseRequestedMessage()
//      {
//         var messengerMock = new Mock<IMessenger>();
//         messengerMock.Setup( m => m.Send( It.IsAny<ExitRequestedMessage>() ) ).Callback<ExitRequestedMessage>( m => m.Complete() );

//         var viewModel = new CommitViewModel( null, null, null, new CommitDocument(), null, messengerMock.Object )
//         {
//            IsExpanded = true
//         };

//         viewModel.AbortCommand.Execute( null );

//         messengerMock.Verify( m => m.Send( It.IsAny<CollapseRequestedMessage>() ), Times.Once() );
//      }

//      [Fact]
//      public void SaveCommand_ShortMessageIsEmpty_RaisesShakeRequestedEvent()
//      {
//         var messengerMock = new Mock<IMessenger>();

//         var commitDocument = new CommitDocument
//         {
//            Subject = string.Empty
//         };

//         var viewModel = new CommitViewModel( null, null, null, commitDocument, null, messengerMock.Object )
//         {
//            IsExiting = false
//         };

//         viewModel.SaveCommand.Execute( null );

//         messengerMock.Verify( m => m.Send( It.IsAny<ShakeRequestedMessage>()), Times.Once() );
//      }
//   }
//}
