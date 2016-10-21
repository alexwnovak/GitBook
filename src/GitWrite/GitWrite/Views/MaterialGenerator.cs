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
      private readonly Size _scaledSize;
      
      public MaterialGenerator( FrameworkElement host )
      {
         _host = host;
         _dpiScale = VisualTreeHelper.GetDpi( host );
         _size = new Size( host.ActualWidth, WindowValues.NonExpandedHeight );
         _scaledSize = new Size( _size.Width * _dpiScale.DpiScaleX, _size.Height * _dpiScale.DpiScaleY );
      }

      public Task GenerateAsync( string saveText )
      {
         var tcs = new TaskCompletionSource<bool>();

         var thread = new Thread( () =>
         {
            GenerateSaveMaterial( saveText );
            GenerateDiscardMaterial();
            tcs.SetResult( true );
         } );

         thread.SetApartmentState( ApartmentState.STA );
         thread.Start();

         GenerateFrontMaterial();

         return tcs.Task;
      }

      private void GenerateFrontMaterial()
      {
         var renderTargetBitmap = new RenderTargetBitmap( (int) _scaledSize.Width, (int) _scaledSize.Height, _dpiScale.PixelsPerInchX, _dpiScale.PixelsPerInchY, PixelFormats.Pbgra32 );

         var frontBox = new MainEntryBox
         {
            Width = _size.Width,
            Height = _size.Height,
         };

         frontBox.Measure( _size );
         frontBox.Arrange( new Rect( _size ) );

         renderTargetBitmap.Render( frontBox );
         renderTargetBitmap.Freeze();

         _host.Dispatcher.Invoke( () => _host.Resources["FrontMaterial"] = renderTargetBitmap );
      }

      private void GenerateSaveMaterial( string exitText ) => GenerateTransitionMaterial( ExitReason.Save, "SaveBackMaterial", exitText );

      private void GenerateDiscardMaterial() => GenerateTransitionMaterial( ExitReason.Discard, "DiscardBackMaterial" );

      private void GenerateTransitionMaterial( ExitReason exitReason, string resourceKey, string exitText = null )
      {
         var renderTargetBitmap = new RenderTargetBitmap( (int) _scaledSize.Width, (int) _scaledSize.Height, _dpiScale.PixelsPerInchX, _dpiScale.PixelsPerInchY, PixelFormats.Pbgra32 );
         TransitionEntryBox transitionEntryBox;

         if ( string.IsNullOrEmpty( exitText ) )
         {
            transitionEntryBox = new TransitionEntryBox( exitReason );
         }
         else
         {
            transitionEntryBox = new TransitionEntryBox( exitReason, exitText );
         }

         transitionEntryBox.Width = _size.Width;
         transitionEntryBox.Height = _size.Height;
         transitionEntryBox.RenderTransform = new ScaleTransform( 1, -1 );
         transitionEntryBox.RenderTransformOrigin = new Point( 0.5, 0.5 );

         transitionEntryBox.Measure( _size );
         transitionEntryBox.Arrange( new Rect( _size ) );

         renderTargetBitmap.Render( transitionEntryBox );
         renderTargetBitmap.Freeze();

         _host.Dispatcher.Invoke( () => _host.Resources[resourceKey] = renderTargetBitmap );
      }
   }
}
