using System.Windows;
using System.Windows.Controls;

namespace GitWrite.Animation
{
   public static class FrameworkExtensions
   {
      public static IAnimator Animate( this FrameworkElement f )
      {
         return new Animator( f );
      }
   }

   public interface IAnimator
   {
      DoubleAnimationBuilder Height();
      DoubleAnimationBuilder Opacity();
      ColorAnimationBuilder Background();
   }

   public class Animator : IAnimator
   {
      private readonly FrameworkElement _frameworkElement;

      internal Animator( FrameworkElement frameworkElement )
      {
         _frameworkElement = frameworkElement;
      }

      public DoubleAnimationBuilder Height()
         => new DoubleAnimationBuilder( _frameworkElement, FrameworkElement.HeightProperty );
      
      public DoubleAnimationBuilder Opacity()
         => new DoubleAnimationBuilder( _frameworkElement, UIElement.OpacityProperty );

      public ColorAnimationBuilder Background()
         => new ColorAnimationBuilder( _frameworkElement, Control.BackgroundProperty );
   }
}
