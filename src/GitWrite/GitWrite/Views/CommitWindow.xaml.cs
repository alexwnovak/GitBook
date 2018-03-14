using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Media3D;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using GitWrite.Messages;
using GitWrite.Services;
using GitWrite.ViewModels;
using Resx = GitWrite.Properties.Resources;

namespace GitWrite.Views
{
   public partial class CommitWindow : Window
   {
      private readonly CommitViewModel _viewModel;
      private readonly ISoundService _soundService = new SoundService();
      private readonly IApplicationSettings _applicationSettings = SimpleIoc.Default.GetInstance<IApplicationSettings>();
      private bool _isCtrlDown;
      private bool _isPlayingExitAnimation;

      public CommitWindow()
      {
         InitializeComponent();

         _viewModel = (CommitViewModel) DataContext;

         //MainEntryBox.MaxLength = _applicationSettings.MaxCommitLength;

         Messenger.Default.Register<ShakeRequestedMessage>( this, _ => OnAsyncShakeRequested() );
         Messenger.Default.Register<ExpansionRequestedMessage>( this, _ => OnAsyncExpansionRequested() );
         Messenger.Default.Register<CollapseRequestedMessage>( this, _ => OnAsyncCollapseRequested() );
         Messenger.Default.Register<ExitRequestedMessage>( this, m => OnAsyncExitRequested( m ) );
      }

      private void CommitWindow_OnLoaded( object sender, RoutedEventArgs e )
      {
         //MainEntryBox.HideCaret();
         //MainEntryBox.MoveCaretToEnd();
         //SecondaryTextBox.MoveCaretToEnd();
      }

      private string GetSaveText() => _viewModel.IsAmending ? Resx.AmendingText : Resx.CommittingText;

      private void OnAsyncExitRequested( ExitRequestedMessage message )
      {
         if ( _isPlayingExitAnimation )
         {
            return;
         }

         _isPlayingExitAnimation = true;

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

         var translateAnimation = new DoubleAnimation( 0, 5, opacityAnimation.Duration )
         {
            BeginTime = opacityAnimation.BeginTime
         };

         var scaleXAnimation = new DoubleAnimation( 1, 0.98, opacityAnimation.Duration )
         {
            BeginTime = opacityAnimation.BeginTime
         };

         var scaleYAnimation = new DoubleAnimation( 1, 0.98, opacityAnimation.Duration )
         {
            BeginTime = opacityAnimation.BeginTime
         };

         var storyboard = new Storyboard();

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

         storyboard.Children.Add( opacityAnimation );
         storyboard.Children.Add( blurRadiusAnimation );
         storyboard.Children.Add( translateAnimation );
         storyboard.Children.Add( scaleXAnimation );
         storyboard.Children.Add( scaleYAnimation );

         storyboard.Completed += async ( _, __ ) =>
         {
            await Task.Delay( 100 );
            message.Complete();
         };

         storyboard.Begin( this );

         _isPlayingExitAnimation = true;
      }

      private void OnAsyncExpansionRequested()
      {
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

         //Storyboard.SetTarget( heightAnimation, SecondaryBorder );
         Storyboard.SetTargetProperty( heightAnimation, new PropertyPath( nameof( Height ) ) );

         var storyboard = new Storyboard();
         storyboard.Children.Add( heightAnimation );

         Storyboard.SetTarget( storyboard, this );
         storyboard.Begin();
      }

      private void OnAsyncCollapseRequested()
      {
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

         //Storyboard.SetTarget( heightAnimation, SecondaryBorder );
         Storyboard.SetTargetProperty( heightAnimation, new PropertyPath( nameof( Height ) ) );

         var storyboard = new Storyboard();
         storyboard.Children.Add( heightAnimation );

         Storyboard.SetTarget( storyboard, this );
         storyboard.Begin();
      }

      private void OnAsyncShakeRequested()
      {
         //var subject = MainEntryBox;
         //var savedTransform = subject.RenderTransform;
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

         //subject.RenderTransform = translateTransform;

         shakeAnimation.Completed += ( _, __ ) =>
         {
            //subject.RenderTransform = savedTransform;
         };

         translateTransform.BeginAnimation( TranslateTransform.XProperty, shakeAnimation );
      }

      private void CommitWindow_OnPreviewCanExecute( object sender, CanExecuteRoutedEventArgs e )
      {
         //if ( e.Command == ApplicationCommands.Paste )
         //{
         //   e.CanExecute = false;
         //   e.Handled = true;
         //}
      }

      private void OnRadialMouseEnter( object sender, RoutedEventArgs e )
      {
         HideCounter();
      }

      private void OnRadialMouseLeave( object sender, RoutedEventArgs e )
      {
         RestoreCounter();
      }

      private void CommitWindow_OnPreviewKeyDown( object sender, KeyEventArgs e )
      {
         if ( e.Key == Key.LeftCtrl )
         {
            HideCounter();
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

      private void HideCounter()
      {
         if ( !_isCtrlDown )
         {
            _isCtrlDown = true;

            string acceptGlyph = (string) Application.Current.Resources["AcceptHintGlyph"];
            //MainEntryBox.AnimateRadialTextTo( acceptGlyph );
         }
      }

      private void RestoreCounter()
      {
         if ( _isCtrlDown )
         {
            //MainEntryBox.RestoreCounter();
            _isCtrlDown = false;
         }
      }

      private void OnRadialClick( object sender, RoutedEventArgs e )
      {
         _viewModel.SaveCommand.Execute( null );
      }
   }
}
