using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GitWrite.ViewModels;

namespace GitWrite.Views.Controls
{
   public partial class TransitionEntryBox : UserControl
   {
      public TransitionEntryBox( ExitReason exitReason )
      {
         InitializeComponent();
         SetColors( exitReason );
      }

      private void SetColors( ExitReason exitReason )
      {
         Brush borderBrush, backgroundBrush;

         if ( exitReason == ExitReason.Save )
         {
            borderBrush = (Brush) Application.Current.Resources["AcceptCommitBorderColor"];
            backgroundBrush = (Brush) Application.Current.Resources["AcceptCommitBackgroundColor"];
         }
         else
         {
            borderBrush = (Brush) Application.Current.Resources["AbortCommitBorderColor"];
            backgroundBrush = (Brush) Application.Current.Resources["AbortCommitBackgroundColor"];
         }

         MainBorder.BorderBrush = borderBrush;
         MainBorder.Background = backgroundBrush;
         CircleBorder.BorderBrush = borderBrush;
         CircleBorder.Background = backgroundBrush;
      }
   }
}
