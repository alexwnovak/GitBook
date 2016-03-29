using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace GitWrite.Views.Controls
{
   public partial class ConfirmPanel : UserControl
   {
      private TaskCompletionSource<ConfirmationResult> _confirmationCompletionSource;
      private bool _isDismissing;

      public ConfirmPanel()
      {
         InitializeComponent();
      }

      public Task<ConfirmationResult> ShowAsync()
      {
         this.PlayStoryboard( "ShowPanelStoryboard" );

         _confirmationCompletionSource = new TaskCompletionSource<ConfirmationResult>();

         return _confirmationCompletionSource.Task;
      }

      private void Complete( ConfirmationResult confirmationResult )
      {
         if ( _isDismissing )
         {
            return;
         }

         _isDismissing = true;

         if ( _confirmationCompletionSource != null && !_confirmationCompletionSource.Task.IsCompleted )
         {
            if ( confirmationResult == ConfirmationResult.Cancel )
            {
               var storyboard = (Storyboard) Resources["DismissPanelStoryboard"];

               storyboard.Completed += ( sender, e ) => _confirmationCompletionSource.SetResult( confirmationResult );
               storyboard.Begin();
            }
            else
            {
               this.PlayStoryboard( "DismissPanelStoryboard" );
               _confirmationCompletionSource.SetResult( confirmationResult );
            }
         }
      }

      private void ConfirmPanel_OnPreviewKeyDown( object sender, KeyEventArgs e )
      {
         if ( _isDismissing )
         {
            return;
         }

         switch ( e.Key )
         {
            case Key.S:
               SaveText.Foreground = (SolidColorBrush) Resources["SaveTextBrush"];
               Complete( ConfirmationResult.Save );
               break;
            case Key.D:
               DiscardText.Foreground = (SolidColorBrush) Resources["DiscardTextBrush"];
               Complete( ConfirmationResult.Discard );
               break;
            case Key.C:
               CancelText.Foreground = (SolidColorBrush) Resources["CancelTextBrush"];
               Complete( ConfirmationResult.Cancel );
               break;
            default:
               e.Handled = true;
               return;
         }
      }
   }
}
