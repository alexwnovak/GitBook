using System.Windows;
using System.Windows.Controls;

namespace GitWrite.Views.Controls
{
   public partial class RadialCounter : UserControl
   {
      public static DependencyProperty ValueProperty = DependencyProperty.Register( nameof( Value ),
         typeof( int ),
         typeof( RadialCounter ),
         new FrameworkPropertyMetadata( 0, FrameworkPropertyMetadataOptions.AffectsRender, OnValueChanged ) );

      public int Value
      {
         get
         {
            return (int) GetValue( ValueProperty );
         }
         set
         {
            SetValue( ValueProperty, value );
         }
      }

      public RadialCounter()
      {
         InitializeComponent();
      }

      private static void OnValueChanged( DependencyObject obj, DependencyPropertyChangedEventArgs e )
      {
         var radialCounter = (RadialCounter) obj;
         radialCounter.SetCounterValue( (int) e.OldValue, (int) e.NewValue );
      }

      private void SetCounterValue( int oldValue, int newValue )
      {
         if ( newValue < 10 )
         {
            LeftDigitText.Visibility = Visibility.Collapsed;
            RightDigitText.Text = newValue.ToString();
         }
         else
         {
            LeftDigitText.Visibility = Visibility.Visible;

            int leftDigit = newValue / 10;
            int rightDigit = newValue % 10;

            LeftDigitText.Text = leftDigit.ToString();
            RightDigitText.Text = rightDigit.ToString();
         }
      }
   }
}
