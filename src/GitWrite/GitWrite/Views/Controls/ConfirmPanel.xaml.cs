using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

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
         _confirmationCompletionSource = new TaskCompletionSource<ConfirmationResult>();

         return _confirmationCompletionSource.Task;
      }

      private void Complete( ConfirmationResult confirmationResult )
      {
         if ( _confirmationCompletionSource != null && !_confirmationCompletionSource.Task.IsCompleted )
         {
            _confirmationCompletionSource.SetResult( confirmationResult );
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
            case Key.Escape:
               Complete( ConfirmationResult.Cancel );
               break;
         }
      }
   }
}
