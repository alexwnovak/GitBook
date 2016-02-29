using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace GitWrite.Views.Controls
{
   public partial class ConfirmPanel : UserControl
   {
      private TaskCompletionSource<ConfirmationResult> _confirmationCompletionSource;
       
      public ConfirmPanel()
      {
         InitializeComponent();
      }

      public Task<ConfirmationResult> ShowAsync()
      {
         ( Resources["ShowPanelStoryboard"] as Storyboard )?.Begin();

         _confirmationCompletionSource = new TaskCompletionSource<ConfirmationResult>();

         return _confirmationCompletionSource.Task;
      }

      private void Complete( ConfirmationResult confirmationResult )
      {
         if ( _confirmationCompletionSource != null && !_confirmationCompletionSource.Task.IsCompleted )
         {
            var storyboard = (Storyboard) Resources["DismissPanelStoryboard"];

            storyboard.Completed += ( sender, e ) => _confirmationCompletionSource.SetResult( confirmationResult );
            storyboard.Begin();
         }
      }

      private void ConfirmPanel_OnPreviewKeyDown( object sender, KeyEventArgs e )
      {
         switch ( e.Key )
         {
            case Key.S:
               Complete( ConfirmationResult.Save );
               break;
            case Key.D:
               Complete( ConfirmationResult.Discard );
               break;
            case Key.C:
               Complete( ConfirmationResult.Cancel );
               break;
            default:
               return;
         }

         e.Handled = true;
      }
   }
}
