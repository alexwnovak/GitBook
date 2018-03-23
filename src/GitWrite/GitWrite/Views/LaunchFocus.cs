using System;
using System.Windows;
using System.Windows.Controls;

namespace GitWrite.Views
{
   public class LaunchFocus : DependencyObject
   {
      public static readonly DependencyProperty IsFocusTargetProperty = DependencyProperty.RegisterAttached( 
         "IsFocusTarget",
         typeof( bool ),
         typeof( TextBox ),
         new PropertyMetadata( false ) );

      public static void SetIsFocusTarget( TextBox textBox, bool isFocusTarget )
      {
         if ( isFocusTarget )
         {
            textBox.Loaded += ( _, __ ) =>
            {
               textBox.Focus();
               textBox.SelectionStart = textBox.Text.Length;
            };
         }
      }

      public static bool GetIsFocusTarget( TextBox textBox )
      {
         throw new NotImplementedException();
      }
   }
}
