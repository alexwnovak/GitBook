using System.Windows;

namespace GitWrite.Views
{
   public static class WindowValues
   {
      public static readonly double RestingPointY = 0.7 * ( SystemParameters.FullPrimaryScreenHeight - 30 ) / 2;
      public static readonly double StartingPointY = RestingPointY + 20;

      public static readonly double NonExpandedHeight = 100;
      public static readonly double ExpandedHeight = 400;
   }
}
