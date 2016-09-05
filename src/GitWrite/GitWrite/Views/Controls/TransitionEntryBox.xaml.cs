using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GitWrite.ViewModels;
using Resx = GitWrite.Properties.Resources;

namespace GitWrite.Views.Controls
{
   public partial class TransitionEntryBox : UserControl
   {
      public TransitionEntryBox( ExitReason exitReason )
      {
         InitializeComponent();
         SetupUI( exitReason );
      }

      private void SetupUI( ExitReason exitReason )
      {
         Brush borderBrush, backgroundBrush;

         if ( exitReason == ExitReason.Save )
         {
            borderBrush = (Brush) Application.Current.Resources["AcceptCommitBorderColor"];
            backgroundBrush = (Brush) Application.Current.Resources["AcceptCommitBackgroundColor"];
            TextBlock.Text = Resx.CommitingText;
         }
         else
         {
            borderBrush = (Brush) Application.Current.Resources["AbortCommitBorderColor"];
            backgroundBrush = (Brush) Application.Current.Resources["AbortCommitBackgroundColor"];
            TextBlock.Text = Resx.DiscardingText;
         }

         MainBorder.BorderBrush = borderBrush;
         MainBorder.Background = backgroundBrush;
         CircleBorder.BorderBrush = borderBrush;
         CircleBorder.Background = backgroundBrush;
      }
   }
}
