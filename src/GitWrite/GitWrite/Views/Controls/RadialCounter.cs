using System.Windows;

namespace GitWrite.Views.Controls
{
   public class RadialCounter : FrameworkElement
   {
      public static DependencyProperty FontSizeProperty = DependencyProperty.Register( nameof( FontSize ),
         typeof( double ),
         typeof( RadialCounter ),
         new FrameworkPropertyMetadata( 12.0, FrameworkPropertyMetadataOptions.AffectsRender ) );

      public double FontSize
      {
         get
         {
            return (double) GetValue( FontSizeProperty );
         }
         set
         {
            SetValue( FontSizeProperty, value );
         }
      }

      public static DependencyProperty FontFamilyProperty = DependencyProperty.Register( nameof( FontFamily ),
         typeof( string ),
         typeof( RadialCounter ),
         new FrameworkPropertyMetadata( "Segoe UI", FrameworkPropertyMetadataOptions.AffectsRender ) );

      public string FontFamily
      {
         get
         {
            return (string) GetValue( FontFamilyProperty );
         }
         set
         {
            SetValue( FontFamilyProperty, value );
         }
      }

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
