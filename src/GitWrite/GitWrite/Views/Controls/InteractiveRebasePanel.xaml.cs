using System;
using System.Collections;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using GitWrite.ViewModels;

namespace GitWrite.Views.Controls
{
   public partial class InteractiveRebasePanel : ItemsControl
   {
      private enum MovementDirection
      {
         Up = -1,
         Down = 1
      }

      private ScrollViewer _scrollViewer;
      private Grid _layoutGrid;
      private ICollection _itemCollection;
      private int _selectedIndex;
      private FrameworkElement _selectedObject;
      private ItemSelectionAdorner _currentAdorner;
      private bool _isMoving;
      private double _previousY;

      static InteractiveRebasePanel()
      {
         ItemsSourceProperty.OverrideMetadata( typeof( InteractiveRebasePanel ), new FrameworkPropertyMetadata( ItemsSourceChanged ) );
      }

      public InteractiveRebasePanel()
      {
         InitializeComponent();
      }

      private async void InteractiveRebasePanel_OnLoaded( object sender, RoutedEventArgs e )
      {
         _scrollViewer = (ScrollViewer) Template.FindName( "ScrollViewer", this );
         _layoutGrid = (Grid) Template.FindName( "LayoutGrid", this );

         await UpdateSelectedIndex( 0 );
      }

      private static void ItemsSourceChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
      {
         var panel = (InteractiveRebasePanel) d;
         panel._itemCollection = (ICollection) e.NewValue;
      }

      private DoubleAnimation GetDoubleAnimation( double from, double to, TimeSpan duration )
          => new DoubleAnimation( from, to, new Duration( duration ) )
          {
             EasingFunction = new QuarticEase()
          };

      private Task MoveItemAsync( int index, MovementDirection direction )
      {
         var taskCompletionSource = new TaskCompletionSource<bool>();

         var container = (ContentPresenter) ItemContainerGenerator.ContainerFromIndex( index );
         var child = (FrameworkElement) VisualTreeHelper.GetChild( container, 0 );

         var doubleAnimation = GetDoubleAnimation( 0, container.ActualHeight * (int) direction, TimeSpan.FromMilliseconds( 200 ) );

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

      private async Task SwapItemsAsync( int moveDownIndex, int moveUpIndex )
      {
         var moveDownTask = MoveItemAsync( moveDownIndex, MovementDirection.Down );
         var moveUpTask = MoveItemAsync( moveUpIndex, MovementDirection.Up );

         var pos = _selectedObject.TranslatePoint( new Point( 0, 0 ), _scrollViewer );
         Task moveHighlightTask;
         int nextIndex;

         if ( moveDownIndex == _selectedIndex )
         {
            nextIndex = _selectedIndex + 1;
            moveHighlightTask = AnimateHighlight( pos.Y, pos.Y + 28, TimeSpan.FromMilliseconds( 70 ) );
         }
         else
         {
            nextIndex = _selectedIndex - 1;
            moveHighlightTask = AnimateHighlight( pos.Y, pos.Y - 28, TimeSpan.FromMilliseconds( 70 ) );
         }

         RemoveCurrentAdorner();

         await Task.WhenAll( moveDownTask, moveUpTask, moveHighlightTask );
 
         var viewModel = (InteractiveRebaseViewModel) DataContext;
         viewModel.SwapItems( moveDownIndex, moveUpIndex );

         SetAdorner( nextIndex );
      }

      private Task AnimateHighlight( double from, double to, TimeSpan duration )
      {
         var margin = new Thickness( 0, from, 0, 0 );
         var marginAfter = new Thickness( 0, to, 0, 0 );

         var animatedRectangle = new Rectangle
         {
            IsHitTestVisible = false,
            VerticalAlignment = VerticalAlignment.Top,
            Width = ActualWidth,
            Margin = margin,
            Height = 28,
            Fill = (Brush) Application.Current.Resources["HighlightColor"]
         };

         _layoutGrid.Children.Add( animatedRectangle );

         var thicknessAnimation = new ThicknessAnimation( margin, marginAfter, new Duration( duration ) )
         {
            EasingFunction = new CircleEase
            {
               EasingMode = EasingMode.EaseOut
            }
         };

         var storyboard = new Storyboard();

         Storyboard.SetTarget( thicknessAnimation, animatedRectangle );
         Storyboard.SetTargetProperty( thicknessAnimation, new PropertyPath( MarginProperty ) );

         var tcs = new TaskCompletionSource<bool>();

         storyboard.Children.Add( thicknessAnimation );
         storyboard.Completed += ( sender, args ) =>
         {
            _layoutGrid.Children.Remove( animatedRectangle );
            tcs.SetResult( true );
         };
         storyboard.Begin();

         return tcs.Task;
      }

      private async Task UpdateSelectedIndex( int index )
      {
         if ( _isMoving )
         {
            return;
         }

         _isMoving = true;
         RemoveCurrentAdorner();

         if ( _selectedObject != null )
         {
            int direction = index > _selectedIndex ? 1 : -1;
            var pos = _selectedObject.TranslatePoint( new Point( 0, 0 ), _scrollViewer );
            double offset = pos.Y;

            if ( offset != _previousY )
            {
               _previousY = offset;
               offset += _scrollViewer.VerticalOffset;

               await AnimateHighlight( offset, offset + 28 * direction, TimeSpan.FromMilliseconds( 200 ) );
            }
         }

         SetAdorner( index );
         _isMoving = false;
      }

      private void RemoveCurrentAdorner()
      {
         if ( _selectedObject == null || _currentAdorner == null )
         {
            return;
         }

         var adornerLayer = AdornerLayer.GetAdornerLayer( _selectedObject );
         adornerLayer.Remove( _currentAdorner );
      }

      private void SetAdorner( int index )
      {
         var currentObject = (ContentPresenter) ItemContainerGenerator.ContainerFromIndex( index );

         var adornerLayer = AdornerLayer.GetAdornerLayer( currentObject );

         _currentAdorner = new ItemSelectionAdorner( currentObject );

         adornerLayer.Add( _currentAdorner );

         _selectedObject = currentObject;
         _selectedIndex = index;

         _selectedObject.BringIntoView();
      }

      private async void InteractiveRebasePanel_OnKeyDown( object sender, KeyEventArgs e )
      {
         if ( e.Key == Key.Down )
         {
            if ( _selectedIndex >= _itemCollection.Count - 1 )
            {
               return;
            }

            if ( Keyboard.Modifiers == ModifierKeys.Control )
            {
               await SwapItemsAsync( _selectedIndex, _selectedIndex + 1 );
            }
            else
            {
               await UpdateSelectedIndex( _selectedIndex + 1 );
            }
         }
         else if ( e.Key == Key.Up )
         {
            if ( _selectedIndex <= 0 )
            {
               return;
            }

            if ( Keyboard.Modifiers == ModifierKeys.Control )
            {
               await SwapItemsAsync( _selectedIndex - 1, _selectedIndex );
            }
            else
            {
               await UpdateSelectedIndex( _selectedIndex - 1 );
            }
         }
      }
   }
}
