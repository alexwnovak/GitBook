using GalaSoft.MvvmLight.Ioc;
using GitWrite.Services;
using Microsoft.Practices.ServiceLocation;

namespace GitWrite.ViewModels
{
   public class ViewModelLocator
   {
      public CommitViewModel CommitViewModel
         => new CommitViewModel( SimpleIoc.Default.GetInstance<IViewService>(), new AppService(), new ClipboardService(), SimpleIoc.Default.GetInstance<ICommitDocument>() );

      public InteractiveRebaseViewModel InteractiveRebaseViewMdoel => ServiceLocator.Current.GetInstance<InteractiveRebaseViewModel>();

      public static void Cleanup()
      {
      }
   }
}
