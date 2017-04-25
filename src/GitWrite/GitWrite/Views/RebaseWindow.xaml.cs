using System.Windows;
using System.Windows.Input;

namespace GitWrite.Views
{
   public partial class RebaseWindow : WindowBase
   {
      public RebaseWindow()
      {
         InitializeComponent();

         CommandBindings.Add( new CommandBinding( SystemCommands.MinimizeWindowCommand, OnMinimizeWindow, OnCanMinimizeWindow ) );
         CommandBindings.Add( new CommandBinding( SystemCommands.MaximizeWindowCommand, OnMaximizeWindow, OnCanResizeWindow ) );
         CommandBindings.Add( new CommandBinding( SystemCommands.RestoreWindowCommand, OnRestoreWindow, OnCanResizeWindow ) );
         CommandBindings.Add( new CommandBinding( SystemCommands.CloseWindowCommand, OnCloseWindow ) );
      }

      private void OnCanMinimizeWindow( object sender, CanExecuteRoutedEventArgs e )
         => e.CanExecute = ResizeMode != ResizeMode.NoResize;

      private void OnMinimizeWindow( object target, ExecutedRoutedEventArgs e )
         => SystemCommands.MinimizeWindow( this );

      private void OnCanResizeWindow( object sender, CanExecuteRoutedEventArgs e )
         => e.CanExecute = ResizeMode == ResizeMode.CanResize || ResizeMode == ResizeMode.CanResizeWithGrip;

      private void OnMaximizeWindow( object target, ExecutedRoutedEventArgs e )
         => SystemCommands.MaximizeWindow( this );

      private void OnRestoreWindow( object target, ExecutedRoutedEventArgs e )
         => SystemCommands.RestoreWindow( this );

      private void OnCloseWindow( object target, ExecutedRoutedEventArgs e )
         => SystemCommands.CloseWindow( this );
   }
}
