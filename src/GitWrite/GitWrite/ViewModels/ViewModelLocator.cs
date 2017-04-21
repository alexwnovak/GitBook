using GalaSoft.MvvmLight.Ioc;

namespace GitWrite.ViewModels
{
   public class ViewModelLocator
   {
      public CommitViewModel CommitViewModel => SimpleIoc.Default.GetInstance<CommitViewModel>();

      public InteractiveRebaseViewModel InteractiveRebaseViewModel => SimpleIoc.Default.GetInstance<InteractiveRebaseViewModel>();
   }
}
