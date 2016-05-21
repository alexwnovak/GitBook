using System.Windows;
using System.Windows.Media;

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

      public static DependencyProperty TextColorProperty = DependencyProperty.Register( nameof( TextColor ),
         typeof( Color ),
         typeof( RadialCounter ),
         new FrameworkPropertyMetadata( Colors.White, FrameworkPropertyMetadataOptions.AffectsRender ) );

      public Color TextColor
      {
         get
         {
            return (Color) GetValue( TextColorProperty );
         }
         set
         {
            SetValue( TextColorProperty, value );
         }
      }

      public static DependencyProperty MinimumProperty = DependencyProperty.Register( nameof( Minimum ),
         typeof( int ),
         typeof( RadialCounter ),
         new FrameworkPropertyMetadata( 0, FrameworkPropertyMetadataOptions.AffectsRender, MinimumChanged, CoerceMinimum ) );

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

      private static void MinimumChanged( DependencyObject obj, DependencyPropertyChangedEventArgs e )
      {
         obj.CoerceValue( ValueProperty );
      }

      private static object CoerceMinimum( DependencyObject obj, object baseValue )
      {
         var radialCounter = (RadialCounter) obj;
         int newMinimum = (int) baseValue;

         if ( newMinimum > radialCounter.Maximum )
         {
            return radialCounter.Maximum;
         }

         return newMinimum;
      }

      public static DependencyProperty MaximumProperty = DependencyProperty.Register( nameof( Maximum ),
         typeof( int ),
         typeof( RadialCounter ),
         new FrameworkPropertyMetadata( 100, FrameworkPropertyMetadataOptions.AffectsRender, MaximumChanged, CoerceMaximum ) );

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

      private static void MaximumChanged( DependencyObject obj, DependencyPropertyChangedEventArgs e )
      {
         obj.CoerceValue( ValueProperty );
      }

      private static object CoerceMaximum( DependencyObject obj, object baseValue )
      {
         var radialCounter = (RadialCounter) obj;
         int newMaximum = (int) baseValue;

         if ( newMaximum < radialCounter.Minimum )
         {
            return radialCounter.Minimum;
         }

         return newMaximum;
      }

      public static DependencyProperty ValueProperty = DependencyProperty.Register( nameof( Value ),
         typeof( int ),
         typeof( RadialCounter ),
         new FrameworkPropertyMetadata( 0, FrameworkPropertyMetadataOptions.AffectsRender, ( _, __ ) => { }, CoerceValue ) );

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

      private static object CoerceValue( DependencyObject obj, object baseValue )
      {
         var radialCounter = (RadialCounter) obj;
         int newValue = (int) baseValue;

         if ( newValue > radialCounter.Maximum )
         {
            return radialCounter.Maximum;
         }
         if ( newValue < radialCounter.Minimum )
         {
            return radialCounter.Minimum;
         }

         return newValue;
      }
   }
}
