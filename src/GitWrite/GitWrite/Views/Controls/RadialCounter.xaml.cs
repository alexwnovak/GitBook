using System.Windows;
using System.Windows.Controls;

namespace GitWrite.Views.Controls
{
   public partial class RadialCounter : UserControl
   {
      public static DependencyProperty ValueProperty = DependencyProperty.Register( nameof( Value ),
         typeof( int ),
         typeof( RadialCounter ),
         new FrameworkPropertyMetadata( 0, FrameworkPropertyMetadataOptions.AffectsRender, null ) );

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
   }
}
