using System.Windows;

namespace GitWrite.Animation
{
   public class AnimationBuilder
   {
      private readonly UIElement _element;

      public PositionAnimator Position() => new PositionAnimator( _element );

      public AnimationBuilder( UIElement element )
      {
         _element = element;
      }
   }
}
