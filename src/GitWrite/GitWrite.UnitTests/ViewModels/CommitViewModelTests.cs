using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GitWrite.Resources;
using GitWrite.Services;
using GitWrite.UnitTests.Helpers;
using GitWrite.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GitWrite.UnitTests.ViewModels
{
   [TestClass]
   public class CommitViewModelTests
   {
      [TestCleanup]
      public void Cleanup()
      {
         SimpleIoc.Default.Reset();
      }

      [TestMethod]
      public void ViewLoaded_DoesNotHaveExtraNotes_DoesNotRaiseExpansionEvent()
      {
         bool expanded = false;

         var commitViewModel = new CommitViewModel();
         commitViewModel.ExpansionRequested += ( sender, e ) => expanded = true;

         commitViewModel.ViewLoaded();

         Assert.IsFalse( expanded );
      }

      [TestMethod]
      public void ViewLoaded_HasExtraNotes_RaisesExpansionEvent()
      {
         bool expanded = false;

         var commitViewModel = new CommitViewModel
         {
            ExtraCommitText = "Extra notes"
         };

         commitViewModel.ExpansionRequested += ( sender, e ) => expanded = true;
         commitViewModel.ViewLoaded();

         Assert.IsTrue( expanded );
      }

      [TestMethod]
      public void Constructor_CommitDocumentHasShortMessage_ViewModelReadsShortMessage()
      {
         const string shortMessage = "Short commit message";

         // Setup

         var commitDocumentMock = new Mock<ICommitDocument>();
         commitDocumentMock.SetupGet( cd => cd.ShortMessage ).Returns( shortMessage );

         App.CommitDocument = commitDocumentMock.Object;

         // Test

         var commitViewModel = new CommitViewModel();

         Assert.AreEqual( shortMessage, commitViewModel.ShortMessage );
      }

      [TestMethod]
      public void Constructor_HasSingleLineLongMessage_ViewModelReadsTheLongMessage()
      {
         var longMessage = new List<string>
         {
            "Long message here"
         };

         // Setup

         var commitDocumentMock = new Mock<ICommitDocument>();
         commitDocumentMock.SetupGet( cd => cd.LongMessage ).Returns( longMessage );

         App.CommitDocument = commitDocumentMock.Object;

         // Test

         var commitViewModel = new CommitViewModel();

         Assert.AreEqual( longMessage[0], commitViewModel.ExtraCommitText );
      }

      [TestMethod]
      public void KeyDown_PressesF1_RunsHelpCommand()
      {
         bool helpCommandExecuted = false;

         var commitViewModel = new CommitViewModel
         {
            HelpCommand = new RelayCommand( () => helpCommandExecuted = true )
         };

         var args = TestHelper.GetKeyEventArgs( Key.F1 );

         commitViewModel.OnCommitNotesKeyDown( args );

         Assert.IsTrue( helpCommandExecuted );
      }

      [TestMethod]
      public void KeyDown_PressesF1WhileHelpStateIsActive_DoesNotRunHelpCommand()
      {
         bool helpCommandExecuted = false;

         var commitViewModel = new CommitViewModel
         {
            HelpCommand = new RelayCommand( () => helpCommandExecuted = true ),
            IsHelpStateActive = true
         };

         var args = TestHelper.GetKeyEventArgs( Key.F1 );

         commitViewModel.OnCommitNotesKeyDown( args );

         Assert.IsFalse( helpCommandExecuted );
      }

      [TestMethod]
      public void KeyDown_PressesF1WhileHelpStateIsActive_DismissesHelpState()
      {
         bool collapseHelpRequested = false;

         var commitViewModel = new CommitViewModel
         {
            IsHelpStateActive = true
         };

         commitViewModel.CollapseHelpRequested += ( sender, e ) => collapseHelpRequested = true;

         var args = TestHelper.GetKeyEventArgs( Key.F1 );

         commitViewModel.OnCommitNotesKeyDown( args );

         Assert.IsTrue( collapseHelpRequested );
      }

      [TestMethod]
      public void KeyDown_PressesBKeyWhileHelpStateIsActive_DismissesHelpState()
      {
         bool collapseHelpRequested = false;

         var commitViewModel = new CommitViewModel
         {
            IsHelpStateActive = true
         };

         commitViewModel.CollapseHelpRequested += ( sender, e ) => collapseHelpRequested = true;

         var args = TestHelper.GetKeyEventArgs( Key.B );

         commitViewModel.OnCommitNotesKeyDown( args );

         Assert.IsTrue( collapseHelpRequested );
      }

      [TestMethod]
      public void KeyDown_PressesEnter_RunsSaveCommand()
      {
         bool saveCommandExecuted = false;

         var commitViewModel = new CommitViewModel
         {
            SaveCommand = new RelayCommand( () => saveCommandExecuted = true )
         };

         var args = TestHelper.GetKeyEventArgs( Key.Enter );

         commitViewModel.OnCommitNotesKeyDown( args );

         Assert.IsTrue( saveCommandExecuted );
      }

      //[TestMethod]
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

      //[TestMethod]
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

      //[TestMethod]
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

      //[TestMethod]
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

      //[TestMethod]
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

      //[TestMethod]
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

      //[TestMethod]
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

      //[TestMethod]
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

      //[TestMethod]
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

      //[TestMethod]
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
