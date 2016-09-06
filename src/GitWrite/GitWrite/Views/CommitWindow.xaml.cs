﻿using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using GitWrite.ViewModels;

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

      private Task OnAsyncExitRequested( object sender, ShutdownEventArgs e )
      {
         if ( _viewModel.IsExiting )
         {
            return Task.CompletedTask;
         }

         var frontMaterial = (ImageSource) Resources["FrontMaterial"];
         FrontMaterial.Brush = new ImageBrush( frontMaterial )
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

         animation.Completed += ( _, __ ) =>
         {
            Thread.Sleep( 500 );
            tcs.SetResult( true );
         };

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

      private Task OnExpansionRequested( object sender, EventArgs eventArgs )
      {
         var tcs = new TaskCompletionSource<bool>();

         var heightAnimation = new DoubleAnimation
         {
            To = 400,
            Duration = new Duration( TimeSpan.FromMilliseconds( 100 ) ),
            EasingFunction = new CircleEase
            {
               EasingMode = EasingMode.EaseOut
            }
         };

         var opacityAnimation = new DoubleAnimation
         {
            To = 1,
            Duration = new Duration( TimeSpan.FromMilliseconds( 100 ) )
         };

         Storyboard.SetTarget( heightAnimation, PageRoot );
         Storyboard.SetTargetProperty( heightAnimation, new PropertyPath( nameof( Height ) ) );

         Storyboard.SetTarget( opacityAnimation, SecondaryBorder );
         Storyboard.SetTargetProperty( opacityAnimation, new PropertyPath( nameof( Opacity ) ) );

         var storyboard = new Storyboard();
         storyboard.Children.Add( heightAnimation );
         storyboard.Children.Add( opacityAnimation );
         storyboard.Completed += ( _, __ ) => tcs.SetResult( true );

         Storyboard.SetTarget( storyboard, this );
         storyboard.Begin();

         return tcs.Task;
      }

      private Task OnCollapseRequested( object sender, EventArgs eventArgs )
      {
         var tcs = new TaskCompletionSource<bool>();

         var heightAnimation = new DoubleAnimation
         {
            To = 100,
            Duration = new Duration( TimeSpan.FromMilliseconds( 100 ) ),
            EasingFunction = new CircleEase
            {
               EasingMode = EasingMode.EaseOut
            }
         };

         var opacityAnimation = new DoubleAnimation
         {
            To = 0,
            Duration = new Duration( TimeSpan.FromMilliseconds( 100 ) )
         };

         Storyboard.SetTarget( heightAnimation, PageRoot );
         Storyboard.SetTargetProperty( heightAnimation, new PropertyPath( nameof( Height ) ) );

         Storyboard.SetTarget( opacityAnimation, SecondaryBorder );
         Storyboard.SetTargetProperty( opacityAnimation, new PropertyPath( nameof( Opacity ) ) );

         var storyboard = new Storyboard();
         storyboard.Children.Add( heightAnimation );
         storyboard.Children.Add( opacityAnimation );
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
