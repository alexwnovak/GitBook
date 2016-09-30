using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using GalaSoft.MvvmLight.Messaging;
using GitWrite.Messages;
using GitWrite.Views.Converters;

namespace GitWrite.Views.Controls
{
   public partial class RadialTextBlock : UserControl
   {
      public static DependencyProperty TextProperty = MainEntryBox.TextProperty.AddOwner( typeof( RadialTextBlock ),
         new PropertyMetadata( OnTextPropertyChanged ) );

      public static readonly RoutedEvent RadialMouseEnterEvent = EventManager.RegisterRoutedEvent( nameof( RadialMouseEnter ),
         RoutingStrategy.Bubble,
         typeof( RoutedEventHandler ),
         typeof( RadialTextBlock ) );

      public event RoutedEventHandler RadialMouseEnter
      {
         add { AddHandler( RadialMouseEnterEvent, value ); }
         remove { RemoveHandler( RadialMouseEnterEvent, value ); }
      }

      private void RaiseRadialMouseEnterEvent() => RaiseEvent( new RoutedEventArgs( RadialMouseEnterEvent ) );

      public static readonly RoutedEvent RadialMouseLeaveEvent = EventManager.RegisterRoutedEvent( nameof( RadialMouseLeave ),
         RoutingStrategy.Bubble,
         typeof( RoutedEventHandler ),
         typeof( RadialTextBlock ) );

      public event RoutedEventHandler RadialMouseLeave
      {
         add { AddHandler( RadialMouseLeaveEvent, value ); }
         remove { RemoveHandler( RadialMouseLeaveEvent, value ); }
      }

      private void RaiseRadialMouseLeaveEvent() => RaiseEvent( new RoutedEventArgs( RadialMouseLeaveEvent ) );

      public static readonly RoutedEvent RadialClickEvent = EventManager.RegisterRoutedEvent( nameof( RadialClick ),
         RoutingStrategy.Bubble,
         typeof( RoutedEventHandler ),
         typeof( RadialTextBlock ) );

      public event RoutedEventHandler RadialClick
      {
         add { AddHandler( RadialClickEvent, value ); }
         remove { RemoveHandler( RadialClickEvent, value ); }
      }

      private void RaiseRadialClickEvent() => RaiseEvent( new RoutedEventArgs( RadialClickEvent ) );

      public string Text
      {
         get
         {
            return (string) GetValue( TextProperty );
         }
         set
         {
            SetValue( TextProperty, value );
         }
      }

      private static void OnTextPropertyChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
      {
         var lengthProgressConverter = new StringLengthToProgressConverter();

         var radialTextBlock = (RadialTextBlock) d;
         radialTextBlock.Progress = 1 - (double) lengthProgressConverter.Convert( e.NewValue, null, null, null );

         if ( radialTextBlock.Progress >= 1.0 )
         {
            radialTextBlock.ProgressRing.Stroke = (Brush) Application.Current.Resources["MessageLengthReachedColor"];
         }
         else
         {
            radialTextBlock.ProgressRing.Stroke = (Brush) Application.Current.Resources["WindowBorderColor"];
         }

         PulseRing( radialTextBlock.ProgressRing );
      }

      private static void PulseRing( UIElement element )
      {
         var doubleAnimation = new DoubleAnimation
         {
            From = 3,
            To = 1,
            Duration = new Duration( TimeSpan.FromMilliseconds( 800 ) ),
            EasingFunction = new CircleEase
            {
               EasingMode = EasingMode.EaseIn
            }
         };

         element.BeginAnimation( Shape.StrokeThicknessProperty, doubleAnimation );
      }

      public static DependencyProperty IsProgressRingVisibleProperty = DependencyProperty.Register( nameof( IsProgressRingVisible ),
         typeof( bool ),
         typeof( RadialTextBlock ),
         new PropertyMetadata( true ) );

      public bool IsProgressRingVisible
      {
         get
         {
            return (bool) GetValue( IsProgressRingVisibleProperty );
         }
         set
         {
            SetValue( IsProgressRingVisibleProperty, value );
         }
      }

      public static DependencyProperty ProgressProperty = DependencyProperty.Register( nameof( Progress ),
         typeof( double ),
         typeof( RadialTextBlock ),
         new PropertyMetadata( 0.0 ) );

