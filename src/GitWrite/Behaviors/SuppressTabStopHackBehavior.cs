using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Xaml.Behaviors;

namespace GitWrite.Behaviors
{
   public class SuppressTabStopHackBehavior : Behavior<FrameworkElement>
   {
      protected override void OnAttached()
      {
         AssociatedObject.Loaded += OnLoaded;
      }

      private void OnLoaded( object sender, RoutedEventArgs e )
      {
         AssociatedObject.Loaded -= OnLoaded;

         // Using the AcrylicWindow seems to have a style that forces a focusable
         // ContentControl somewhere in the visual tree above our root layout element
         // in our window. We don't want that here, and want to tightly control what
         // elements we can tab to. So we hack our way up the visual tree to find this
         // ContentControl and disable its IsTabStop property to prevent that

         var parent = (ContentPresenter) VisualTreeHelper.GetParent( (DependencyObject) sender );
         var parent2 = (ContentControl) VisualTreeHelper.GetParent( parent );
         parent2.IsTabStop = false;
      }
   }
}
