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

namespace GitWrite.Views.Controls
{
   public partial class InteractiveRebasePanel : ItemsControl
   {
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

      private void InteractiveRebasePanel_OnLoaded( object sender, RoutedEventArgs e )
      {
         _scrollViewer = (ScrollViewer) Template.FindName( "ScrollViewer", this );
         _layoutGrid = (Grid) Template.FindName( "LayoutGrid", this );
      }

      private static void ItemsSourceChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
      {
         var panel = (InteractiveRebasePanel) d;
         panel._itemCollection = (ICollection) e.NewValue;
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

               var margin = new Thickness( 0, offset, 0, 0 );
               var marginAfter = new Thickness( 0, offset + 28 * direction, 0, 0 );

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

               var thicknessAnimation = new ThicknessAnimation( margin, marginAfter, new Duration( TimeSpan.FromMilliseconds( 70 ) ) )
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
               storyboard.Completed += ( sender, args ) => tcs.SetResult( true );
               storyboard.Begin();

               await tcs.Task;

               _layoutGrid.Children.Remove( animatedRectangle );
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
   }
}
