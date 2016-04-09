using System;
using FluentAssertions;
using Moq;
using Xunit;
using GitWrite.Services;
using GitWrite.ViewModels;

namespace GitWrite.UnitTests.ViewModels
{
   public class CommitViewModelTests
   {
      [Fact]
      public void ViewLoaded_DoesNotHaveExtraNotes_DoesNotRaiseExpansionEvent()
      {
         bool expanded = false;

         var commitViewModel = new CommitViewModel( null, null, null, null, null );
         commitViewModel.ExpansionRequested += ( sender, e ) => expanded = true;

         commitViewModel.ViewLoaded();

         expanded.Should().BeFalse();
      }

      [Fact]
      public void Constructor_CommitDocumentHasShortMessage_ViewModelReadsShortMessage()
      {
         const string shortMessage = "Short commit message";

         // Setup

         var commitDocumentMock = new Mock<ICommitDocument>();
         commitDocumentMock.SetupGet( cd => cd.ShortMessage ).Returns( shortMessage );

         // Test

         var commitViewModel = new CommitViewModel( null, null, null, commitDocumentMock.Object, null );

         commitViewModel.ShortMessage.Should().Be( shortMessage );
      }

      [Fact]
      public void Constructor_HasSingleLineLongMessage_ViewModelReadsTheLongMessage()
      {
         const string longMessage = "Long message here";

         // Setup

         var commitDocumentMock = new Mock<ICommitDocument>();
         commitDocumentMock.SetupGet( cd => cd.LongMessage ).Returns( longMessage );

         // Test

         var commitViewModel = new CommitViewModel( null, null, null, commitDocumentMock.Object, null );

         commitViewModel.ExtraCommitText.Should().Be( longMessage );
      }

      [Fact]
      public void ViewLoaded_HasExtraNotes_RaisesExpansionEvent()
      {
         bool expanded = false;

         var commitViewModel = new CommitViewModel( null, null, null, null, null )
         {
            ExtraCommitText = "Extra notes"
         };

         commitViewModel.ExpansionRequested += ( sender, e ) => expanded = true;
         commitViewModel.ViewLoaded();

         expanded.Should().BeTrue();
      }

      //[Fact]
      //public void KeyDown_PressesF1_RunsHelpCommand()
      //{
      //   bool helpCommandExecuted = false;

      //   var commitViewModel = new CommitViewModel
      //   {
      //      HelpCommand = new RelayCommand( () => helpCommandExecuted = true )
      //   };

      //   var args = TestHelper.GetKeyEventArgs( Key.F1 );

      //   commitViewModel.OnCommitNotesKeyDown( args );

      //   Assert.IsTrue( helpCommandExecuted );
      //}

      //[Fact]
      //public void KeyDown_PressesF1WhileHelpStateIsActive_DoesNotRunHelpCommand()
      //{
      //   bool helpCommandExecuted = false;

      //   var commitViewModel = new CommitViewModel
      //   {
      //      HelpCommand = new RelayCommand( () => helpCommandExecuted = true ),
      //      IsHelpStateActive = true
      //   };

      //   var args = TestHelper.GetKeyEventArgs( Key.F1 );

      //   commitViewModel.OnCommitNotesKeyDown( args );

      //   Assert.IsFalse( helpCommandExecuted );
      //}

      //[Fact]
      //public void KeyDown_PressesF1WhileHelpStateIsActive_DismissesHelpState()
      //{
      //   bool collapseHelpRequested = false;

      //   var commitViewModel = new CommitViewModel
      //   {
      //      IsHelpStateActive = true
      //   };

      //   commitViewModel.CollapseHelpRequested += ( sender, e ) => collapseHelpRequested = true;

      //   var args = TestHelper.GetKeyEventArgs( Key.F1 );

      //   commitViewModel.OnCommitNotesKeyDown( args );

      //   Assert.IsTrue( collapseHelpRequested );
      //}

