using Microsoft.Practices.ServiceLocation;

namespace GitWrite.ViewModels
{
   public class ViewModelLocator
   {
      public CommitViewModel CommitViewModel => ServiceLocator.Current.GetInstance<CommitViewModel>();

      public static void Cleanup()
      {
      }
   }
}