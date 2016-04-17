using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace GitWrite.Views.Controls
{
   public class ItemSelectionAdorner : Adorner
   {
      public ItemSelectionAdorner( UIElement adornedElement )
         : base( adornedElement )
      {
      }

      protected override void OnRender( DrawingContext drawingContext )
      {
         var size = AdornedElement.DesiredSize;
         var rect = new Rect( new Point( 0, 0 ), new Point( ActualWidth, size.Height ) );

         var brush = (Brush) Application.Current.Resources["HighlightColor"];
         drawingContext.DrawRectangle( brush, null, rect );
      }
   }
}
