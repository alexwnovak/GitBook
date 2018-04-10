using System.Windows;
using System.Windows.Controls;
using GitWrite.ViewModels;

namespace GitWrite.Views
{
   public class SettingsDataTemplateSelector : DataTemplateSelector
   {
      public override DataTemplate SelectTemplate( object item, DependencyObject container )
      {
         var frameworkElement = (FrameworkElement) container;

         if ( item is GeneralSettingsViewModel )
         {
            return frameworkElement.FindResource( "GeneralSettingsDataTemplate" ) as DataTemplate;
         }
         if ( item is CommitSettingsViewModel )
         {
            return frameworkElement.FindResource( "CommitSettingsDataTemplate" ) as DataTemplate;
         }
         return base.SelectTemplate( item, container );
      }
   }
}
