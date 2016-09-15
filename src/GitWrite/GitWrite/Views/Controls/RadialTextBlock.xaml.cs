using System.Windows;
using System.Windows.Controls;

namespace GitWrite.Views.Controls
{
   public partial class RadialTextBlock : UserControl
   {
      public static DependencyProperty TextProperty = MainEntryBox.TextProperty.AddOwner( typeof( RadialTextBlock ) );

      public string Text
      {
         get
         {
            return (string) GetValue( TextProperty );
         }
         set
         {
            SetValue( TextProperty, value );
         }
      }

      public RadialTextBlock()
      {
         InitializeComponent();
      }
   }
}
