using System.Windows;
using System.Windows.Media.Animation;

namespace GitWrite.Views
{
   public static class WindowExtensions
   {
      public static void PlayStoryboard( this Window window, string storyboardName )
      {
         var storyboard = (Storyboard) window.Resources["SubjectHint"];
         storyboard.Begin();
      }
   }
}
