using System;
using System.Collections;
using System.Windows.Controls;

namespace GitWrite.Views.Controls
{
   public partial class InteractiveRebasePanel : ItemsControl
   {
      private ScrollViewer _scrollViewer;
      private Grid _layoutGrid;
      private ICollection _itemCollection;

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
   }
}
