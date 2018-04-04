using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using GitWrite.Services;
using Resx = GitWrite.Properties.Resources;

namespace GitWrite.Views.Controls
{
   public partial class StyledDialog : UserControl
   {
      private DialogResult _dialogResult;
      private Window _modalWindow;
      private Button _defaultFocusedButton;
      private bool _hasPlayedExitAnimation;

      public StyledDialog()
      {
         InitializeComponent();
      }

      public DialogResult ShowDialog( string title, string message, DialogButtons buttons )
         => ShowDialog( title, message, buttons, w => { } );

      public DialogResult ShowDialog( string title, string message, DialogButtons buttons, Action<Window> preShowCallback )
      {
         TitleTextBlock.Text = title;
         MessageTextBlock.Text = message;

         SetupButtons( buttons );

         var mainWindow = Application.Current.MainWindow;

         _modalWindow = new Window
         {
            AllowsTransparency = true,
            Background = Brushes.Transparent,
            Content = this,
            Height = 230,
            Owner = mainWindow,
            ShowInTaskbar = false,
            Width = 400,
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            WindowStyle = WindowStyle.None
         };

         preShowCallback( _modalWindow );

         _modalWindow.Loaded += ( sender, e ) => FocusManager.SetFocusedElement( _defaultFocusedButton, _defaultFocusedButton );
         _modalWindow.KeyDown += ModalWindowKeyDown;
         _modalWindow.Closing += ModalWindowClosing;
         _modalWindow.ShowDialog();

         return _dialogResult;
      }

      private void ModalWindowClosing( object sender, CancelEventArgs e )
      {
         if ( _hasPlayedExitAnimation )
         {
            return;
         }

         e.Cancel = true;
         _hasPlayedExitAnimation = true;

         var exitStoryboard = (Storyboard) Resources["ExitStoryboard"];
         exitStoryboard.Completed += ( _, __ ) => _modalWindow.Close();
         exitStoryboard.Begin();
      }

      private void DialogHeader_OnMouseDown( object sender, MouseButtonEventArgs e )
      {
         if ( e.LeftButton == MouseButtonState.Pressed )
         {
            _modalWindow.DragMove();
         }
      }

      private void SetupButtons( DialogButtons buttons )
      {
         switch ( buttons )
         {
            case DialogButtons.OK:
            {
               CreateButton( Resx.OKAcceleratorText, DialogResult.OK, true );
               break;
            }
            case DialogButtons.YesNo:
            {
               CreateButton( Resx.YesAcceleratorText, DialogResult.Yes, true );
               CreateButton( Resx.NoAcceleratorText, DialogResult.No );
               break;
            }
            case DialogButtons.YesNoCancel:
            {
               CreateButton( Resx.YesAcceleratorText, DialogResult.Yes, true );
               CreateButton( Resx.NoAcceleratorText, DialogResult.No );
               CreateButton( Resx.CancelAcceleratorText, DialogResult.Cancel );
               break;
            }
            case DialogButtons.SaveDiscardCancel:
            {
               CreateButton( Resx.SaveAcceleratorText, DialogResult.Save, true );
               CreateButton( Resx.DiscardAcceleratorText, DialogResult.Discard );
               CreateButton( Resx.CancelAcceleratorText, DialogResult.Cancel );
               break;
            }
         }
      }

      private void CreateButton( string text, DialogResult dialogResult, bool hasFocus = false )
      {
         var button = new Button
         {
            Height = 30,
            HorizontalAlignment = HorizontalAlignment.Right,
            Margin = new Thickness( 0, 0, 6, 0 ),
            Tag = dialogResult,
            VerticalAlignment = VerticalAlignment.Bottom,
            Width = 80,
         };

         button.Click += Button_OnClick;

         var canvas = new Canvas
         {
            Width = button.Width,
            Height = button.Height,
            ClipToBounds = true
         };

         var accessGrid = new Grid
         {
            Width = button.Width - 2,
            Height = button.Height - 2,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
         };

         var accessText = new AccessText
         {
            Foreground = Brushes.White,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Text = text,
         };

         var ellipse = new Ellipse
         {
            Width = 0,
            Height = 0,
            Fill = new SolidColorBrush( Color.FromArgb( 64, 255, 255, 255 ) ),
         };

         accessGrid.Children.Add( accessText );
         canvas.Children.Add( accessGrid );
         canvas.Children.Add( ellipse );

         button.Content = canvas;

         ButtonPanel.Children.Add( button );

         if ( hasFocus )
         {
            _defaultFocusedButton = button;
         }
      }

      private void Button_OnClick( object sender, EventArgs e )
      {
         var button = (Button) sender;
         Point deltaOffset;

         if ( button.IsMouseOver )
         {
            deltaOffset = Mouse.GetPosition( button );
         }
         else
         {
            deltaOffset = new Point( button.Width / 2, button.Height / 2 );
         }

         AnimateButton( button, deltaOffset );

         _dialogResult = (DialogResult) button.Tag;
      }

      private void AnimateButton( Button button, Point deltaOffset )
      {

         var canvas = (Canvas) button.Content;
         var ellipse = (Ellipse) canvas.Children[1];

         Canvas.SetLeft( ellipse, deltaOffset.X );
         Canvas.SetTop( ellipse, deltaOffset.Y );

         var storyboard = new Storyboard();

         double finalSize = button.ActualWidth * 2;

         var widthAnimation = new DoubleAnimation( 0, finalSize, new Duration( TimeSpan.FromMilliseconds( 250 ) ) );
         var heightAnimation = new DoubleAnimation( 0, finalSize, widthAnimation.Duration );
         var opacityAnimation = new DoubleAnimation( 1, 0, new Duration( TimeSpan.FromMilliseconds( 200 ) ) )
         {
            BeginTime = TimeSpan.FromMilliseconds( 80 )
         };
         var leftAnimation = new DoubleAnimation
         {
            From = deltaOffset.X,
            To = deltaOffset.X - finalSize / 2,
            Duration = widthAnimation.Duration
         };
         var topAnimation = new DoubleAnimation
         {
            From = deltaOffset.Y,
            To = deltaOffset.Y - finalSize / 2,
            Duration = widthAnimation.Duration
         };

         Storyboard.SetTarget( widthAnimation, ellipse );
         Storyboard.SetTargetProperty( widthAnimation, new PropertyPath( WidthProperty ) );

         Storyboard.SetTarget( heightAnimation, ellipse );
         Storyboard.SetTargetProperty( heightAnimation, new PropertyPath( HeightProperty ) );

         Storyboard.SetTarget( opacityAnimation, ellipse );
         Storyboard.SetTargetProperty( opacityAnimation, new PropertyPath( OpacityProperty ) );

         Storyboard.SetTarget( leftAnimation, ellipse );
         Storyboard.SetTargetProperty( leftAnimation, new PropertyPath( Canvas.LeftProperty ) );

         Storyboard.SetTarget( topAnimation, ellipse );
         Storyboard.SetTargetProperty( topAnimation, new PropertyPath( Canvas.TopProperty ) );

         storyboard.Children.Add( widthAnimation );
         storyboard.Children.Add( heightAnimation );
         storyboard.Children.Add( opacityAnimation );
         storyboard.Children.Add( leftAnimation );
         storyboard.Children.Add( topAnimation );

         storyboard.Completed += ( _, __ ) => _modalWindow.Close();
         storyboard.Begin();
      }

      private void ModalWindowKeyDown( object sender, KeyEventArgs e )
      {
         if ( e.Key == Key.Escape )
         {
            _dialogResult = DialogResult.Cancel;
            _modalWindow.Close();
         }
      }
   }
}
