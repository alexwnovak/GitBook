using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using GitWrite.Views.Converters;

namespace GitWrite.Views.Controls
{
   public partial class RadialTextBlock : UserControl
   {
      public static DependencyProperty TextProperty = MainEntryBox.TextProperty.AddOwner( typeof( RadialTextBlock ),
         new PropertyMetadata( OnTextPropertyChanged ) );

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
            PulseRing( radialTextBlock.ProgressRing );
         }
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
      }
   }
}
