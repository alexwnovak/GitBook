using System;
using System.Runtime.Remoting.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace GitWrite.Animation
{
   public class PositionAnimator
   {
      private UIElement _element;
      private Point _from;
      private Point _to;
      private TimeSpan _duration;
      private EasingFunctionBase _easingFunction;

      public PositionAnimator( UIElement element )
      {
         _element = element;

         if ( element is Window )
         {
            var window = (Window) element;

            double x = window.Left;
            double y = window.Top;

            Point screenCoordinates = window.PointToScreen( new Point( 0, 0 ) );

            _from = new Point( screenCoordinates.X, screenCoordinates.Y );
            //_from = new Point( x, y);
         }
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

      public PositionAnimator EaseIn<T>() where T : EasingFunctionBase, new()
      {
         _easingFunction = new T
         {
            EasingMode = EasingMode.EaseIn
         };

         return this;
      }

      public PositionAnimator EaseOut<T>() where T : EasingFunctionBase, new()
      {
         _easingFunction = new T
         {
            EasingMode = EasingMode.EaseOut
         };

         return this;
      }

      public PositionAnimator EaseInOut<T>() where T : EasingFunctionBase, new()
      {
         _easingFunction = new T
         {
            EasingMode = EasingMode.EaseInOut
         };

         return this;
      }

      public Task PlayAsync()
      {
         var horizontalAnimation = new DoubleAnimation
         {
            From = _from.X,
            To = _to.X,
            EasingFunction = _easingFunction
         };

         var verticalAnimation = new DoubleAnimation
         {
            From = _from.Y,
            To = _to.Y,
            EasingFunction = _easingFunction
         };

         if ( _element is Window )
         {
            var window = (Window) _element;
            var tcs = new TaskCompletionSource<bool>();
            //horizontalAnimation.Completed += ( sender, e ) => tcs.SetResult( true );

            //var tcs2 = new TaskCompletionSource<bool>();
            //verticalAnimation.Completed += ( sender, e ) => tcs2.SetResult( true );

            //_element.BeginAnimation( Window.LeftProperty, horizontalAnimation );
            //_element.BeginAnimation( Window.TopProperty, verticalAnimation );

            var storyboard = new Storyboard
            {
               //Duration = new Duration( _duration )
               Duration = new Duration( TimeSpan.FromSeconds( 2 ))
            };

            storyboard.Children.Add( horizontalAnimation );
            storyboard.Children.Add( verticalAnimation );

            storyboard.Completed += ( sender, e ) =>
            {
               _from = new Point( window.Left, window.Top );
               tcs.SetResult( true );
               _element = null;
            };

            Storyboard.SetTarget( horizontalAnimation, _element );
            Storyboard.SetTargetProperty( horizontalAnimation, new PropertyPath( Window.LeftProperty ) );

            Storyboard.SetTarget( verticalAnimation, _element );
            Storyboard.SetTargetProperty( verticalAnimation, new PropertyPath( Window.TopProperty ) );

            storyboard.Begin();

            return tcs.Task;

            //return Task.WhenAll( tcs.Task, tcs2.Task );
         }

         throw new NotImplementedException();
      }
   }
}
