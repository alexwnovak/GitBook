using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media.Animation;
using GalaSoft.MvvmLight.Ioc;
using GitWrite.Animation;
using GitWrite.Services;

namespace GitWrite.Behaviors
{
   public class WindowDragBehavior : Behavior<Window>
   {
      protected override void OnAttached()
      {
         AssociatedObject.MouseDown += OnMouseDown;
         AssociatedObject.MouseDoubleClick += OnMouseDoubleClick;
      }
      protected override void OnDetaching()
      {
         AssociatedObject.MouseDown -= OnMouseDown;
         AssociatedObject.MouseDoubleClick -= OnMouseDoubleClick;
      }

      private void OnMouseDown( object sender, MouseButtonEventArgs e )
      {
         if ( e.LeftButton == MouseButtonState.Pressed )
         {
            AssociatedObject.DragMove();

            var appSettings = SimpleIoc.Default.GetInstance<IApplicationSettings>();

            appSettings.WindowX = (int) AssociatedObject.RestoreBounds.Left;
            appSettings.WindowY = (int) AssociatedObject.RestoreBounds.Top;
         }
      }

      private async void OnMouseDoubleClick( object sender, MouseButtonEventArgs e )
      {
         double x = ( SystemParameters.FullPrimaryScreenWidth - AssociatedObject.Width ) / 2;
         double y = 0.7 * ( SystemParameters.FullPrimaryScreenHeight - 30 ) / 2;

         await AssociatedObject.Animate()
            .Position()
            .From( 500, 500 )
            .To( x, y )
            .For( 2000 )
            .EaseOut<CircleEase>()
            .PlayAsync();

         var appSettings = SimpleIoc.Default.GetInstance<IApplicationSettings>();

         //appSettings.WindowX = (int) AssociatedObject.RestoreBounds.Left;
         //appSettings.WindowY = (int) AssociatedObject.RestoreBounds.Top;
      }
   }
}
