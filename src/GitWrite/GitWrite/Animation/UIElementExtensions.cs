using System.Windows;

namespace GitWrite.Animation
{
   public static class UIElementExtensions
   {
      public static AnimationBuilder Animate( this UIElement element ) => new AnimationBuilder( element );
   }
}
