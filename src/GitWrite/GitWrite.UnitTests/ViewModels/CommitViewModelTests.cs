﻿using System;
using System.Threading.Tasks;
using System.Windows.Input;
using FluentAssertions;
using GalaSoft.MvvmLight.Command;
using GitModel;
using Moq;
using Xunit;
using GitWrite.Services;
using GitWrite.UnitTests.Helpers;
using GitWrite.ViewModels;

namespace GitWrite.UnitTests.ViewModels
{
   public class CommitViewModelTests
   {
      [Fact]
      public void Constructor_IncomingSubjectIsNull_ViewModelShortMessageMatches()
      {
         var commitDocument = new CommitDocument
         {
            Subject = null
         };

         var commitViewModel = new CommitViewModel( null, null, null, null, commitDocument, null, null );

         commitViewModel.ShortMessage.Should().BeNull();
      }

      [Fact]
      public void Constructor_CommitDocumentHasShortMessage_ViewModelReadsShortMessage()
      {
         const string shortMessage = "Short commit message";

         var commitDocument = new CommitDocument
         {
            Subject = shortMessage
         };

         var commitViewModel = new CommitViewModel( null, null, null, null, commitDocument, null, null );

         commitViewModel.ShortMessage.Should().Be( shortMessage );
      }

      [Fact]
      public void Constructor_HasSingleLineBody_ViewModelReadsTheLongMessage()
      {
         const string longMessage = "Long message here";

         var commitDocument = new CommitDocument
         {
            Body = new[] { longMessage }
         };

         var commitViewModel = new CommitViewModel( null, null, null, null, commitDocument, null, null );

         commitViewModel.ExtraCommitText.Should().Be( longMessage );
      }

      [Fact]
      public void Constructor_IncomingSubjectIsNull_IsNotAmending()
      {
         var commitDocument = new CommitDocument
         {
            Subject = null
         };

         var commitViewModel = new CommitViewModel( null, null, null, null, commitDocument, null, null );

         commitViewModel.IsAmending.Should().BeFalse();
      }

      [Fact]
      public void Constructor_HasIncomingShortMessage_IsAmending()
      {
         var commitDocument = new CommitDocument
         {
            Subject = "Not empty"
         };

         var commitViewModel = new CommitViewModel( null, null, null, null, commitDocument, null, null );

         commitViewModel.IsAmending.Should().BeTrue();
      }

      [Fact]
      public void ViewLoaded_DoesNotHaveExtraNotes_DoesNotRaiseExpansionEvent()
      {
         bool expanded = false;

         var commitViewModel = new CommitViewModel( null, null, null, null, null, null, null );
         commitViewModel.AsyncExpansionRequested += ( sender, e ) =>
         {
            expanded = true;
            return Task.CompletedTask;
         };

         commitViewModel.ViewLoaded();

         expanded.Should().BeFalse();
      }

      [Fact]
      public void ViewLoaded_HasExtraNotes_RaisesExpansionEvent()
      {
         bool expanded = false;

         var commitViewModel = new CommitViewModel( null, null, null, null, null, null, null )
         {
            ExtraCommitText = "Extra notes"
         };

         commitViewModel.AsyncExpansionRequested += ( sender, e ) =>
         {
            expanded = true;
            return Task.CompletedTask;
         };
         commitViewModel.ViewLoaded();

         expanded.Should().BeTrue();
      }

      [Fact]
      public void SaveCommand_ShortMessageIsBlank_RaisesShakeRequested()
      {
         bool shakeRequestedRaised = false;

         var commitDocument = new CommitDocument
         {
            Subject = string.Empty
         };

         var commitViewModel = new CommitViewModel( null, null, null, null, commitDocument, null, null );

         commitViewModel.AsyncShakeRequested += ( _, __ ) =>
         {
            shakeRequestedRaised = true;
            return Task.CompletedTask;
         };

         commitViewModel.SaveCommand.Execute( null );

         shakeRequestedRaised.Should().BeTrue();
      }

      [Fact]
      public void ExpandCommand_IsNotExpanded_SetsExpandedFlag()
      {
         var commitViewModel = new CommitViewModel( null, null, null, null, null, null, null )
         {
            IsExpanded = false
         };

         commitViewModel.ExpandCommand.Execute( null );

         commitViewModel.IsExpanded.Should().BeTrue();
      }

      [Fact]
      public void ExpandCommand_IsNotExpanded_RaisesExpansionRequestedEvent()
      {
         bool expansionEventRaised = false;

         var commitViewModel = new CommitViewModel( null, null, null, null, null, null, null )
         {
            IsExpanded = false
         };

         commitViewModel.AsyncExpansionRequested += ( sender, e ) =>
         {
            expansionEventRaised = true;
            return Task.CompletedTask;
         };

         commitViewModel.ExpandCommand.Execute( null );

         expansionEventRaised.Should().BeTrue();
      }

