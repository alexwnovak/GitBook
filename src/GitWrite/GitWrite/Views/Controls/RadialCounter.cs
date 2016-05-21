using System.Windows;

namespace GitWrite.Views.Controls
{
   public class RadialCounter : FrameworkElement
   {
      public static DependencyProperty MinimumProperty = DependencyProperty.Register( nameof( Minimum ),
         typeof( int ),
         typeof( RadialCounter ),
         new FrameworkPropertyMetadata( 0, FrameworkPropertyMetadataOptions.AffectsRender ) );

      public int Minimum
      {
         get
         {
            return (int) GetValue( MinimumProperty );
         }
         set
         {
            SetValue( MinimumProperty, value );
         }
      }

      public static DependencyProperty MaximumProperty = DependencyProperty.Register( nameof( Maximum ),
         typeof( int ),
         typeof( RadialCounter ),
         new FrameworkPropertyMetadata( 100, FrameworkPropertyMetadataOptions.AffectsRender ) );

      public int Maximum
      {
         get
         {
            return (int) GetValue( MaximumProperty );
         }
         set
         {
            SetValue( MaximumProperty, value );
         }
      }
   }
}
