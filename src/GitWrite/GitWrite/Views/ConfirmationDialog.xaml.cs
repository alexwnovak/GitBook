using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using GitWrite.ViewModels;

namespace GitWrite.Views
{
   public partial class ConfirmationDialog : Window
   {
      private readonly ConfirmationViewModel _viewModel;
      private ExitReason _confirmationResult;
      private bool _hasPlayedExitAnimation;
      private Task _buttonAnimationTask;

      public ConfirmationDialog( Window owner )
      {
         InitializeComponent();
         Owner = owner;

         _viewModel = (ConfirmationViewModel) DataContext;
         _viewModel.CloseRequested += OnCloseRequested;
      }

      public new ExitReason ShowDialog()
      {
         base.ShowDialog();

         return _confirmationResult;
      }

      private void OnCloseRequested( object sender, CloseRequestedEventArgs e )
      {
         _confirmationResult = e.ConfirmationResult;

         Close();
      }

      private void DialogHeader_OnMouseDown( object sender, MouseButtonEventArgs e )
      {
         if ( e.LeftButton == MouseButtonState.Pressed )
         {
            DragMove();
         }
      }

      private void ConfirmationDialog_OnKeyDown( object sender, KeyEventArgs e )
      {
         if ( e.Key == Key.Escape )
         {
            OnCloseRequested( this, new CloseRequestedEventArgs( ExitReason.Cancel ) );
         }
      }

      private async void ConfirmationDialog_OnClosing( object sender, CancelEventArgs e )
      {
         if ( _hasPlayedExitAnimation )
         {
            return;
         }

         _hasPlayedExitAnimation = true;
         e.Cancel = true;

         await _buttonAnimationTask;

         var exitStoryboard = (Storyboard) Resources["ExitStoryboard"];

         exitStoryboard.Completed += ( _, __ ) => Close();
         exitStoryboard.Begin();
      }

      private void Button_OnClick( object sender, RoutedEventArgs e )
      {
         var button = (Button) sender;

         var ellipse = new Ellipse
         {
            Width = 0,
            Height = 0,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Fill = new SolidColorBrush( Color.FromArgb( 64, 255, 255, 255 ) )
         };

         var grid = (Grid) button.Content;
         grid.Children.Add( ellipse );

         var storyboard = new Storyboard();

         var widthAnimation = new DoubleAnimation( 0, button.ActualWidth * 2, new Duration( TimeSpan.FromMilliseconds( 250 ) ) );
         var heightAnimation = new DoubleAnimation( 0, button.ActualWidth * 2, widthAnimation.Duration );
         var opacityAnimation = new DoubleAnimation( 1, 0, new Duration( TimeSpan.FromMilliseconds( 200 ) ) )
         {
            BeginTime = TimeSpan.FromMilliseconds( 100 )
         };

         Storyboard.SetTarget( widthAnimation, ellipse );
         Storyboard.SetTargetProperty( widthAnimation, new PropertyPath( WidthProperty ) );

         Storyboard.SetTarget( heightAnimation, ellipse );
         Storyboard.SetTargetProperty( heightAnimation, new PropertyPath( HeightProperty ) );

         Storyboard.SetTarget( opacityAnimation, ellipse );
         Storyboard.SetTargetProperty( opacityAnimation, new PropertyPath( OpacityProperty ) );

         storyboard.Children.Add( widthAnimation );
         storyboard.Children.Add( heightAnimation );
         storyboard.Children.Add( opacityAnimation );

         var tcs = new TaskCompletionSource<bool>();
         storyboard.Completed += ( _, __ ) => tcs.SetResult( true );

         _buttonAnimationTask = tcs.Task;

         storyboard.Begin();
      }
   }
}