      [Fact]
      public void ExpandCommand_IsAlreadyExpanded_DoesNotChangeExpandedFlag()
      {
         var commitViewModel = new CommitViewModel( null, null, null, null, null, null, null )
         {
            IsExpanded = true
         };

         commitViewModel.ExpandCommand.Execute( null );

         commitViewModel.IsExpanded.Should().BeTrue();
      }

      [Fact]
      public void ExpandCommand_IsAlreadyExpanded_DoesNotRaiseExpansionRequestedEvent()
      {
         bool expansionEventRaised = false;

         var commitViewModel = new CommitViewModel( null, null, null, null, null, null, null )
         {
            IsExpanded = true
         };

         commitViewModel.AsyncExpansionRequested += ( sender, e ) =>
         {
            expansionEventRaised = true;
            return Task.CompletedTask;
         };

         commitViewModel.ExpandCommand.Execute( null );

         expansionEventRaised.Should().BeFalse();
      }

      [Fact]
      public void ExpandCommand_IsExitingFlagSetButIsNotExpanded_DoesNotChangeExpandedFlag()
      {
         var commitViewModel = new CommitViewModel( null, null, null, null, null, null, null )
         {
            IsExiting = true,
            IsExpanded = false
         };

         commitViewModel.ExpandCommand.Execute( null );

         commitViewModel.IsExpanded.Should().BeFalse();
      }

      [Fact]
      public void ExpandCommand_IsExitingFlagSetAndIsExpanded_DoesNotRaiseExpansionRequestedEvent()
      {
         bool expansionEventRaised = false;

         var commitViewModel = new CommitViewModel( null, null, null, null, null, null, null )
         {
            IsExiting = true,
            IsExpanded = true
         };

         commitViewModel.AsyncExpansionRequested += ( sender, e ) =>
         {
            expansionEventRaised = true;
            return Task.CompletedTask;
         };

         commitViewModel.ExpandCommand.Execute( null );

         expansionEventRaised.Should().BeFalse();
      }

      [Fact]
      public void PasteCommand_ClipboardHasOneLineOfText_SetsShortMessage()
      {
         const string clipboardText = "Some text";

         // Setup

         var clipboardServiceMock = new Mock<IClipboardService>();
         clipboardServiceMock.Setup( cs => cs.GetText() ).Returns( clipboardText );

         // Test

         var viewModel = new CommitViewModel( null, null, null, clipboardServiceMock.Object, null, null, null );

         viewModel.PasteCommand.Execute( null );

         // Assert

         viewModel.ShortMessage.Should().Be( clipboardText );
      }

      [Fact]
      public void PasteCommand_ClipboardHasTwoLinesWithNoBlankLine_SetsBothMessages()
      {
         string clipboardText = $"First line{Environment.NewLine}Second line";

         // Setup

         var clipboardServiceMock = new Mock<IClipboardService>();
         clipboardServiceMock.Setup( cs => cs.GetText() ).Returns( clipboardText );

         // Test

         var viewModel = new CommitViewModel( null, null, null, clipboardServiceMock.Object, null, null, null );

         viewModel.PasteCommand.Execute( null );

         // Assert

         viewModel.ShortMessage.Should().Be( "First line" );
         viewModel.ExtraCommitText.Should().Be( "Second line" );
      }

      [Fact]
      public void PasteCommand_ClipboardHasOneLineEndingWithLineBreak_SetsShortMessage()
      {
         string clipboardText = $"First line{Environment.NewLine}";

         // Setup

         var clipboardServiceMock = new Mock<IClipboardService>();
         clipboardServiceMock.Setup( cs => cs.GetText() ).Returns( clipboardText );

         // Test

         var viewModel = new CommitViewModel( null, null, null, clipboardServiceMock.Object, null, null, null );

         viewModel.PasteCommand.Execute( null );

         // Assert

         viewModel.ShortMessage.Should().Be( "First line" );
         viewModel.ExtraCommitText.Should().BeNull();
      }

      [Fact]
      public void PasteCommand_ClipboardHasOneLineEndingWithTwoLineBreaks_SetsShortMessage()
      {
         string clipboardText = $"First line{Environment.NewLine}{Environment.NewLine}";

         // Setup

         var clipboardServiceMock = new Mock<IClipboardService>();
         clipboardServiceMock.Setup( cs => cs.GetText() ).Returns( clipboardText );

         // Test

         var viewModel = new CommitViewModel( null, null, null, clipboardServiceMock.Object, null, null, null );

         viewModel.PasteCommand.Execute( null );

         // Assert

         viewModel.ShortMessage.Should().Be( "First line" );
         viewModel.ExtraCommitText.Should().BeNull();
      }

