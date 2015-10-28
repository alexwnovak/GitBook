using Microsoft.Practices.ServiceLocation;

namespace GitWrite.ViewModels
{
   public class ViewModelLocator
   {
      public MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();

      public static void Cleanup()
      {
      }
   }
}