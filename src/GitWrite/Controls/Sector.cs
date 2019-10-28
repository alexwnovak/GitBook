using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GitWrite.Controls
{
   public class Sector : Shape
   {
      public static readonly DependencyProperty AngleProperty = DependencyProperty.Register(
         nameof( Angle ),
         typeof( double ),
         typeof( Sector ),
         new FrameworkPropertyMetadata( 360.0, FrameworkPropertyMetadataOptions.AffectsRender ) );

      public double Angle
      {
         get => (double) GetValue( AngleProperty );
         set => SetValue( AngleProperty, value );
      }

      protected override Geometry DefiningGeometry
      {
         get
         {
            var geometry = new StreamGeometry();

            using ( StreamGeometryContext context = geometry.Open() )
            {
               double centerX = RenderSize.Width / 2;
               double centerY = RenderSize.Height / 2;
               var centerPoint = new Point( centerX, centerY );
               double radians = ( Angle - 90 ) * Math.PI / 180;

               context.BeginFigure( centerPoint, isFilled: true, isClosed: false );

               // This initial line starts from the center point and goes up to the top/middle,
               // so the arc is swept clockwise from there, but anchored in the center
               context.LineTo( new Point( centerX, 0 ), isStroked: false, isSmoothJoin: false );

               // Sweeping an arc past 180 degrees makes the figure concave, which messes it up.
               // If we need to draw more than 180 degrees, we'll do it in two passes

               if ( Angle > 180 )
               {
                  // This fills the first half up to 180 degrees

                  context.ArcTo( new Point( centerX, RenderSize.Height ), new Size( centerX, centerY ), 180, false, SweepDirection.Clockwise, false, false );
               }

               // This next part fills in either the remainder of whatever is past 180 degrees,
               // or the entire arc if it was less than 180 degrees to begin with

               double endX = centerX + centerX * Math.Cos( radians );
               double endY = centerY + centerY * Math.Sin( radians );

               var endPoint = new Point( endX, endY );
               context.ArcTo( endPoint, new Size( centerX, centerY ), Angle, false, SweepDirection.Clockwise, false, false );
            }

            geometry.Freeze();
            return geometry;
         }
      }
   }
}
