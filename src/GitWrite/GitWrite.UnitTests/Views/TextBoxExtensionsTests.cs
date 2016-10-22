using System;
using System.Windows.Controls;
using FluentAssertions;
using Xunit;
using GitWrite.Views;

namespace GitWrite.UnitTests.Views
{
   public class TextBoxExtensionsTests
   {
      [StaFact]
      public void MoveCaretToEnd_TextIsBlank_CaretDoesNotMove()
      {
         var textBox = new TextBox();

         textBox.MoveCaretToEnd();

         textBox.SelectionStart.Should().Be( textBox.Text.Length );
         textBox.SelectionLength.Should().Be( 0 );
      }

      [StaFact]
      public void MoveCaretToEnd_HasText_CaretJumpsToTheEnd()
      {
         var textBox = new TextBox
         {
            Text = "Some text"
         };

         textBox.MoveCaretToEnd();

         textBox.SelectionStart.Should().Be( textBox.Text.Length );
         textBox.SelectionLength.Should().Be( 0 );
      }
   }
}
