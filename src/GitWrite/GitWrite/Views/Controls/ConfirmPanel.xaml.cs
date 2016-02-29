using System.Threading.Tasks;
using System.Windows.Controls;

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
         _confirmationCompletionSource.SetResult( ConfirmationResult.Discard );

         return _confirmationCompletionSource.Task;
      }
   }
}
