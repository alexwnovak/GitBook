using System.Windows.Controls;

namespace GitWrite.Views
{
   public static class TextBoxExtensions
   {
      public static void MoveCaretToEnd( this TextBox textBox ) => textBox.SelectionStart = textBox.Text.Length;
   }
}
