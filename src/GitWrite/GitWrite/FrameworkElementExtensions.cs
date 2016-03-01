using System.Windows;
using System.Windows.Media.Animation;

namespace GitWrite
{
   public static class FrameworkElementExtensions
   {
      public static void PlayStoryboard( this FrameworkElement frameworkElement, string storyboardName )
         => ( frameworkElement.Resources[storyboardName] as Storyboard )?.Begin();
   }
}
