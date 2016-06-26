using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Media.Animation;
using GalaSoft.MvvmLight.Ioc;
using GitWrite.Behaviors;
using GitWrite.Services;
using GitWrite.ViewModels;

namespace GitWrite.Views
{
   public class WindowBase : Window, IViewService
   {
      private GitWriteViewModelBase _viewModel;

      public WindowBase()
      {
         SimpleIoc.Default.Register<IViewService>( () => this );

         var behaviors = Interaction.GetBehaviors( this );
         behaviors.Add( new WindowDragBehavior() );
         behaviors.Add( new WindowPlacementBehavior() );
         behaviors.Add( new CommitKeyPressBehavior() );

         Loaded += OnLoaded;
         Closing += OnClosing;
      }

      private void OnLoaded( object sender, EventArgs e )
      {
         _viewModel = (GitWriteViewModelBase) DataContext;
         _viewModel.ShutdownRequested += OnShutdownRequested;
      }

      private void OnClosing( object sender, CancelEventArgs e )
      {
         e.Cancel = true;
         _viewModel.AbortCommand.Execute( null );
      }

      private async Task OnShutdownRequested( object sender, ShutdownEventArgs e )
      {
         await PlayExitAnimationAsync( e.ExitReason );
      }

      private Task PlayExitAnimationAsync( ExitReason exitReason )
      {
         var taskCompletionSource = new TaskCompletionSource<bool>();
         var storyboard = (Storyboard) Resources["ExitStoryboard"];

         storyboard.Completed += async ( sender, e ) =>
         {
            await Task.Delay( 500 );
            taskCompletionSource.SetResult( true );
         };
         storyboard.Begin();

         return taskCompletionSource.Task;
      }

      public ExitReason ConfirmExit()
      {
         var confirmationDialog = new ConfirmationDialog();
         return confirmationDialog.ShowDialog();   
      }
   }
}
