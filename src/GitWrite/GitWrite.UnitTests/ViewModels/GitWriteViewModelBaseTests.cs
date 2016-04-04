using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using GitWrite.Services;
using GitWrite.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GitWrite.UnitTests.ViewModels
{
   [TestClass]
   public class GitWriteViewModelBaseTests
   {
      [TestInitialize]
      public void Initialize()
      {
         SimpleIoc.Default.Reset();
      }

      [TestMethod]
      public void AbortCommand_DoesNotNeedConfirmation_ShutsDownTheApp()
      {
         // Setup

         var appServiceMock = new Mock<IAppService>();
         SimpleIoc.Default.Register( () => appServiceMock.Object );

         // Test

         var viewModel = new GitWriteViewModelBase();

         viewModel.AbortCommand.Execute( null );

         // Assert

         appServiceMock.Verify( @as => @as.Shutdown(), Times.Once );
      }

      [TestMethod]
      public void AbortCommand_DoesNotNeedConfirmation_RaisesShutdownEvent()
      {
         bool shutdownRequestedRaised = false;

         // Setup

         var appServiceMock = new Mock<IAppService>();
         SimpleIoc.Default.Register( () => appServiceMock.Object );

         // Test

         var viewModel = new GitWriteViewModelBase();

         viewModel.ShutdownRequested += ( sender, e ) =>
         {
            shutdownRequestedRaised = true;
            return Task.FromResult( true );
         };

         viewModel.AbortCommand.Execute( null );

         // Assert

         Assert.IsTrue( shutdownRequestedRaised );
      }
   }
}
