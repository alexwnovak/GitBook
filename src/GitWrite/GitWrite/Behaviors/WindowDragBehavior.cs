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
         AssociatedObject.MouseDoubleClick += OnMouseDoubleClick;
      }
      protected override void OnDetaching()
      {
         AssociatedObject.MouseDown -= OnMouseDown;
         AssociatedObject.MouseDoubleClick -= OnMouseDoubleClick;
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

      private void OnMouseDoubleClick( object sender, MouseButtonEventArgs e )
      {
         AssociatedObject.Left = ( SystemParameters.FullPrimaryScreenWidth - AssociatedObject.Width ) / 2;
         AssociatedObject.Top = 0.7 * ( SystemParameters.FullPrimaryScreenHeight - 30 ) / 2;
      }
   }
}
