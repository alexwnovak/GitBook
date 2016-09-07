using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GitWrite.ViewModels;
using GitWrite.Views.Controls;

namespace GitWrite.Views
{
   public class MaterialGenerator
   {
      private readonly FrameworkElement _host;
      private readonly DpiScale _dpiScale;
      private readonly Size _size;

      public MaterialGenerator( FrameworkElement host )
      {
         _host = host;
         _dpiScale = VisualTreeHelper.GetDpi( host );
         _size = new Size( host.ActualWidth, WindowValues.NonExpandedHeight );
      }

      public Task GenerateAsync()
      {
         var t1 = StartNewStaTask( GenerateFrontMaterial  );
         var t2 = StartNewStaTask( GenerateSaveMaterial );
         var t3 = StartNewStaTask( GenerateDiscardMaterial );

         return Task.WhenAll( t1, t2, t3 );
      }

      private Task StartNewStaTask( Action action )
         => Task.Factory.StartNew( action, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext() );

      private void GenerateFrontMaterial()
      {
         var renderTargetBitmap = new RenderTargetBitmap( (int) _size.Width * (int) _dpiScale.DpiScaleX, (int) _size.Height * (int) _dpiScale.DpiScaleY, _dpiScale.PixelsPerInchX, _dpiScale.PixelsPerInchY, PixelFormats.Pbgra32 );

         var frontBox = new MainEntryBox
         {
            Width = _size.Width,
            Height = _size.Height,
         };

         frontBox.HideRadialText();

         frontBox.Measure( _size );
         frontBox.Arrange( new Rect( _size ) );

         renderTargetBitmap.Render( frontBox );
         renderTargetBitmap.Freeze();

         _host.Dispatcher.Invoke( () => _host.Resources["FrontMaterial"] = renderTargetBitmap );
      }

      private void GenerateSaveMaterial() => GenerateTransitionMaterial( ExitReason.Save, "SaveBackMaterial" );

      private void GenerateDiscardMaterial() => GenerateTransitionMaterial( ExitReason.Discard, "DiscardBackMaterial" );

      private void GenerateTransitionMaterial( ExitReason exitReason, string resourceKey )
      {
         var renderTargetBitmap = new RenderTargetBitmap( (int) _size.Width * (int) _dpiScale.DpiScaleX, (int) _size.Height * (int) _dpiScale.DpiScaleY, _dpiScale.PixelsPerInchX, _dpiScale.PixelsPerInchY, PixelFormats.Pbgra32 );

         var transitionEntryBox = new TransitionEntryBox( exitReason )
         {
            Width = _size.Width,
            Height = _size.Height,
            RenderTransform = new ScaleTransform( 1, -1 ),
            RenderTransformOrigin = new Point( 0.5, 0.5 )
         };

         transitionEntryBox.Measure( _size );
         transitionEntryBox.Arrange( new Rect( _size ) );

         renderTargetBitmap.Render( transitionEntryBox );
         renderTargetBitmap.Freeze();

         _host.Dispatcher.Invoke( () => _host.Resources[resourceKey] = renderTargetBitmap );
      }
   }
}
