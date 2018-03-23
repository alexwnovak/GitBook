using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace GitWrite.Views
{
   public static class HintManager
   {
      private static readonly Dictionary<TextBox, string> _hintTable = new Dictionary<TextBox, string>();

      public static readonly DependencyProperty HintTextProperty = DependencyProperty.RegisterAttached(
         "HintText",
         typeof( string ),
         typeof( HintManager ),
         new PropertyMetadata( "" ) );

      public static string GetHintText( TextBox textBox ) => (string) textBox.GetValue( HintTextProperty );
      public static void SetHintText( TextBox textBox, string hintText ) => textBox.SetValue( HintTextProperty, hintText );
   }
}
