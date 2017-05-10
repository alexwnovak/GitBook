using Moq;
using GitWrite.Services;
using GitWrite.ViewModels;

namespace GitWrite.UnitTests.Internal.Builders
{
   internal class CommitViewModelBuilder
   {
      private IViewService _viewService = Mock.Of<IViewService>();
      private IAppService _appService = Mock.Of<IAppService>();
      private IClipboardService _clipboardService = Mock.Of<IClipboardService>();
      private ICommitDocument _commitDocument = Mock.Of<ICommitDocument>();
      private IGitService _gitService = Mock.Of<IGitService>();

      public CommitViewModel Build()
         => new CommitViewModel( _viewService, _appService, _clipboardService, _commitDocument, _gitService );
   }
}
