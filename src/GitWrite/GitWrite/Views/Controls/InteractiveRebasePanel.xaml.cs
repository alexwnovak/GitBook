using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;

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
