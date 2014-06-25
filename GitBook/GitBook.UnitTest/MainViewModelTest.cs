using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Ioc;
using GitBook.Resources;
using GitBook.Service;
using GitBook.UnitTest.Helpers;
using GitBook.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GitBook.UnitTest
{
   [TestClass]
   public class MainViewModelTest
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

         var viewModel = new MainViewModel();

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

         var viewModel = new MainViewModel
         {
            CommitText = "Some notes"
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

         var viewModel = new MainViewModel
         {
            CommitText = "Some notes"
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

         var viewModel = new MainViewModel
         {
            CommitText = "Some notes"
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

         var viewModel = new MainViewModel
         {
            CommitText = "Some notes"
         };

         var args = TestHelper.GetKeyEventArgs( Key.Escape );

         viewModel.OnCommitNotesKeyDown( args );

         // Assert

         serviceMock.Verify( sm => sm.DisplayMessageBox( Strings.ConfirmDiscardMessage, MessageBoxButton.YesNo ), Times.Once() );

         serviceMock.Verify( sm => sm.Shutdown(), Times.Never() );
      }
   }
}
