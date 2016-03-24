using System.Windows;
using GitWrite.Views.Dwm;

namespace GitWrite.Views
{
   public class WindowBase : Window
   {
      public WindowBase()
      {
         Loaded += ( sender, e ) =>
         {
            WindowCompositionManager.EnableWindowBlur( this );
         };
      }
   }
}
