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
         Up,
         Down
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

      public InteractiveRebasePanel()
      {
         InitializeComponent();
      }

      private void InteractiveRebasePanel_Loaded( object sender, RoutedEventArgs e )
      {
         ListBox.Focus();
      }

      private Task MoveItemAsync( int index, MovementDirection direction )
      {
         int directionMultiplier = direction == MovementDirection.Up ? -1 : 1;
         var taskCompletionSource = new TaskCompletionSource<bool>();

         var container = (ListBoxItem) ListBox.ItemContainerGenerator.ContainerFromIndex( index );
         var child = (FrameworkElement) VisualTreeHelper.GetChild( container, 0 );
         var doubleAnimation = new DoubleAnimation( 0, container.ActualHeight * directionMultiplier, new Duration( TimeSpan.FromMilliseconds( 70 ) ) )
         {
            EasingFunction = new QuarticEase()
         };
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

      private Task MoveHighlightAsync( MovementDirection direction )
      {
         _isHighlightMoving = true;

         int directionMultiplier = direction == MovementDirection.Up ? -1 : 1;
         var taskCompletionSource = new TaskCompletionSource<bool>();

         var container = (ListBoxItem) ListBox.ItemContainerGenerator.ContainerFromIndex( _highlightedIndex );

         double height = container.ActualHeight;
         double y = _highlightedIndex * height;

         var doubleAnimation = new DoubleAnimation( y, y + container.ActualHeight * directionMultiplier, new Duration( TimeSpan.FromMilliseconds( 70 ) ) )
         {
            EasingFunction = new QuarticEase()
         };
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

      private void InteractiveRebaseWindow_OnPreviewKeyDown( object sender, KeyEventArgs e )
      {
      }
   }
}