      [Fact]
      public void HelpCommand_HelpStateNotActive_SetsHelpStateFlag()
      {
         var commitViewModel = new CommitViewModel( null, null, null, null, null )
         {
            IsHelpStateActive = false
         };

         commitViewModel.HelpCommand.Execute( null );

         commitViewModel.IsHelpStateActive.Should().BeTrue();
      }

      [Fact]
      public void HelpCommand_HelpStateNotActive_RaisesHelpRequestedEvent()
      {
         bool raisedEvent = false;

         var commitViewModel = new CommitViewModel( null, null, null, null, null )
         {
            IsHelpStateActive = false
         };

         commitViewModel.HelpRequested += ( sender, args ) => raisedEvent = true;

         commitViewModel.HelpCommand.Execute( null );

         raisedEvent.Should().BeTrue();
      }

      [Fact]
      public void HelpCommand_HelpStateIsActive_DoesNotChangeHelpFlag()
      {
         var commitViewModel = new CommitViewModel( null, null, null, null, null )
         {
            IsHelpStateActive = true
         };

         commitViewModel.HelpCommand.Execute( null );

         commitViewModel.IsHelpStateActive.Should().BeTrue();
      }

      [Fact]
      public void HelpCommand_HelpStateIsActive_DoesNotRaiseHelpRequestedEvent()
      {
         bool raisedEvent = false;

         var commitViewModel = new CommitViewModel( null, null, null, null, null )
         {
            IsHelpStateActive = true
         };

         commitViewModel.HelpRequested += ( sender, args ) => raisedEvent = true;

         commitViewModel.HelpCommand.Execute( null );

         raisedEvent.Should().BeFalse();
      }

      //[Fact]
      //public void KeyDown_PressesBKeyWhileHelpStateIsActive_DismissesHelpState()
      //{
      //   bool collapseHelpRequested = false;

      //   var commitViewModel = new CommitViewModel
      //   {
      //      IsHelpStateActive = true
      //   };

      //   commitViewModel.CollapseHelpRequested += ( sender, e ) => collapseHelpRequested = true;

      //   var args = TestHelper.GetKeyEventArgs( Key.B );

      //   commitViewModel.OnCommitNotesKeyDown( args );

      //   Assert.IsTrue( collapseHelpRequested );
      //}

      //[Fact]
      //public void KeyDown_PressesEnter_RunsSaveCommand()
      //{
      //   bool saveCommandExecuted = false;

      //   var commitViewModel = new CommitViewModel
      //   {
      //      SaveCommand = new RelayCommand( () => saveCommandExecuted = true )
      //   };

      //   var args = TestHelper.GetKeyEventArgs( Key.Enter );

      //   commitViewModel.OnCommitNotesKeyDown( args );

      //   Assert.IsTrue( saveCommandExecuted );
      //}

      //[Fact]
      //public void KeyDown_PressesEscape_RunsAbortCommand()
      //{
      //   bool abortCommandRun = false;

      //   var commitViewModel = new CommitViewModel
      //   {
      //      AbortCommand = new RelayCommand( () => abortCommandRun = true )
      //   };

      //   var args = TestHelper.GetKeyEventArgs( Key.Escape );

      //   commitViewModel.OnCommitNotesKeyDown( args );

      //   Assert.IsTrue( abortCommandRun );
      //}

      //[Fact]
      //public void KeyDown_PressesEscape_MarksEventAsHandled()
      //{
      //   var commitViewModel = new CommitViewModel
      //   {
      //      AbortCommand = new RelayCommand( () =>
      //      {
      //      } )
      //   };

      //   var args = TestHelper.GetKeyEventArgs( Key.Escape );

      //   commitViewModel.OnCommitNotesKeyDown( args );

      //   Assert.IsTrue( args.Handled );
      //}

      [Fact]
      public void ExpandCommand_IsNotExpanded_SetsExpandedFlag()
      {
         var commitViewModel = new CommitViewModel( null, null, null, null, null )
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

         var commitViewModel = new CommitViewModel( null, null, null, null, null )
         {
            IsExpanded = false
         };

         commitViewModel.ExpansionRequested += ( sender, e ) => expansionEventRaised = true;

         commitViewModel.ExpandCommand.Execute( null );

         expansionEventRaised.Should().BeTrue();
      }

