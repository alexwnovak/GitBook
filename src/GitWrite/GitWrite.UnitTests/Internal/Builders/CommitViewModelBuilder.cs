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

      public CommitViewModelBuilder WithViewService( IViewService viewService )
      {
         _viewService = viewService;
         return this;
      }

      public CommitViewModelBuilder WithAppService( IAppService appService )
      {
         _appService = appService;
         return this;
      }

      public CommitViewModelBuilder WithClipboardService( IClipboardService clipboardService )
      {
         _clipboardService = clipboardService;
         return this;
      }

      public CommitViewModelBuilder WithCommitDocument( ICommitDocument commitDocument )
      {
         _commitDocument = commitDocument;
         return this;
      }

      public CommitViewModelBuilder WithGitService( IGitService gitService )
      {
         _gitService = gitService;
         return this;
      }

      public CommitViewModel Build()
         => new CommitViewModel( _viewService, _appService, _clipboardService, _commitDocument, _gitService );
   }
}
