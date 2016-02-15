using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace GitWrite.Views
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
         if ( e.ChangedButton == MouseButton.Left )
         {
            AssociatedObject.DragMove();
         }  
      }
   }
}
