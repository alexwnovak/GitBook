using System.Threading.Tasks;
using FluentAssertions;
using GalaSoft.MvvmLight.Ioc;
using GitWrite.Services;
using GitWrite.ViewModels;
using Moq;
using Xunit;

namespace GitWrite.UnitTests.ViewModels
{
   public class GitWriteViewModelBaseTests
   {
      public GitWriteViewModelBaseTests()
      {
         SimpleIoc.Default.Reset();
      }

      [Fact]
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

      [Fact]
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

         shutdownRequestedRaised.Should().BeTrue();
      }
   }
}
