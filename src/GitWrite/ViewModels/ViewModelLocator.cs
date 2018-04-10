using GalaSoft.MvvmLight.Ioc;

namespace GitWrite.ViewModels
{
   public class ViewModelLocator
   {
      public CommitViewModel CommitViewModel => SimpleIoc.Default.GetInstance<CommitViewModel>();
      public RebaseViewModel RebaseViewModel => SimpleIoc.Default.GetInstance<RebaseViewModel>();
      public SettingsViewModel SettingsViewModel => SimpleIoc.Default.GetInstance<SettingsViewModel>();
   }
}
