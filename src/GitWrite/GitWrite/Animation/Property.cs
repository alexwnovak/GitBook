using System.Windows;

namespace GitWrite.Animation
{
   public class Property
   {
      public static Property Height = new Property( FrameworkElement.HeightProperty );

      internal DependencyProperty DependencyProperty
      {
         get;
      }

      private Property( DependencyProperty dependencyProperty )
      {
         DependencyProperty = dependencyProperty;
      }
   }
}
