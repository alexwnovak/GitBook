using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace GitWrite.Views
{
   public static class FrameworkElementExtensions
   {
      public static void PlayStoryboard( this FrameworkElement frameworkElement, string storyboardName )
      {
         var storyboard = (Storyboard) frameworkElement.Resources[storyboardName];
         storyboard.Begin();
      }

      public static Task PlayStoryboardAsync( this FrameworkElement frameworkElement, string storyboardName )
      {
         var storyboard = (Storyboard) frameworkElement.Resources[storyboardName];

         var tcs = new TaskCompletionSource<bool>();
         storyboard.Completed += ( _, __ ) => tcs.SetResult( true );

         storyboard.Begin();
         return tcs.Task;
      }
   }
}
