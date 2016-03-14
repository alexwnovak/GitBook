using Microsoft.Practices.ServiceLocation;

namespace GitWrite.ViewModels
{
   public class ViewModelLocator
   {
      public CommitViewModel CommitViewModel => ServiceLocator.Current.GetInstance<CommitViewModel>();
      public InteractiveRebaseViewModel InteractiveRebaseViewMdoel => ServiceLocator.Current.GetInstance<InteractiveRebaseViewModel>();

      public static void Cleanup()
      {
      }
   }
}
