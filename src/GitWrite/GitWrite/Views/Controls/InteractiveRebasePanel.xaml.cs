using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using GitWrite.ViewModels;

namespace GitWrite.Views.Controls
{
   public partial class InteractiveRebasePanel : ListBox
   {
      private enum MovementDirection
      {
         Up,
         Down
      }

      public InteractiveRebasePanel()
      {
         InitializeComponent();
      }

      private Task MoveItemAsync( int index, MovementDirection direction )
      {
         int directionMultiplier = direction == MovementDirection.Up ? -1 : 1;
         var taskCompletionSource = new TaskCompletionSource<bool>();

         var container = (ListBoxItem) ItemContainerGenerator.ContainerFromIndex( index );
         var child = (Border) VisualTreeHelper.GetChild( container, 0 );
         var doubleAnimation = new DoubleAnimation( 0, container.ActualHeight * directionMultiplier, new Duration( TimeSpan.FromMilliseconds( 100 ) ) )
         {
            EasingFunction = new CircleEase
            {
               EasingMode = EasingMode.EaseOut
            }
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

      private async Task SwapItemsAsync( int moveDownIndex, int MoveUpIndex )
      {
         var moveDownTask = MoveItemAsync( moveDownIndex, MovementDirection.Down );
         var moveUpTask = MoveItemAsync( MoveUpIndex, MovementDirection.Up );

         await Task.WhenAll( moveDownTask, moveUpTask );

         var viewModel = (InteractiveRebaseViewModel) DataContext;
         viewModel.SwapItems( moveDownIndex, MoveUpIndex );
      }

      private async void InteractiveRebaseWindow_OnPreviewKeyDown( object sender, KeyEventArgs e )
      {
         if ( e.Key == Key.Down )
         {
            if ( SelectedIndex + 1 < Items.Count )
            {
               SelectedIndex++;
            }
         }
         else if ( e.Key == Key.Up )
         {
            if ( SelectedIndex - 1 >= 0 )
            {
               SelectedIndex--;
            }
         }
      }
   }
}
