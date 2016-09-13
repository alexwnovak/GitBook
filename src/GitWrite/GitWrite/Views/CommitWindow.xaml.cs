using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using GitWrite.ViewModels;

namespace GitWrite.Views
{
   public partial class CommitWindow : WindowBase
   {
      private readonly CommitViewModel _viewModel;

      public CommitWindow()
      {
         InitializeComponent();

         _viewModel = (CommitViewModel) DataContext;
         _viewModel.ExpansionRequested += OnExpansionRequested;
         _viewModel.CollapseRequested += OnCollapseRequested;
         _viewModel.AsyncExitRequested += OnAsyncExitRequested;
      }

      private void CommitWindow_OnLoaded( object sender, RoutedEventArgs e )
      {
         MainEntryBox.HideCaret();
         MainEntryBox.MoveCaretToEnd();

         Opacity = 0;

         const double duration = 100;

         var storyboard = new Storyboard();

         var opacityAnimation = new DoubleAnimation( 0, 1, new Duration( TimeSpan.FromMilliseconds( duration ) ) );

         storyboard.Children.Add( opacityAnimation );
         storyboard.Completed += ( _, __ ) => MainEntryBox.ShowCaret();

         Storyboard.SetTargetProperty( opacityAnimation, new PropertyPath( nameof( Opacity ) ) );
         Storyboard.SetTarget( opacityAnimation, this );

         storyboard.Begin();
      }

      private Task OnAsyncExitRequested( object sender, ShutdownEventArgs e )
      {
         if ( _viewModel.IsExiting )
         {
            return Task.CompletedTask;
         }

         var frontMaterial = (ImageSource) Resources["FrontMaterial"];

         var drawingVisual = new DrawingVisual();
         var dpiScale = VisualTreeHelper.GetDpi( drawingVisual );

         using ( var drawingContext = drawingVisual.RenderOpen() )
         {
            var foregroundBrush = (Brush) Application.Current.Resources["TextColor"];
            drawingContext.DrawImage( frontMaterial, new Rect( 0, 0, frontMaterial.Width, frontMaterial.Height) );

            if ( !string.IsNullOrWhiteSpace( _viewModel.ShortMessage ) )
            {
               var formattedText = new FormattedText( _viewModel.ShortMessage, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface( "Consolas" ), 24, foregroundBrush, dpiScale.DpiScaleX );
               drawingContext.DrawText( formattedText, new Point( 28, 36 ) );
            }
         }

         var image = new DrawingImage( drawingVisual.Drawing );
         FrontMaterial.Brush = new ImageBrush( image )
         {
            Stretch = Stretch.Uniform
         };

         ImageSource backMaterial;

         if ( e.ExitReason == ExitReason.Save )
         {
            backMaterial = (ImageSource) Resources["SaveBackMaterial"];
         }
         else
         {
            backMaterial = (ImageSource) Resources["DiscardBackMaterial"];
         }

         BackMaterial.Brush = new ImageBrush( backMaterial )
         {
            Stretch = Stretch.Uniform
         };

         Camera.Position = new Point3D( 0, 0, 5.67 );

         MainEntryBox.Visibility = Visibility.Collapsed;
         Viewport.Visibility = Visibility.Visible;

         var tcs = new TaskCompletionSource<bool>();

         var rotationAnimation = new DoubleAnimation( 0, 180, new Duration( TimeSpan.FromMilliseconds( 600 ) ) )
         {
            AccelerationRatio = 0.7,
            DecelerationRatio = 0.3,
            EasingFunction = new BackEase
            {
               Amplitude = 0.65,
               EasingMode = EasingMode.EaseOut
            }
         };

         var storyboard = new Storyboard();
         Storyboard.SetTargetName( rotationAnimation, "RotationTransform" );
         Storyboard.SetTargetProperty( rotationAnimation, new PropertyPath( AxisAngleRotation3D.AngleProperty ) );
         storyboard.Children.Add( rotationAnimation );

         storyboard.Completed += ( _, __ ) =>
         {
            tcs.SetResult( true );
         };

         storyboard.Begin( this );

         return tcs.Task;
      }

      private Task OnExpansionRequested( object sender, EventArgs eventArgs )
      {
         var tcs = new TaskCompletionSource<bool>();
         const double duration = 200;

         var heightAnimation = new DoubleAnimation
         {
            To = 300,
            Duration = new Duration( TimeSpan.FromMilliseconds( duration ) ),
            EasingFunction = new CircleEase
            {
               EasingMode = EasingMode.EaseOut,
            }
         };

         Storyboard.SetTarget( heightAnimation, SecondaryBorder );
         Storyboard.SetTargetProperty( heightAnimation, new PropertyPath( nameof( Height ) ) );

         var storyboard = new Storyboard();
         storyboard.Children.Add( heightAnimation );
         storyboard.Completed += ( _, __ ) => tcs.SetResult( true );

         Storyboard.SetTarget( storyboard, this );
         storyboard.Begin();

         return tcs.Task;
      }

      private Task OnCollapseRequested( object sender, EventArgs eventArgs )
      {
         var tcs = new TaskCompletionSource<bool>();
         const double duration = 200;

         var heightAnimation = new DoubleAnimation
         {
            To = 0,
            Duration = new Duration( TimeSpan.FromMilliseconds( duration ) ),
            EasingFunction = new CircleEase
            {
               EasingMode = EasingMode.EaseOut
            }
         };

         Storyboard.SetTarget( heightAnimation, SecondaryBorder );
         Storyboard.SetTargetProperty( heightAnimation, new PropertyPath( nameof( Height ) ) );

         var storyboard = new Storyboard();
         storyboard.Children.Add( heightAnimation );
         storyboard.Completed += ( _, __ ) => tcs.SetResult( true );

         Storyboard.SetTarget( storyboard, this );
         storyboard.Begin();

         return tcs.Task;
      }

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
