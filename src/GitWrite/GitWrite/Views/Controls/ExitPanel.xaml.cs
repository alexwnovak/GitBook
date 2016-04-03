using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace GitWrite.Views.Controls
{
   public partial class ExitPanel : UserControl
   {
      public static DependencyProperty ExitReasonProperty = DependencyProperty.Register( "ExitReason",
         typeof( ExitReason ),
         typeof( ExitPanel ),
         new FrameworkPropertyMetadata( ExitReason.AbortCommit ) );

      public ExitReason ExitReason
      {
         get
         {
            return (ExitReason) GetValue( ExitReasonProperty );
         }
         set
         {
            SetValue( ExitReasonProperty, value );
         }
      }

      public ExitPanel()
      {
         InitializeComponent();
      }

      public Task ShowAsync()
      {
         var taskCompletionSource = new TaskCompletionSource<bool>();
         var storyboard = (Storyboard) Resources["DisplayPanel"];

         storyboard.Completed += ( sender, e ) => taskCompletionSource.SetResult( true );
         storyboard.Begin();

         return taskCompletionSource.Task;
      }
   }
}
