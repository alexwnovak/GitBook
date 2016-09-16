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

      public static DependencyProperty IsProgressRingVisibleProperty = DependencyProperty.Register( nameof( IsProgressRingVisible ),
         typeof( bool ),
         typeof( RadialTextBlock ),
         new PropertyMetadata( true ) );

      public bool IsProgressRingVisible
      {
         get
         {
            return (bool) GetValue( IsProgressRingVisibleProperty );
         }
         set
         {
            SetValue( IsProgressRingVisibleProperty, value );
         }
      }

      public static DependencyProperty ProgressProperty = DependencyProperty.Register( nameof( Progress ),
         typeof( double ),
         typeof( RadialTextBlock ),
         new PropertyMetadata( 0.0 ) );

      public double Progress
      {
         get
         {
            return (double) GetValue( ProgressProperty );
         }
         set
         {
            SetValue( ProgressProperty, value );
         }
      }

      public RadialTextBlock()
      {
         InitializeComponent();
      }
   }
}
