using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Media.Imaging;
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

         SetIcon();
      }

      private void SetIcon()
      {
         Uri iconUri = new Uri( "pack://application:,,,/AppIcon.ico", UriKind.RelativeOrAbsolute );
         Icon = BitmapFrame.Create( iconUri );
      }

      private void OnLoaded( object sender, EventArgs e )
      {
         _viewModel = (GitWriteViewModelBase) DataContext;
      }

      private void OnClosing( object sender, CancelEventArgs e )
      {
         e.Cancel = true;
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
   }
}
