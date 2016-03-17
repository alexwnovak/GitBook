using System;
using System.Collections;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using GitWrite.ViewModels;

namespace GitWrite.Views.Controls
{
   public partial class InteractiveRebasePanel
   {
      private enum MovementDirection
      {
         Up = -1,
         Down = 1
      }

      public static DependencyProperty ItemsSourceProperty = DependencyProperty.Register( "ItemsSource",
         typeof( IEnumerable ),
         typeof( InteractiveRebasePanel ),
         new FrameworkPropertyMetadata( null ) );

      public IEnumerable ItemsSource
      {
         get
         {
            return (IEnumerable) GetValue( ItemsSourceProperty );
         }
         set
         {
            SetValue( ItemsSourceProperty, value );
         }
      }

      private const int _movementAnimationDuration = 70;
      private int _highlightedIndex;
      private bool _isHighlightMoving;

      public InteractiveRebasePanel()
      {
         InitializeComponent();
      }

      private Duration AnimationDuration => new Duration( TimeSpan.FromMilliseconds( _movementAnimationDuration ) );

      private void InteractiveRebasePanel_Loaded( object sender, RoutedEventArgs e )
      {
         ListBox.Focus();
      }

      private DoubleAnimation GetDoubleAnimation( double from, double to )
         => new DoubleAnimation( from, to, AnimationDuration )
         {
            EasingFunction = new QuarticEase()
         };

      private Task MoveItemAsync( int index, MovementDirection direction )
      {
         var taskCompletionSource = new TaskCompletionSource<bool>();

         var container = (ListBoxItem) ListBox.ItemContainerGenerator.ContainerFromIndex( index );
         var child = (FrameworkElement) VisualTreeHelper.GetChild( container, 0 );

         var doubleAnimation = GetDoubleAnimation( 0, container.ActualHeight * (int) direction );

         doubleAnimation.Completed += ( sender, e ) =>
         {
            child.RenderTransform = null;
            taskCompletionSource.SetResult( true );
         };

         var translateTransform = new TranslateTransform();

         child.RenderTransform = translateTransform;
         translateTransform.BeginAnimation( TranslateTransform.YProperty, doubleAnimation );

         return taskCompletionSource.Task;
      }

      private Task MoveHighlightAsync( int newIndex )
      {
         _isHighlightMoving = true;

         var taskCompletionSource = new TaskCompletionSource<bool>();

         var container = (ListBoxItem) ListBox.ItemContainerGenerator.ContainerFromIndex( _highlightedIndex );

         var doubleAnimation = GetDoubleAnimation( _highlightedIndex * container.ActualHeight, newIndex * container.ActualHeight );

         doubleAnimation.Completed += ( sender, e ) =>
         {
            _isHighlightMoving = false;
            taskCompletionSource.SetResult( true );
         };

         HighlightElement.BeginAnimation( Canvas.TopProperty, doubleAnimation );

         return taskCompletionSource.Task;
      }

      private Task MoveHighlightAsync( MovementDirection direction )
      {
         _isHighlightMoving = true;

         var taskCompletionSource = new TaskCompletionSource<bool>();

         var container = (ListBoxItem) ListBox.ItemContainerGenerator.ContainerFromIndex( _highlightedIndex );

         double height = container.ActualHeight;
         double from = _highlightedIndex * height;
         double to = from + container.ActualHeight * (int) direction;

         var doubleAnimation = GetDoubleAnimation( from, to );
         doubleAnimation.Completed += ( sender, e ) =>
         {
            _isHighlightMoving = false;
            taskCompletionSource.SetResult( true );
         };

         HighlightElement.BeginAnimation( Canvas.TopProperty, doubleAnimation );

         return taskCompletionSource.Task;
      }

      private async Task SwapItemsAsync( int moveDownIndex, int MoveUpIndex )
      {
         var moveDownTask = MoveItemAsync( moveDownIndex, MovementDirection.Down );
         var moveUpTask = MoveItemAsync( MoveUpIndex, MovementDirection.Up );

         Task moveHighlightTask;

         if ( moveDownIndex == _highlightedIndex )
         {
            moveHighlightTask = MoveHighlightAsync( MovementDirection.Down );
         }
         else
         {
            moveHighlightTask = MoveHighlightAsync( MovementDirection.Up );
         }

         await Task.WhenAll( moveDownTask, moveUpTask, moveHighlightTask );

         var viewModel = (InteractiveRebaseViewModel) DataContext;
         viewModel.SwapItems( moveDownIndex, MoveUpIndex );
      }

      private async void InteractiveRebaseWindow_OnPreviewKeyDown( object sender, KeyEventArgs e )
      {
         if ( _isHighlightMoving )
         {
            return;
         }

         bool isCtrlDown = Keyboard.Modifiers == ModifierKeys.Control;

         if ( e.Key == Key.Down )
         {
            if ( _highlightedIndex == ListBox.Items.Count - 1 )
            {
               return;
            }

            if ( isCtrlDown )
            {
               await SwapItemsAsync( _highlightedIndex, _highlightedIndex + 1 );
            }
            else
            {
               await MoveHighlightAsync( MovementDirection.Down );
            }

            _highlightedIndex++;
         }
         else if ( e.Key == Key.Up )
         {
            if ( _highlightedIndex == 0 )
            {
               return;
            }

            if ( isCtrlDown )
            {
               await SwapItemsAsync( _highlightedIndex - 1, _highlightedIndex );
            }
            else
            {
               await MoveHighlightAsync( MovementDirection.Up );
            }

            _highlightedIndex--;
         }
         else if ( e.Key == Key.Home && isCtrlDown )
         {
            await MoveHighlightAsync( 0 );
            _highlightedIndex = 0;
         }
         else if ( e.Key == Key.End && isCtrlDown )
         {
            await MoveHighlightAsync( ListBox.Items.Count - 1 );
            _highlightedIndex = ListBox.Items.Count - 1;
         }
      }
   }
}
