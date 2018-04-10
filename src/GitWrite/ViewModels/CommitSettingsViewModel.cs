using GalaSoft.MvvmLight;
using GitWrite.Properties;

namespace GitWrite.ViewModels
{
   public class CommitSettingsViewModel : ViewModelBase, ISettingsSectionViewModel
   {
      public string Header => Resources.CommitText;
   }
}
