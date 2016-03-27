using System.Windows;

namespace GitWrite.FluentAnimation
{
   public static class UIElementExtensions
   {
      public static AnimationBuilder Animate( this UIElement element ) => new AnimationBuilder( element );
   }
}