      [Fact]
      public void ExpandCommand_IsAlreadyExpanded_DoesNotChangeExpandedFlag()
      {
         var commitViewModel = new CommitViewModel( null, null, null, null, null )
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

         var commitViewModel = new CommitViewModel( null, null, null, null, null )
         {
            IsExpanded = true
         };

         commitViewModel.ExpansionRequested += ( sender, e ) => expansionEventRaised = true;

         commitViewModel.ExpandCommand.Execute( null );

         expansionEventRaised.Should().BeFalse();
      }

      [Fact]
      public void ExpandCommand_IsExitingFlagSetButIsNotExpanded_DoesNotChangeExpandedFlag()
      {
         var commitViewModel = new CommitViewModel( null, null, null, null, null )
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

         var commitViewModel = new CommitViewModel( null, null, null, null, null )
         {
            IsExiting = true,
            IsExpanded = true
         };

         commitViewModel.ExpansionRequested += ( sender, e ) => expansionEventRaised = true;

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

         var viewModel = new CommitViewModel( null, null, clipboardServiceMock.Object, null, null );

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

         var viewModel = new CommitViewModel( null, null, clipboardServiceMock.Object, null, null );

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

         var viewModel = new CommitViewModel( null, null, clipboardServiceMock.Object, null, null );

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

         var viewModel = new CommitViewModel( null, null, clipboardServiceMock.Object, null, null );

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

         var viewModel = new CommitViewModel( null, null, clipboardServiceMock.Object, null, null );

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

         var viewModel = new CommitViewModel( null, null, clipboardServiceMock.Object, null, null );

         viewModel.PasteCommand.Execute( null );

         // Assert

         viewModel.ShortMessage.Should().Be( "Short message" );
         viewModel.ExtraCommitText.Should().Be( "Secondary notes" );
      }

      [Fact]
      public void PasteCommand_ClipboardHasBothMessagesAndExtraNotesSpanMultipleLines_SetsBothMessage()
      {
         const string shortMessage = "First line message";
         string extraMessage = $"Secondary notes, first line{Environment.NewLine}{Environment.NewLine}Second line{Environment.NewLine}Third line";
         string clipboardText = $"{shortMessage}{Environment.NewLine}{Environment.NewLine}{extraMessage}";

         // Setup

         var clipboardServiceMock = new Mock<IClipboardService>();
         clipboardServiceMock.Setup( cs => cs.GetText() ).Returns( clipboardText );

         // Test

         var viewModel = new CommitViewModel( null, null, clipboardServiceMock.Object, null, null );

         viewModel.PasteCommand.Execute( null );

         // Assert

         viewModel.ShortMessage.Should().Be( shortMessage );
         viewModel.ExtraCommitText.Should().Be( extraMessage );
      }

      //[Fact]
      //public void OnCommitNotesKeyDown_KeyIsEscapeAndHasNoCommitText_CallsShutdown()
      //{
      //   // Setup

      //   var serviceMock = new Mock<IAppService>();
      //   SimpleIoc.Default.Register( () => serviceMock.Object );

      //   // Test

      //   var viewModel = new CommitViewModel();

      //   var args = TestHelper.GetKeyEventArgs( Key.Escape );

      //   viewModel.OnCommitNotesKeyDown( args );

      //   // Assert

      //   serviceMock.Verify( sm => sm.Shutdown(), Times.Once() );
      //}

      //[Fact]
      //public void OnCommitNotesKeyDown_KeyIsEscapeAndNotesHaveNotBeenEntered_DoesNotDisplayConfirmDialog()
      //{
      //   // Setup

      //   var serviceMock = new Mock<IAppService>();
      //   SimpleIoc.Default.Register( () => serviceMock.Object );

      //   // Test

