using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GitWrite.Views.Controls
{
   public class Pie : Shape
   {
      public static DependencyProperty AngleProperty = DependencyProperty.Register( nameof( Angle ),
         typeof( double ),
         typeof( Pie ),
         new FrameworkPropertyMetadata( 0.0, FrameworkPropertyMetadataOptions.AffectsRender, OnAngleChanged ) );

      public double Angle
      {
         get
         {
            return (double) GetValue( AngleProperty );
         }
         set
         {
            SetValue( AngleProperty, value );
         }
      }

      public Pie()
      {
         Clip = new RectangleGeometry( Rect.Empty );
      }

      private static void OnAngleChanged( DependencyObject obj, DependencyPropertyChangedEventArgs e )
      {
         (obj as Pie)?.UpdateClip();
      }

      protected override Geometry DefiningGeometry => CreateDefiningGeometry();

      private Geometry CreateDefiningGeometry()
      {
         double halfWidth = Width / 2;
         double halfHeight = Height / 2;

         var centerPoint = new Point( halfWidth, halfHeight );
         return new EllipseGeometry( centerPoint, halfWidth, halfHeight );
      }

      private void UpdateClip()
      {
         if ( Angle >= 360 )
         {
            Clip = CreateDefiningGeometry();
            return;
         }

         double radius = Width / 2;
         double radians = ( Angle - 90 ) * Math.PI / 180;

         double x = radius * Math.Cos( radians ) + radius;
         double y = radius * Math.Sin( radians ) + radius;

         Clip = Geometry.Parse( $"M {radius},{radius} V 0 A {Width},{Height} 0 1 1 {x},{y}" );
      }
   }
}
