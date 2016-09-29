using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Media3D;
using GalaSoft.MvvmLight.Messaging;
using GitWrite.Messages;
using GitWrite.Services;
using GitWrite.ViewModels;

namespace GitWrite.Views
{
   public partial class CommitWindow : WindowBase
   {
      private readonly CommitViewModel _viewModel;
      private readonly ISoundService _soundService = new SoundService();
      private readonly IApplicationSettings _applicationSettings = new ApplicationSettings( new RegistryService() );
      private bool _isCtrlDown;

      public CommitWindow()
      {
         InitializeComponent();

         MainEntryBox.MaxLength = _applicationSettings.MaxCommitLength;

         _viewModel = (CommitViewModel) DataContext;
         _viewModel.ExpansionRequested += OnExpansionRequested;
         _viewModel.CollapseRequested += OnCollapseRequested;
         _viewModel.ShakeRequested += OnShakeRequested;
         _viewModel.AsyncExitRequested += OnAsyncExitRequested;
      }

      private async void CommitWindow_OnLoaded( object sender, RoutedEventArgs e )
      {
         MainEntryBox.HideCaret();
         MainEntryBox.MoveCaretToEnd();

         Opacity = 0;

         await Task.Delay( 200 );

         const double duration = 200;

         var storyboard = new Storyboard();

         var opacityAnimation = new DoubleAnimation( 0, 1, new Duration( TimeSpan.FromMilliseconds( duration ) ) );
         var scaleXAnimation = new DoubleAnimation( 0.95, 1, opacityAnimation.Duration )
         {
            EasingFunction = new BackEase
            {
               EasingMode = EasingMode.EaseOut,
               Amplitude = 1
            }
         };
         var scaleYAnimation = new DoubleAnimation( 0.96, 1, opacityAnimation.Duration )
         {
            EasingFunction = new BackEase
            {
               EasingMode = EasingMode.EaseOut,
               Amplitude = 0.9
            }
         };

         storyboard.Children.Add( opacityAnimation );
         storyboard.Children.Add( scaleXAnimation );
         storyboard.Children.Add( scaleYAnimation );

         storyboard.Completed += ( _, __ ) => MainEntryBox.ShowCaret();

         Storyboard.SetTargetProperty( opacityAnimation, new PropertyPath( nameof( Opacity ) ) );
         Storyboard.SetTarget( opacityAnimation, this );

         Storyboard.SetTargetName( scaleXAnimation, nameof( ScaleTransform ) );
         Storyboard.SetTargetProperty( scaleXAnimation, new PropertyPath( ScaleTransform.ScaleXProperty ) );

         Storyboard.SetTargetName( scaleYAnimation, nameof( ScaleTransform ) );
         Storyboard.SetTargetProperty( scaleYAnimation, new PropertyPath( ScaleTransform.ScaleYProperty ) );

         storyboard.Begin( this );

         if ( _applicationSettings.PlaySoundOnLaunch )
         {
            _soundService.PlayPopSound();
         }
      }

      private DrawingImage GetFrontMaterialImage()
      {
         var frontMaterial = (ImageSource) Resources["FrontMaterial"];

         var drawingVisual = new DrawingVisual();
         var dpiScale = VisualTreeHelper.GetDpi( drawingVisual );

         using ( var drawingContext = drawingVisual.RenderOpen() )
         {
            var foregroundBrush = (Brush) Application.Current.Resources["TextColor"];
            drawingContext.DrawImage( frontMaterial, new Rect( 0, 0, frontMaterial.Width, frontMaterial.Height ) );

            if ( !string.IsNullOrWhiteSpace( _viewModel.ShortMessage ) )
            {
               var formattedText = new FormattedText( _viewModel.ShortMessage, CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface( "Consolas" ), 24, foregroundBrush, dpiScale.DpiScaleX );
               drawingContext.DrawText( formattedText, new Point( 28, 36 ) );
            }
         }

         return new DrawingImage( drawingVisual.Drawing );
      }

      private ImageSource GetBackMaterialImage( ExitReason exitReason )
      {
         if ( exitReason == ExitReason.Discard )
         {
            return (ImageSource) Resources["DiscardBackMaterial"];
         }

         return (ImageSource) Resources["SaveBackMaterial"];
      }

      private Brush GetMaterialBrush( ImageSource imageSource )
      {
         return new ImageBrush( imageSource )
         {
            Stretch = Stretch.Uniform
         };
      }

