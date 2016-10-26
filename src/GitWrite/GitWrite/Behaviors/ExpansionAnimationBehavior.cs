using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using GalaSoft.MvvmLight.Ioc;
using GitWrite.Animation;
using GitWrite.ViewModels;
using GitWrite.Views;

namespace GitWrite.Behaviors
{
   public class ExpansionAnimationBehavior : Behavior<CommitWindow>
   {
      private CommitViewModel _viewModel;

      protected override void OnAttached()
      {
         AssociatedObject.Loaded += ( _, __ ) =>
         {
            _viewModel = SimpleIoc.Default.GetInstance<CommitViewModel>();

            AssociatedObject.MainEntryBox.MouseEnter += MainEntryBoxOnMouseEnter;
            AssociatedObject.MouseLeave += CommitWindowOnMouseLeave;
         };
      }

      private async void AnimateDrawer( double from, double to )
      {
         if ( _viewModel.IsExpanded )
         {
            return;
         }

         AssociatedObject.SecondaryBorder.Animate()
            .Height()
            .From( from )
            .To( to )
            .For( 100 )
            .Begin();

         await Task.Delay( 200 );

         //AssociatedObject.SecondaryBorder.Animate()
         //   .Opacity()
         //   .From( 1 )
         //   .To( 0 )
         //   .For( 500 )
         //   .Begin();

         AssociatedObject.SecondaryBorder.Animate()
            .Background()
            .From( Colors.Black )
            .To( Colors.White )
            .For( 500 )
            .Begin();
      }

      private void MainEntryBoxOnMouseEnter( object sender, MouseEventArgs e )
         => AnimateDrawer( 0, 24 );

      private void CommitWindowOnMouseLeave( object sender, MouseEventArgs e )
         => AnimateDrawer( 24, 0 );
   }
}
