using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
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
      public void OnCommitNotesKeyDown_KeyIsEscapeAndHasNoCommitText_CallsShutdown()
      {
         // Setup

         var serviceMock = new Mock<IAppService>();
         SimpleIoc.Default.Register( () => serviceMock.Object );

         // Test

         var viewModel = new CommitViewModel();

         var args = TestHelper.GetKeyEventArgs( Key.Escape );

         viewModel.OnCommitNotesKeyDown( args );

         // Assert

         serviceMock.Verify( sm => sm.Shutdown(), Times.Once() );
      }

      [TestMethod]
      public void OnCommitNotesKeyDown_KeyIsEscapeAndNotesHaveBeenEntered_DisplaysConfirmDialog()
      {
         // Setup

         var serviceMock = new Mock<IAppService>();
         SimpleIoc.Default.Register( () => serviceMock.Object );

         // Test

         var viewModel = new CommitViewModel
         {
            ShortMessage = "Some notes"
         };

         var args = TestHelper.GetKeyEventArgs( Key.Escape );

         viewModel.OnCommitNotesKeyDown( args );

         // Assert

         serviceMock.Verify( sm => sm.DisplayMessageBox( It.IsAny<string>(), It.IsAny<MessageBoxButton>() ), Times.Once() );
      }

      [TestMethod]
      public void OnCommitNotesKeyDown_KeyIsEscapeAndNotesHaveBeenEntered_DisplaysConfirmDialogWithCorrectTextAndButtons()
      {
         // Setup

         var serviceMock = new Mock<IAppService>();
         SimpleIoc.Default.Register( () => serviceMock.Object );

         // Test

         var viewModel = new CommitViewModel
         {
            ShortMessage = "Some notes"
         };

         var args = TestHelper.GetKeyEventArgs( Key.Escape );

         viewModel.OnCommitNotesKeyDown( args );

         // Assert

         serviceMock.Verify( sm => sm.DisplayMessageBox( Strings.ConfirmDiscardMessage, MessageBoxButton.YesNo ), Times.Once() );
      }

      [TestMethod]
      public void OnCommitNotesKeyDown_UserDiscardsTheirCommit_ShutsDownAfterConfirmation()
      {
         // Setup

         var serviceMock = new Mock<IAppService>();
         serviceMock.Setup( sm => sm.DisplayMessageBox( Strings.ConfirmDiscardMessage, MessageBoxButton.YesNo ) ).Returns( MessageBoxResult.Yes );    
         SimpleIoc.Default.Register( () => serviceMock.Object );

         // Test

         var viewModel = new CommitViewModel
         {
            ShortMessage = "Some notes"
         };

         var args = TestHelper.GetKeyEventArgs( Key.Escape );

         viewModel.OnCommitNotesKeyDown( args );

         // Assert

         serviceMock.Verify( sm => sm.DisplayMessageBox( Strings.ConfirmDiscardMessage, MessageBoxButton.YesNo ), Times.Once() );

         serviceMock.Verify( sm => sm.Shutdown(), Times.Once() );
      }

      [TestMethod]
      public void OnCommitNotesKeyDown_UserDoesNotDiscardTheirCommit_DoesNotShutDownAfterConfirmation()
      {
         // Setup

         var serviceMock = new Mock<IAppService>();
         serviceMock.Setup( sm => sm.DisplayMessageBox( Strings.ConfirmDiscardMessage, MessageBoxButton.YesNo ) ).Returns( MessageBoxResult.No );
         SimpleIoc.Default.Register( () => serviceMock.Object );

         // Test

         var viewModel = new CommitViewModel
         {
            ShortMessage = "Some notes"
         };

         var args = TestHelper.GetKeyEventArgs( Key.Escape );

         viewModel.OnCommitNotesKeyDown( args );

         // Assert

         serviceMock.Verify( sm => sm.DisplayMessageBox( Strings.ConfirmDiscardMessage, MessageBoxButton.YesNo ), Times.Once() );

         serviceMock.Verify( sm => sm.Shutdown(), Times.Never() );
      }

      [TestMethod]
      public void OnCommitNotesKeyDown_EnterKeyPressed_StoresCommitNotesIntoDocument()
      {
         const string commitText = "This commit text.";

         // Setup

         var serviceMock = new Mock<IAppService>();
         SimpleIoc.Default.Register( () => serviceMock.Object );

         var commitDocumentMock = new Mock<ICommitDocument>();
         commitDocumentMock.SetupProperty( cd => cd.ShortMessage );
         commitDocumentMock.SetupGet( cd => cd.LongMessage ).Returns( new List<string>() );
         App.CommitDocument = commitDocumentMock.Object;

         // Test

         var viewModel = new CommitViewModel
         {
            ShortMessage = commitText
         };

         var args = TestHelper.GetKeyEventArgs( Key.Enter );

         viewModel.OnCommitNotesKeyDown( args );

         // Assert

         Assert.AreEqual( commitText, App.CommitDocument.ShortMessage );
      }

      [TestMethod]
      public void OnCommitNotesKeyDown_EnterKeyPressed_StoresExtraCommitNotesIntoDocument()
      {
         string extraCommitText = "This is much longer" + Environment.NewLine + "text for the commit.";

         // Setup

         var serviceMock = new Mock<IAppService>();
         SimpleIoc.Default.Register( () => serviceMock.Object );

         var commitDocumentMock = new Mock<ICommitDocument>();
         commitDocumentMock.SetupGet( cd => cd.LongMessage ).Returns( new List<string>() );
         App.CommitDocument = commitDocumentMock.Object;

         // Test

         var viewModel = new CommitViewModel
         {
            ExtraCommitText = extraCommitText
         };

         var args = TestHelper.GetKeyEventArgs( Key.Enter );

         viewModel.OnCommitNotesKeyDown( args );

         // Assert

         Assert.AreEqual( extraCommitText, App.CommitDocument.LongMessage.First() );
      }

      [TestMethod]
      public void OnCommitNotesKeyDown_EnterKeyPressed_SavesCommitNotes()
      {
         // Setup

         var serviceMock = new Mock<IAppService>();
         SimpleIoc.Default.Register( () => serviceMock.Object );

         var commitDocumentMock = new Mock<ICommitDocument>();
         commitDocumentMock.SetupGet( cd => cd.LongMessage ).Returns( new List<string>() );
         App.CommitDocument = commitDocumentMock.Object;

         // Test

         var viewModel = new CommitViewModel();

         var args = TestHelper.GetKeyEventArgs( Key.Enter );

         viewModel.OnCommitNotesKeyDown( args );

         // Assert

         commitDocumentMock.Verify( cd => cd.Save(), Times.Once() );
      }

      [TestMethod]
      public void OnCommitNotesKeyDown_EnterKeyPressed_ExitsAppWithCodeZero()
      {
         // Setup

         var commitDocumentMock = new Mock<ICommitDocument>();
         commitDocumentMock.SetupGet( cd => cd.LongMessage ).Returns( new List<string>() );
         App.CommitDocument = commitDocumentMock.Object;

         var appServiceMock = new Mock<IAppService>();
         SimpleIoc.Default.Register( () => appServiceMock.Object );

         // Test

         var viewModel = new CommitViewModel();

         var args = TestHelper.GetKeyEventArgs( Key.Enter );

         viewModel.OnCommitNotesKeyDown( args );

         // Assert

         appServiceMock.Verify( @as => @as.Shutdown(), Times.Once() );
      }
   }
}
