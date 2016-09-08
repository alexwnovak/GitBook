using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;
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

      private async void OnLoaded( object sender, EventArgs e )
      {
         _viewModel = (GitWriteViewModelBase) DataContext;

         await Task.Delay( 200 );

         var materialGenerator = new MaterialGenerator( this );
         await materialGenerator.GenerateAsync();
      }

      private void OnClosing( object sender, CancelEventArgs e )
      {
         e.Cancel = true;
         _viewModel.AbortCommand.Execute( null );
      }

      public ExitReason ConfirmExit()
      {
         var confirmationDialog = new ConfirmationDialog();
         return confirmationDialog.ShowDialog();   
      }
   }
}
