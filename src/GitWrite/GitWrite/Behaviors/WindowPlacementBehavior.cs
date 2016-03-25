using System.Windows;
using System.Windows.Interactivity;
using GalaSoft.MvvmLight.Ioc;
using GitWrite.Services;

namespace GitWrite.Behaviors
{
   public class WindowPlacementBehavior : Behavior<Window>
   {
      protected override void OnAttached() => AssociatedObject.Loaded += AssociatedObject_OnLoaded;
      protected override void OnDetaching() => AssociatedObject.Loaded -= AssociatedObject_OnLoaded;

      private void AssociatedObject_OnLoaded( object sender, RoutedEventArgs e )
      {
         var registryService = SimpleIoc.Default.GetInstance<IRegistryService>();

         AssociatedObject.WindowStartupLocation = WindowStartupLocation.Manual;
         AssociatedObject.Left = registryService.GetWindowX();
         AssociatedObject.Top = registryService.GetWindowY();
      }
   }
}
