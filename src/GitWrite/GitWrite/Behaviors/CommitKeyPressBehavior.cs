using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace GitWrite.Behaviors
{
   public class CommitKeyPressBehavior : Behavior<Window>
   {
      protected override void OnAttached() => AssociatedObject.PreviewKeyDown += AssociatedObject_PreviewKeyDown;
      protected override void OnDetaching() => AssociatedObject.PreviewKeyDown -= AssociatedObject_PreviewKeyDown;
      
      private void AssociatedObject_PreviewKeyDown( object sender, KeyEventArgs e )
      {
         
      }
   }
}
