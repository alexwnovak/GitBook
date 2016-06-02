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
         new FrameworkPropertyMetadata( 0.0, FrameworkPropertyMetadataOptions.AffectsRender ) );

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

      protected override Geometry DefiningGeometry => CreateDefiningGeometry();

      private Geometry CreateDefiningGeometry()
      {
         double halfWidth = Width / 2;
         double halfHeight = Height / 2;

         var centerPoint = new Point( halfWidth, halfHeight );
         return new EllipseGeometry( centerPoint, halfWidth, halfHeight );
      }
   }
}
