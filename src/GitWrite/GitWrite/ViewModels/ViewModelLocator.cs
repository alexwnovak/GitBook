using GalaSoft.MvvmLight.Ioc;

namespace GitWrite.ViewModels
{
   public class ViewModelLocator
   {
      public CommitViewModel CommitViewModel => SimpleIoc.Default.GetInstance<CommitViewModel>();

      public InteractiveRebaseViewModel InteractiveRebaseViewMdoel => SimpleIoc.Default.GetInstance<InteractiveRebaseViewModel>();

      public ConfirmationViewModel ConfirmationViewModel => SimpleIoc.Default.GetInstance<ConfirmationViewModel>();
   }
}
