using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using GalaSoft.MvvmLight.Ioc;
using GitWrite.Services;

namespace GitWrite.Behaviors
{
   public class WindowDragBehavior : Behavior<Window>
   {
      protected override void OnAttached()
      {
         AssociatedObject.MouseDown += OnMouseDown;
      }
      protected override void OnDetaching()
      {
         AssociatedObject.MouseDown -= OnMouseDown;
      }

      private void OnMouseDown( object sender, MouseButtonEventArgs e )
      {
         if ( e.LeftButton == MouseButtonState.Pressed )
         {
            AssociatedObject.DragMove();

            var registryService = SimpleIoc.Default.GetInstance<IRegistryService>();

            registryService.SetWindowX( (int) AssociatedObject.RestoreBounds.Left );
            registryService.SetWindowY( (int) AssociatedObject.RestoreBounds.Top );
         }
      }
   }
}
