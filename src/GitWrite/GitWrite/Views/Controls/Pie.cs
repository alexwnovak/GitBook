using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GitWrite.Views.Controls
{
   public class Pie : Shape
   {
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