      //   var viewModel = new CommitViewModel();

      //   var args = TestHelper.GetKeyEventArgs( Key.Escape );

      //   viewModel.OnCommitNotesKeyDown( args );

      //   // Assert

      //   serviceMock.Verify( sm => sm.DisplayMessageBox( It.IsAny<string>(), It.IsAny<MessageBoxButton>() ), Times.Never() );
      //}

      //[Fact]
      //public void OnCommitNotesKeyDown_KeyIsEscapeAndNotesHaveBeenEntered_DisplaysConfirmDialog()
      //{
      //   // Setup

      //   var serviceMock = new Mock<IAppService>();
      //   SimpleIoc.Default.Register( () => serviceMock.Object );

      //   // Test

      //   var viewModel = new CommitViewModel
      //   {
      //      ShortMessage = "Some notes"
      //   };

      //   var args = TestHelper.GetKeyEventArgs( Key.Escape );

      //   viewModel.OnCommitNotesKeyDown( args );

      //   // Assert

      //   serviceMock.Verify( sm => sm.DisplayMessageBox( It.IsAny<string>(), It.IsAny<MessageBoxButton>() ), Times.Once() );
      //}

      //[Fact]
      //public void OnCommitNotesKeyDown_KeyIsEscapeAndNotesHaveBeenEntered_DisplaysConfirmDialogWithCorrectTextAndButtons()
      //{
      //   // Setup

      //   var serviceMock = new Mock<IAppService>();
      //   SimpleIoc.Default.Register( () => serviceMock.Object );

      //   // Test

      //   var viewModel = new CommitViewModel
      //   {
      //      ShortMessage = "Some notes"
      //   };

      //   var args = TestHelper.GetKeyEventArgs( Key.Escape );

      //   viewModel.OnCommitNotesKeyDown( args );

      //   // Assert

      //   serviceMock.Verify( sm => sm.DisplayMessageBox( Strings.ConfirmDiscardMessage, MessageBoxButton.YesNo ), Times.Once() );
      //}

      //[Fact]
      //public void OnCommitNotesKeyDown_UserDiscardsTheirCommit_ShutsDownAfterConfirmation()
      //{
      //   // Setup

      //   var serviceMock = new Mock<IAppService>();
      //   serviceMock.Setup( sm => sm.DisplayMessageBox( Strings.ConfirmDiscardMessage, MessageBoxButton.YesNo ) ).Returns( MessageBoxResult.Yes );
      //   SimpleIoc.Default.Register( () => serviceMock.Object );

      //   // Test

      //   var viewModel = new CommitViewModel
      //   {
      //      ShortMessage = "Some notes"
      //   };

      //   var args = TestHelper.GetKeyEventArgs( Key.Escape );

      //   viewModel.OnCommitNotesKeyDown( args );

      //   // Assert

      //   serviceMock.Verify( sm => sm.DisplayMessageBox( Strings.ConfirmDiscardMessage, MessageBoxButton.YesNo ), Times.Once() );

      //   serviceMock.Verify( sm => sm.Shutdown(), Times.Once() );
      //}

      //[Fact]
      //public void OnCommitNotesKeyDown_UserDoesNotDiscardTheirCommit_DoesNotShutDownAfterConfirmation()
      //{
      //   // Setup

      //   var serviceMock = new Mock<IAppService>();
      //   serviceMock.Setup( sm => sm.DisplayMessageBox( Strings.ConfirmDiscardMessage, MessageBoxButton.YesNo ) ).Returns( MessageBoxResult.No );
      //   SimpleIoc.Default.Register( () => serviceMock.Object );

      //   // Test

      //   var viewModel = new CommitViewModel
      //   {
      //      ShortMessage = "Some notes"
      //   };

      //   var args = TestHelper.GetKeyEventArgs( Key.Escape );

      //   viewModel.OnCommitNotesKeyDown( args );

      //   // Assert

      //   serviceMock.Verify( sm => sm.DisplayMessageBox( Strings.ConfirmDiscardMessage, MessageBoxButton.YesNo ), Times.Once() );

