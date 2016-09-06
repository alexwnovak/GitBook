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
            TextBlock.Foreground = (Brush) Application.Current.Resources["AcceptCommitForegroundColor"];
            GlyphTextBlock.Text = (string) Application.Current.Resources["AcceptCommitGlyph"];
            GlyphTextBlock.Foreground = (Brush) Application.Current.Resources["AcceptCommitForegroundColor"];
         }
         else
         {
            borderBrush = (Brush) Application.Current.Resources["AbortCommitBorderColor"];
            backgroundBrush = (Brush) Application.Current.Resources["AbortCommitBackgroundColor"];
            TextBlock.Text = Resx.DiscardingText;
            TextBlock.Foreground = (Brush) Application.Current.Resources["AbortCommitForegroundColor"];
            GlyphTextBlock.Text = (string) Application.Current.Resources["AbortCommitGlyph"];
            GlyphTextBlock.Foreground = (Brush) Application.Current.Resources["AbortCommitForegroundColor"];
         }

         MainBorder.BorderBrush = borderBrush;
         MainBorder.Background = backgroundBrush;
         CircleBorder.BorderBrush = borderBrush;
         CircleBorder.Background = backgroundBrush;
      }
   }
}
