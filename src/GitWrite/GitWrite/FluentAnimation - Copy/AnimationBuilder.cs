using System.Windows;

namespace GitWrite.FluentAnimation
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
