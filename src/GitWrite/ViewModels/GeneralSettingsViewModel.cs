using GalaSoft.MvvmLight;
using Resx = GitWrite.Properties.Resources;

namespace GitWrite.ViewModels
{
   public class GeneralSettingsViewModel : ViewModelBase, ISettingsSectionViewModel
   {
      public string Header => Resx.GeneralText;
   }
}
