using System;
using System.Windows.Controls;

namespace GitWrite.Views.Controls
{
   public partial class InteractiveRebasePanel : ItemsControl
   {
      private ScrollViewer _scrollViewer;
      private Grid _layoutGrid;

      public InteractiveRebasePanel()
      {
         InitializeComponent();
      }

      private void InteractiveRebasePanel_OnLoaded( object sender, RoutedEventArgs e )
      {
         _scrollViewer = (ScrollViewer) Template.FindName( "ScrollViewer", this );
         _layoutGrid = (Grid) Template.FindName( "LayoutGrid", this );
      }
   }
}
