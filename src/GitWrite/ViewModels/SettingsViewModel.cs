using System.Collections.Generic;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace GitWrite.ViewModels
{
   public class SettingsViewModel : ViewModelBase
   {
      public ObservableCollection<ISettingsSectionViewModel> Sections { get; }

      public SettingsViewModel( IEnumerable<ISettingsSectionViewModel> settingsSections )
      {
         Sections = new ObservableCollection<ISettingsSectionViewModel>( settingsSections );
      }
   }
}
