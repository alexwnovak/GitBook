using System.Windows.Controls;

namespace GitWrite.Views
{
   public static class TextBoxExtensions
   {
      public static void HighlightLine( this TextBox textBox, int lineIndex )
      {
         textBox.SelectionStart = textBox.GetCharacterIndexFromLineIndex( lineIndex );
         textBox.SelectionLength = textBox.GetLineLength( lineIndex );
      }
   }
}