      //   serviceMock.Verify( sm => sm.Shutdown(), Times.Never() );
      //}

      //[Fact]
      //public void OnCommitNotesKeyDown_EnterKeyPressed_StoresCommitNotesIntoDocument()
      //{
      //   const string commitText = "This commit text.";

      //   // Setup

      //   var serviceMock = new Mock<IAppService>();
      //   SimpleIoc.Default.Register( () => serviceMock.Object );

      //   var commitDocumentMock = new Mock<ICommitDocument>();
      //   commitDocumentMock.SetupProperty( cd => cd.ShortMessage );
      //   commitDocumentMock.SetupGet( cd => cd.LongMessage ).Returns( new List<string>() );
      //   App.CommitDocument = commitDocumentMock.Object;

      //   // Test

      //   var viewModel = new CommitViewModel
      //   {
      //      ShortMessage = commitText
      //   };

      //   var args = TestHelper.GetKeyEventArgs( Key.Enter );

      //   viewModel.OnCommitNotesKeyDown( args );

      //   // Assert

      //   Assert.AreEqual( commitText, App.CommitDocument.ShortMessage );
      //}

      //[Fact]
      //public void OnCommitNotesKeyDown_EnterKeyPressed_StoresExtraCommitNotesIntoDocument()
      //{
      //   string extraCommitText = "This is much longer" + Environment.NewLine + "text for the commit.";

      //   // Setup

      //   var serviceMock = new Mock<IAppService>();
      //   SimpleIoc.Default.Register( () => serviceMock.Object );

      //   var commitDocumentMock = new Mock<ICommitDocument>();
      //   commitDocumentMock.SetupGet( cd => cd.LongMessage ).Returns( new List<string>() );
      //   App.CommitDocument = commitDocumentMock.Object;

      //   // Test

      //   var viewModel = new CommitViewModel
      //   {
      //      ExtraCommitText = extraCommitText
      //   };

      //   var args = TestHelper.GetKeyEventArgs( Key.Enter );

      //   viewModel.OnCommitNotesKeyDown( args );

      //   // Assert

      //   Assert.AreEqual( extraCommitText, App.CommitDocument.LongMessage.First() );
      //}

      //[Fact]
      //public void OnCommitNotesKeyDown_EnterKeyPressed_SavesCommitNotes()
      //{
      //   // Setup

      //   var serviceMock = new Mock<IAppService>();
      //   SimpleIoc.Default.Register( () => serviceMock.Object );

      //   var commitDocumentMock = new Mock<ICommitDocument>();
      //   commitDocumentMock.SetupGet( cd => cd.LongMessage ).Returns( new List<string>() );
      //   App.CommitDocument = commitDocumentMock.Object;

      //   // Test

      //   var viewModel = new CommitViewModel();

      //   var args = TestHelper.GetKeyEventArgs( Key.Enter );

      //   viewModel.OnCommitNotesKeyDown( args );

      //   // Assert

      //   commitDocumentMock.Verify( cd => cd.Save(), Times.Once() );
      //}

      //[Fact]
      //public void OnCommitNotesKeyDown_EnterKeyPressed_ExitsAppWithCodeZero()
      //{
      //   // Setup

      //   var commitDocumentMock = new Mock<ICommitDocument>();
      //   commitDocumentMock.SetupGet( cd => cd.LongMessage ).Returns( new List<string>() );
      //   App.CommitDocument = commitDocumentMock.Object;

      //   var appServiceMock = new Mock<IAppService>();
      //   SimpleIoc.Default.Register( () => appServiceMock.Object );

      //   // Test

      //   var viewModel = new CommitViewModel();

      //   var args = TestHelper.GetKeyEventArgs( Key.Enter );

      //   viewModel.OnCommitNotesKeyDown( args );

      //   // Assert

      //   appServiceMock.Verify( @as => @as.Shutdown(), Times.Once() );
      //}
   }
}
