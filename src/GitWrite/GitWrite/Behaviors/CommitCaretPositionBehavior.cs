using System.Windows;
using System.Windows.Interactivity;
using GitWrite.ViewModels;
using GitWrite.Views;

namespace GitWrite.Behaviors
{
   public class CommitCaretPositionBehavior : Behavior<CommitWindow>
   {
      protected override void OnAttached() => AssociatedObject.Loaded += AssociatedObject_OnLoaded;
      protected override void OnDetaching() => AssociatedObject.Loaded += AssociatedObject_OnLoaded;

      private void AssociatedObject_OnLoaded( object sender, RoutedEventArgs e )
      {
         var viewModel = (CommitViewModel) AssociatedObject.DataContext;

         if ( !string.IsNullOrEmpty( viewModel.ShortMessage ) )
         {
            AssociatedObject.CommitText.SelectionStart = AssociatedObject.CommitText.Text.Length;
         }
      }
   }
}
