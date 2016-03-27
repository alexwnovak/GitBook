using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace GitWrite.FluentAnimation
{
   public class PositionAnimator
   {
      private readonly UIElement _element;
      private Point _from;
      private Point _to;
      private TimeSpan _duration;

      public PositionAnimator( UIElement element )
      {
         _element = element;
      }

      public PositionAnimator From( double x, double y )
      {
         _from = new Point( x, y );
         return this;
      }

      public PositionAnimator To( double x, double y )
      {
         _to = new Point( x, y );
         return this;
      }

      public PositionAnimator For( double milliseconds )
      {
         _duration = TimeSpan.FromMilliseconds( milliseconds );
         return this;
      }

      public void Play()
      {
         var horizontalAnimation = new DoubleAnimation
         {
            From = _from.X,
            To = _to.X,
            Duration = new Duration( _duration )
         };

         var verticalAnimation = new DoubleAnimation
         {
            From = _from.Y,
            To = _to.Y,
            Duration = new Duration( _duration )
         };

         if ( _element is Window )
         {
            _element.BeginAnimation( Window.LeftProperty, horizontalAnimation );
            _element.BeginAnimation( Window.TopProperty, verticalAnimation );
         }
      }
   }
}
