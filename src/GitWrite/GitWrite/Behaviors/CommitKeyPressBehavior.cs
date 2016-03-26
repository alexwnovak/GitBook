using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using GalaSoft.MvvmLight.Ioc;
using GitWrite.Services;
using GitWrite.Themes;
using GitWrite.ViewModels;
using GitWrite.Views;

namespace GitWrite.Behaviors
{
   public class CommitKeyPressBehavior : Behavior<Window>
   {
      private readonly CommitViewModel _commitViewModel = SimpleIoc.Default.GetInstance<CommitViewModel>();

      protected override void OnAttached() => AssociatedObject.Loaded += OnLoaded;
      protected override void OnDetaching() => AssociatedObject.Unloaded -= OnUnloaded;

      private void OnLoaded( object sender, RoutedEventArgs e )
      {
         AssociatedObject.KeyDown += KeyDown;
         AssociatedObject.PreviewKeyDown += PreviewKeyDown;
      }

      private void OnUnloaded( object sender, RoutedEventArgs e )
      {
         AssociatedObject.KeyDown -= KeyDown;
         AssociatedObject.PreviewKeyDown -= PreviewKeyDown;
      }
      
      public void KeyDown( object sender, KeyEventArgs e )
      {
         switch ( _commitViewModel.InputState )
         {
            case CommitInputState.Editing:
               HandleEditingState( e );
               break;
         }
      }

      private void PreviewKeyDown( object sender, KeyEventArgs e )
      {
         if ( e.Key == Key.V && Keyboard.Modifiers == ModifierKeys.Control )
         {
            _commitViewModel.PasteCommand.Execute( null );
         }
         else if ( e.Key == Key.T && Keyboard.Modifiers == ModifierKeys.Control )
         {
            ThemeSwitcher.SwitchToNext();

            var appSettings = SimpleIoc.Default.GetInstance<IApplicationSettings>();
            appSettings.Theme = ThemeSwitcher.ThemeName;
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
