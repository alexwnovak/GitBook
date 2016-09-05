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
            GlyphTextBlock.Text = (string) Application.Current.Resources["AcceptCommitGlyph"];
         }
         else
         {
            borderBrush = (Brush) Application.Current.Resources["AbortCommitBorderColor"];
            backgroundBrush = (Brush) Application.Current.Resources["AbortCommitBackgroundColor"];
            TextBlock.Text = Resx.DiscardingText;
            GlyphTextBlock.Text = (string) Application.Current.Resources["AbortCommitGlyph"];
         }

         MainBorder.BorderBrush = borderBrush;
         MainBorder.Background = backgroundBrush;
         CircleBorder.BorderBrush = borderBrush;
         CircleBorder.Background = backgroundBrush;
      }
   }
}
