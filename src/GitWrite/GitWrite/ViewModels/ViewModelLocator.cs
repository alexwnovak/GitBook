using GalaSoft.MvvmLight.Ioc;
using GitWrite.Services;

namespace GitWrite.ViewModels
{
   public class ViewModelLocator
   {
      public CommitViewModel CommitViewModel
         => new CommitViewModel( SimpleIoc.Default.GetInstance<IViewService>(),
            new AppService(),
            new ClipboardService(),
            SimpleIoc.Default.GetInstance<ICommitDocument>(),
            SimpleIoc.Default.GetInstance<IGitService>() );

      public InteractiveRebaseViewModel InteractiveRebaseViewMdoel
         => new InteractiveRebaseViewModel( SimpleIoc.Default.GetInstance<IViewService>(),
            new AppService() );
   }
}
