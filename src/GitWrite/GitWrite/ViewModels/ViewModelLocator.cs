using GalaSoft.MvvmLight.Ioc;
using GitWrite.Services;

namespace GitWrite.ViewModels
{
   public class ViewModelLocator
   {
      public CommitViewModel CommitViewModel => SimpleIoc.Default.GetInstance<CommitViewModel>();

      public InteractiveRebaseViewModel InteractiveRebaseViewMdoel
         => new InteractiveRebaseViewModel( SimpleIoc.Default.GetInstance<IViewService>(),
            new AppService() );
   }
}
