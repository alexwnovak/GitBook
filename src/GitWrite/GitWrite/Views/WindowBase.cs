using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;
using GalaSoft.MvvmLight.Ioc;
using GitWrite.Behaviors;
using GitWrite.Services;
using GitWrite.ViewModels;
using GitWrite.Views.Controls;
using Resx = GitWrite.Properties.Resources;

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
         behaviors.Add( new SuppressAltBehavior() );

         Loaded += OnLoaded;
         Closing += OnClosing;
      }

      private async void OnLoaded( object sender, EventArgs e )
      {
         _viewModel = (GitWriteViewModelBase) DataContext;
          
         await Task.Delay( 200 );

         string saveText = GetSaveText();

         var materialGenerator = new MaterialGenerator( this );
         await materialGenerator.GenerateAsync( saveText );
      }

      private void OnClosing( object sender, CancelEventArgs e )
      {
         e.Cancel = true;
         _viewModel.AbortCommand.Execute( null );
      }

      public ExitReason ConfirmExit()
      {
         string title = Resx.ExitConfirmationHeaderText;
         string message = Resx.ExitConfirmationBodyText;

         var styledDialog = new StyledDialog();
         var dialogResult = styledDialog.ShowDialog( title, message, DialogButtons.SaveDiscardCancel );

         switch ( dialogResult )
         {
            case Controls.DialogResult.Save:
               return ExitReason.Save;
            case Controls.DialogResult.Discard:
               return ExitReason.Discard;
            default:
               return ExitReason.Cancel;
         }
      }

      private string GetSaveText()
      {
         var commitViewModel = _viewModel as CommitViewModel;

         if ( commitViewModel != null )
         {
            return commitViewModel.IsAmending ? Resx.AmendingText : Resx.CommittingText;
         }

         return null;
      }
   }
}