      public double Progress
      {
         get
         {
            return (double) GetValue( ProgressProperty );
         }
         set
         {
            SetValue( ProgressProperty, value );
         }
      }

      public RadialTextBlock()
      {
         InitializeComponent();
         Messenger.Default.Register<PulseRequestedMessage>( this, m => PulseRing( ProgressRing ) );
      }

      public void AnimateTextTo( string text )
      {
         const double duration = 300;

         FadeOutCounter( duration );
         FadeInSecondaryText( text, duration );
      }

      public void RestoreCounter()
      {
         const double duration = 300;

         FadeInCounter( duration );
         FadeOutSecondaryText( duration );
      }

      private void FadeOutCounter( double duration )
      {
         var translateTransform = new TranslateTransform();

         TextBlock.RenderTransformOrigin = new Point( 0.5, 0.5 );
         TextBlock.RenderTransform = translateTransform;

         var offsetAnimation = new DoubleAnimation
         {
            To = -16,
            Duration = new Duration( TimeSpan.FromMilliseconds( duration ) ),
            EasingFunction = new CircleEase
            {
               EasingMode = EasingMode.EaseOut
            }
         };

         var opacityAnimation = new DoubleAnimation
         {
            To = 0,
            Duration = new Duration( TimeSpan.FromMilliseconds( duration ) )
         };

         translateTransform.BeginAnimation( TranslateTransform.YProperty, offsetAnimation );
         TextBlock.BeginAnimation( OpacityProperty, opacityAnimation );
      }

      private void FadeInCounter( double duration )
      {
         var offsetAnimation = new DoubleAnimation
         {
            To = 0,
            Duration = new Duration( TimeSpan.FromMilliseconds( duration ) ),
            EasingFunction = new CircleEase
            {
               EasingMode = EasingMode.EaseOut
            }
         };

         var opacityAnimation = new DoubleAnimation
         {
            To = 1,
            Duration = new Duration( TimeSpan.FromMilliseconds( duration ) )
         };

         TextBlock.RenderTransform.BeginAnimation( TranslateTransform.YProperty, offsetAnimation );
         TextBlock.BeginAnimation( OpacityProperty, opacityAnimation );
      }

      private void FadeInSecondaryText( string text, double duration )
      {
         var translateTransform = new TranslateTransform();

         SecondaryTextBlock.RenderTransformOrigin = new Point( 0.5, 0.5 );
         SecondaryTextBlock.RenderTransform = translateTransform;
         SecondaryTextBlock.Text = text;

         var offsetAnimation = new DoubleAnimation
         {
            From = 16,
            To = 0,
            Duration = new Duration( TimeSpan.FromMilliseconds( duration ) ),
            EasingFunction = new CircleEase
            {
               EasingMode = EasingMode.EaseOut
            }
         };

         var opacityAnimation = new DoubleAnimation
         {
            From = 0,
            To = 1,
            Duration = new Duration( TimeSpan.FromMilliseconds( duration ) )
         };

         translateTransform.BeginAnimation( TranslateTransform.YProperty, offsetAnimation );
         SecondaryTextBlock.BeginAnimation( OpacityProperty, opacityAnimation );
      }

      private void FadeOutSecondaryText( double duration )
      {
         var offsetAnimation = new DoubleAnimation
         {
            To = 16,
            Duration = new Duration( TimeSpan.FromMilliseconds( duration ) ),
            EasingFunction = new CircleEase
            {
               EasingMode = EasingMode.EaseOut
            }
         };

         var opacityAnimation = new DoubleAnimation
         {
            To = 0,
            Duration = new Duration( TimeSpan.FromMilliseconds( duration ) )
         };

         SecondaryTextBlock.RenderTransform.BeginAnimation( TranslateTransform.YProperty, offsetAnimation );
         SecondaryTextBlock.BeginAnimation( OpacityProperty, opacityAnimation );
      }

      private void OnMouseEnter( object sender, MouseEventArgs e ) => RaiseRadialMouseEnterEvent();
      private void OnMouseLeave( object sender, MouseEventArgs e ) => RaiseRadialMouseLeaveEvent();

      private void OnMouseDown( object sender, MouseEventArgs e )
      {
         if ( e.LeftButton == MouseButtonState.Pressed )
         {
            RaiseRadialClickEvent();
         }
      }
   }
}
