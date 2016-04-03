using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using GalaSoft.MvvmLight.Ioc;
using GitWrite.Behaviors;
using GitWrite.Services;
using GitWrite.ViewModels;
using GitWrite.Views.Controls;
using GitWrite.Views.Dwm;

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

         WindowCompositionManager.EnableWindowBlur( this );
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

      private async Task PlayExitAnimationAsync( ExitReason exitReason )
      {
         var exitPanel = new ExitPanel
         {
            ExitReason = exitReason,
            VerticalAlignment = VerticalAlignment.Stretch
         };

         var layoutRoot = (Panel) Content;
         layoutRoot.Children.Add( exitPanel );

         await exitPanel.ShowAsync();
      }

      public async Task<ConfirmationResult> ConfirmExitAsync()
      {
         var confirmPanel = new ConfirmPanel
         {
            VerticalAlignment = VerticalAlignment.Stretch
         };

         var layoutRoot = (Panel) Content;
         layoutRoot.Children.Add( confirmPanel );

         var previousFocusElement = Keyboard.FocusedElement;
         confirmPanel.Focus();

         var confirmationResult = await confirmPanel.ShowAsync();

         if ( confirmationResult == ConfirmationResult.Cancel )
         {
            layoutRoot.Children.Remove( confirmPanel );
            previousFocusElement.Focus();
         }

         return confirmationResult;
      }
   }
}
