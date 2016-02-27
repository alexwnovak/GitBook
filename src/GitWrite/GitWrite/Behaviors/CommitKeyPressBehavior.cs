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
            case CommitInputState.Exiting:
               HandleExitingState( e );
               break;
         }
      }

      private void HandleEditingState( KeyEventArgs e )
      {
      }

      private void HandleExitingState( KeyEventArgs e )
      {
      }
   }
}