      private Task OnAsyncExitRequested( object sender, ShutdownEventArgs e )
      {
         if ( _viewModel.IsExiting )
         {
            return Task.CompletedTask;
         }

         var frontMaterialImage = GetFrontMaterialImage();
         FrontMaterial.Brush = GetMaterialBrush( frontMaterialImage );

         var backMaterialImage = GetBackMaterialImage( e.ExitReason );
         BackMaterial.Brush = GetMaterialBrush( backMaterialImage );

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

         var opacityAnimation = new DoubleAnimation( 1, 0, new Duration( TimeSpan.FromMilliseconds( 200 ) ) )
         {
            BeginTime = TimeSpan.FromMilliseconds( 700 )
         };

         var blurRadiusAnimation = new DoubleAnimation( 0, 6, opacityAnimation.Duration )
         {
            BeginTime = opacityAnimation.BeginTime,
            EasingFunction = new CircleEase
            {
               EasingMode = EasingMode.EaseOut
            }
         };

         var translateAnimation = new DoubleAnimation( 0, 25, opacityAnimation.Duration )
         {
            BeginTime = opacityAnimation.BeginTime
         };

         var scaleXAnimation = new DoubleAnimation( 1, 0.97, opacityAnimation.Duration )
         {
            BeginTime = opacityAnimation.BeginTime
         };

         var scaleYAnimation = new DoubleAnimation( 1, 0.97, opacityAnimation.Duration )
         {
            BeginTime = opacityAnimation.BeginTime
         };

         var storyboard = new Storyboard();
         Storyboard.SetTargetName( rotationAnimation, "RotationTransform" );
         Storyboard.SetTargetProperty( rotationAnimation, new PropertyPath( AxisAngleRotation3D.AngleProperty ) );

         Storyboard.SetTarget( opacityAnimation, MainGrid );
         Storyboard.SetTargetProperty( opacityAnimation, new PropertyPath( OpacityProperty ) );

         Storyboard.SetTargetName( blurRadiusAnimation, "BlurEffect" );
         Storyboard.SetTargetProperty( blurRadiusAnimation, new PropertyPath( BlurEffect.RadiusProperty ) );

         Storyboard.SetTargetName( translateAnimation, "TranslateTransform" );
         Storyboard.SetTargetProperty( translateAnimation, new PropertyPath( TranslateTransform.YProperty ) );

         Storyboard.SetTargetName( scaleXAnimation, "ScaleTransform" );
         Storyboard.SetTargetProperty( scaleXAnimation, new PropertyPath( ScaleTransform.ScaleXProperty ) );

         Storyboard.SetTargetName( scaleYAnimation, "ScaleTransform" );
         Storyboard.SetTargetProperty( scaleYAnimation, new PropertyPath( ScaleTransform.ScaleYProperty ) );

         storyboard.Children.Add( rotationAnimation );
         storyboard.Children.Add( opacityAnimation );
         storyboard.Children.Add( blurRadiusAnimation );
         storyboard.Children.Add( translateAnimation );
         storyboard.Children.Add( scaleXAnimation );
         storyboard.Children.Add( scaleYAnimation );

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

      private Task OnShakeRequested( object sender, EventArgs e )
      {
         var subject = MainEntryBox;
         var savedTransform = subject.RenderTransform;
         var translateTransform = new TranslateTransform();

         var shakeAnimation = new DoubleAnimation
         {
            From = 8,
            To = 0,
            Duration = new Duration( TimeSpan.FromMilliseconds( 600 ) ),
            EasingFunction = new ElasticEase
            {
               EasingMode = EasingMode.EaseOut,
               Oscillations = 3,
               Springiness = 1
            }
         };

         subject.RenderTransform = translateTransform;

         var tcs = new TaskCompletionSource<bool>();

         shakeAnimation.Completed += ( _, __ ) =>
         {
            subject.RenderTransform = savedTransform;
            tcs.SetResult( true );
         };

         translateTransform.BeginAnimation( TranslateTransform.XProperty, shakeAnimation );

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

      private void SecondaryNotes_OnTextChanged( object sender, TextChangedEventArgs e )
      {
         Messenger.Default.Send( new PulseRequestedMessage() );
      }

      private void CommitWindow_OnPreviewKeyDown( object sender, KeyEventArgs e )
      {
         if ( e.Key == Key.LeftCtrl && !_isCtrlDown )
         {
            _isCtrlDown = true;

            string acceptGlyph = (string) Application.Current.Resources["AcceptHintGlyph"];
            MainEntryBox.AnimateRadialTextTo( acceptGlyph );
         }
      }

      private void CommitWindow_OnPreviewKeyUp( object sender, KeyEventArgs e )
      {
         bool wasSystemKey = e.Key == Key.System && ( e.SystemKey == Key.LeftCtrl || e.SystemKey == Key.RightCtrl );
         bool wasNormalKey = e.Key != Key.System && ( e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl );

         if ( wasSystemKey || wasNormalKey )
         {
            RestoreCounter();
         }
      }

      private void CommitWindow_OnActivated( object sender, EventArgs e ) => RestoreCounter();

      private void RestoreCounter()
      {
         if ( _isCtrlDown )
         {
            MainEntryBox.RestoreCounter();
            _isCtrlDown = false;
         }
      }
   }
}
