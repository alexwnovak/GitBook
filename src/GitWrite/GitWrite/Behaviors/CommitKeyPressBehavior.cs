using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using GalaSoft.MvvmLight.Ioc;
using GitWrite.ViewModels;
using GitWrite.Views;

namespace GitWrite.Behaviors
{
   public class CommitKeyPressBehavior : Behavior<Window>
   {
      private readonly CommitViewModel _commitViewModel = SimpleIoc.Default.GetInstance<CommitViewModel>();

      protected override void OnAttached() => AssociatedObject.PreviewKeyDown += PreviewKeyDown;
      protected override void OnDetaching() => AssociatedObject.PreviewKeyDown -= PreviewKeyDown;
      
      public void PreviewKeyDown( object sender, KeyEventArgs e )
      {
         switch ( _commitViewModel.InputState )
         {
            case CommitInputState.Editing:
               HandleEditingState( e );
               break;
         }
      }

      private void HandleEditingState( KeyEventArgs e )
      {
         if ( _commitViewModel.DismissHelpIfActive() )
         {
            return;
         }

         if ( e.Key == Key.F1 )
         {
            _commitViewModel.HelpCommand.Execute( null );
         }
         else if ( e.Key == Key.Enter )
         {
            _commitViewModel.SaveCommand.Execute( null );
         }
         else if ( e.Key == Key.Escape
            || ( e.Key == Key.W && ( Keyboard.Modifiers & ModifierKeys.Control ) == ModifierKeys.Control )
            || ( e.Key == Key.F4 && ( Keyboard.Modifiers & ModifierKeys.Control ) == ModifierKeys.Control ) )
         {
            e.Handled = true;
            _commitViewModel.AbortCommand.Execute( null );
         }
      }
   }
}