      [Fact]
      public void PasteCommand_ClipboardHasBothMessagesSeparatedByBlankLine_SetsBothMessages()
      {
         string clipboardText = $"Short message{Environment.NewLine}{Environment.NewLine}Secondary notes";

         // Setup

         var clipboardServiceMock = new Mock<IClipboardService>();
         clipboardServiceMock.Setup( cs => cs.GetText() ).Returns( clipboardText );

         // Test

         var viewModel = new CommitViewModel( null, null, null, clipboardServiceMock.Object, null, null, null );

         viewModel.PasteCommand.Execute( null );

         // Assert

         viewModel.ShortMessage.Should().Be( "Short message" );
         viewModel.ExtraCommitText.Should().Be( "Secondary notes" );
      }

      [Fact]
      public void PasteCommand_ClipboardHasBothMessagesEndingWithLineBreaks_TrimsEndLineBreaks()
      {
         string clipboardText = $"Short message{Environment.NewLine}Secondary notes{Environment.NewLine}{Environment.NewLine}";

         // Setup

         var clipboardServiceMock = new Mock<IClipboardService>();
         clipboardServiceMock.Setup( cs => cs.GetText() ).Returns( clipboardText );

         // Test

         var viewModel = new CommitViewModel( null, null, null, clipboardServiceMock.Object, null, null, null );

         viewModel.PasteCommand.Execute( null );

         // Assert

         viewModel.ShortMessage.Should().Be( "Short message" );
         viewModel.ExtraCommitText.Should().Be( "Secondary notes" );
      }

      [Fact]
      public async Task PasteCommand_ClipboardHasBothMessagesAndExtraNotesSpanMultipleLines_SetsBothMessage()
      {
         const string shortMessage = "First line message";
         string extraMessage = $"Secondary notes, first line{Environment.NewLine}{Environment.NewLine}Second line{Environment.NewLine}Third line";
         string clipboardText = $"{shortMessage}{Environment.NewLine}{Environment.NewLine}{extraMessage}";

         // Setup

         await Task.Yield();

         var clipboardServiceMock = new Mock<IClipboardService>();
         clipboardServiceMock.Setup( cs => cs.GetText() ).Returns( clipboardText );

         // Test

         var viewModel = new CommitViewModel( null, null, null, clipboardServiceMock.Object, null, null, null );

         viewModel.PasteCommand.Execute( null );

         // Assert

         viewModel.ShortMessage.Should().Be( shortMessage );
         viewModel.ExtraCommitText.Should().Be( extraMessage );
      }

      [Fact]
      public void Title_HappyPath_TitleContainsBranchName()
      {
         const string branchName = "master";

         // Setup

         var gitServiceMock = new Mock<IGitService>();
         gitServiceMock.Setup( gs => gs.GetCurrentBranchName() ).Returns( branchName );

         // Test

         var viewModel = new CommitViewModel( null, null, null, null, null, gitServiceMock.Object, null );
         string title = viewModel.Title;

         // Assert

         title.Should().Contain( branchName );
      }

      [Fact]
      public void AbortCommand_ExpandedFlagIsSet_RaisesCollapseRequested()
      {
         bool wasRaised = false;

         // Arrange

         var appServiceMock = new Mock<IAppService>();

         // Act

         var viewModel = new CommitViewModel( null, null, appServiceMock.Object, null, new CommitDocument(), null, Mock.Of<ICommitFileWriter>() )
         {
            IsExpanded = true
         };

         viewModel.AsyncCollapseRequested += ( _, __ ) =>
         {
            wasRaised = true;
            return Task.CompletedTask;
         };

         viewModel.AbortCommand.Execute( null );

         // Assert

         wasRaised.Should().BeTrue();
      }

      //[Fact]
      //public void SaveCommand_ShortMessageIsEmpty_RaisesShakeRequestedEvent()
      //{
      //   bool wasRaised = false;

      //   // Arrange

      //   var appServiceMock = new Mock<IAppService>();
      //   var commitDocumentMock = new Mock<ICommitDocument>();
      //   commitDocumentMock.SetupGet( cd => cd.ShortMessage ).Returns( string.Empty );

      //   // Act

      //   var viewModel = new CommitViewModel( null, appServiceMock.Object, null, commitDocumentMock.Object, null )
      //   {
      //      IsExiting = false
      //   };

      //   viewModel.AsyncShakeRequested += ( _, __ ) =>
      //   {
      //      wasRaised = true;
      //      return Task.CompletedTask;
      //   };

      //   viewModel.SaveCommand.Execute( null );

      //   // Assert

      //   wasRaised.Should().BeTrue();
      //}
   }
}
