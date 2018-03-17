using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media.Animation;
using GalaSoft.MvvmLight.Ioc;
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

            //AssociatedObject.MainEntryBox.MouseEnter += MainEntryBoxOnMouseEnter;
            AssociatedObject.MouseLeave += CommitWindowOnMouseLeave;
         };
      }

      private void AnimateDrawer( double from, double to )
      {
         if ( _viewModel.IsExpanded /*|| _viewModel.IsExiting*/ )
         {
            return;
         }

         var animation = new DoubleAnimation
         {
            From = from,
            To = to,
            Duration = new Duration( TimeSpan.FromMilliseconds( 100 ) )
         };

         //AssociatedObject.SecondaryBorder.BeginAnimation( FrameworkElement.HeightProperty, animation );
      }

      private void MainEntryBoxOnMouseEnter( object sender, MouseEventArgs e )
         => AnimateDrawer( 0, 24 );

      private void CommitWindowOnMouseLeave( object sender, MouseEventArgs e )
         => AnimateDrawer( 24, 0 );
   }
}
