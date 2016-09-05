using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using GitWrite.ViewModels;
using GitWrite.Views.Controls;

namespace GitWrite.Views
{
   public partial class CommitWindow : WindowBase
   {
      private readonly CommitViewModel _viewModel;

      public double HelpScrollDistance
      {
         get
         {
            return (double) GetValue( HelpScrollDistanceProperty );
         }
         set
         {
            SetValue( HelpScrollDistanceProperty, value );
         }
      }

      public static readonly DependencyProperty HelpScrollDistanceProperty = DependencyProperty.Register( nameof( HelpScrollDistance ),
         typeof( double ),
         typeof( CommitWindow ),
         new PropertyMetadata( 0.0 ) );

      public CommitWindow()
      {
         InitializeComponent();

         _viewModel = (CommitViewModel) DataContext;
         _viewModel.ExpansionRequested += OnExpansionRequested;
         _viewModel.CollapseRequested += OnCollapseRequested;
         _viewModel.HelpRequested += OnHelpRequested;
         _viewModel.CollapseHelpRequested += OnCollapseHelpRequested;
         _viewModel.AsyncExitRequested += OnAsyncExitRequested;
      }

      private Task OnAsyncExitRequested( object sender, EventArgs e )
      {
         var dpiScale = VisualTreeHelper.GetDpi( MainEntryBox );
         var size = new Size( MainEntryBox.ActualWidth, MainEntryBox.ActualHeight );

         var frontBitmap = new RenderTargetBitmap( (int) size.Width * (int) dpiScale.DpiScaleX, (int) size.Height * (int) dpiScale.DpiScaleY, dpiScale.PixelsPerInchX, dpiScale.PixelsPerInchY, PixelFormats.Pbgra32 );
         var backBitmap = new RenderTargetBitmap( (int) size.Width * (int) dpiScale.DpiScaleX, (int) size.Height * (int) dpiScale.DpiScaleY, dpiScale.PixelsPerInchX, dpiScale.PixelsPerInchY, PixelFormats.Pbgra32 );

         MainEntryBox.Measure( size );
         MainEntryBox.Arrange( new Rect( size ) );

         frontBitmap.Render( MainEntryBox );

         DrawingVisual drawingVisual = new DrawingVisual();
         using ( DrawingContext context = drawingVisual.RenderOpen() )
         {
            var backBox = new TransitionEntryBox( ExitReason.Save )
            {
               Width = size.Width,
               Height = size.Height,
               RenderTransform = new ScaleTransform( 1, -1 )
            };

            backBox.Measure( size );
            backBox.Arrange( new Rect( size ) );

            VisualBrush visualBrush = new VisualBrush( backBox );
            context.DrawRectangle( visualBrush, null, new Rect( size ) );
         }

         backBitmap.Render( drawingVisual );

         FrontMaterial.Brush = new ImageBrush( frontBitmap )
         {
            Stretch = Stretch.Uniform
         };

         BackMaterial.Brush = new ImageBrush( backBitmap )
         {
            Stretch = Stretch.Uniform
         };

         Camera.Position = new Point3D( 0, 0, 5.67 );

         MainEntryBox.Visibility = Visibility.Collapsed;
         Viewport.Visibility = Visibility.Visible;

         var tcs = new TaskCompletionSource<bool>();

         var animation = new DoubleAnimation( 0, 180, new Duration( TimeSpan.FromMilliseconds( 600 ) ) )
         {
            AccelerationRatio = 0.7,
            DecelerationRatio = 0.3,
            EasingFunction = new BackEase
            {
               Amplitude = 0.65,
               EasingMode = EasingMode.EaseOut
            }
         };

         void OnAnimationCompleted( object _, EventArgs __ )
         {
            animation.Completed -= OnAnimationCompleted;

            Thread.Sleep( 500 );
            tcs.SetResult( true );
         }

         animation.Completed += OnAnimationCompleted;
         RotationTransform.BeginAnimation( AxisAngleRotation3D.AngleProperty, animation );

         return tcs.Task;
      }

      private void OnHelpRequested( object sender, EventArgs e )
      {
         HelpScrollDistance = -ActualHeight;

         this.PlayStoryboard( "ActivateHelpStoryboard" );
      }

      private void OnCollapseHelpRequested( object sender, EventArgs e )
         => this.PlayStoryboard( "CollapseHelpStoryboard" );

      private void OnExpansionRequested( object sender, EventArgs eventArgs )
         => VisualStateManager.GoToElementState( MainGrid, "Expanded", false );

      private void OnCollapseRequested( object sender, EventArgs eventArgs )
         => VisualStateManager.GoToElementState( MainGrid, "Collapsed", false );

      private void CommitWindow_OnPreviewCanExecute( object sender, CanExecuteRoutedEventArgs e )
      {
         //if ( e.Command == ApplicationCommands.Paste )
         //{
         //   e.CanExecute = false;
         //   e.Handled = true;
         //}
      }
   }
}
