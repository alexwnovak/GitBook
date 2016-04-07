using System.Threading.Tasks;
using FluentAssertions;
using GitWrite.Services;
using GitWrite.ViewModels;
using Moq;
using Xunit;

namespace GitWrite.UnitTests.ViewModels
{
   public class GitWriteViewModelBaseTests
   {
      [Fact]
      public void AbortCommand_DoesNotNeedConfirmation_ShutsDownTheApp()
      {
         // Setup

         var appServiceMock = new Mock<IAppService>();

         // Test

         var viewModel = new GitWriteViewModelBase( null, appServiceMock.Object );

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

         // Test

         var viewModel = new GitWriteViewModelBase( null, appServiceMock.Object );

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
