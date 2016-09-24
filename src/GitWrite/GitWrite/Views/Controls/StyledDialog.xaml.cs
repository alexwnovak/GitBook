﻿using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Resx = GitWrite.Properties.Resources;

namespace GitWrite.Views.Controls
{
   public partial class StyledDialog : UserControl
   {
      public StyledDialog()
      {
         InitializeComponent();
      }

      private void SetupButtons( DialogButtons buttons )
      {
         switch ( buttons )
         {
            case DialogButtons.OK:
            {
               CreateButton( Resx.OKText );
               break;
            }
            case DialogButtons.YesNo:
            {
               CreateButton( Resx.YesText );
               CreateButton( Resx.NoText );
               break;
            }
            case DialogButtons.YesNoCancel:
            {
               CreateButton( Resx.YesText );
               CreateButton( Resx.NoText );
               CreateButton( Resx.CancelText );
               break;
            }
            case DialogButtons.SaveDiscardCancel:
            {
               CreateButton( Resx.SaveText );
               CreateButton( Resx.DiscardText );
               CreateButton( Resx.CancelText );
               break;
            }
         }
      }

      private void CreateButton( string text )
      {
         var button = new Button
         {
            Height = 30,
            HorizontalAlignment = HorizontalAlignment.Right,
            Margin = new Thickness( 0, 0, 6, 0 ),
            VerticalAlignment = VerticalAlignment.Bottom,
            Width = 80,
         };

         button.Click += Button_OnClick;

         var grid = new Grid
         {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch
         };

         var accessText = new AccessText
         {
            Foreground = Brushes.White,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Text = text
         };

         grid.Children.Add( accessText );

         button.Content = grid;

         ButtonPanel.Children.Add( button );
      }

      private void Button_OnClick( object sender, EventArgs e )
      {
         var button = (Button) sender;

         var ellipse = new Ellipse
         {
            Width = 0,
            Height = 0,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Fill = new SolidColorBrush( Color.FromArgb( 64, 255, 255, 255 ) )
         };

         var grid = (Grid) button.Content;
         grid.Children.Add( ellipse );

         var storyboard = new Storyboard();

         var widthAnimation = new DoubleAnimation( 0, button.ActualWidth * 2, new Duration( TimeSpan.FromMilliseconds( 250 ) ) );
         var heightAnimation = new DoubleAnimation( 0, button.ActualWidth * 2, widthAnimation.Duration );
         var opacityAnimation = new DoubleAnimation( 1, 0, new Duration( TimeSpan.FromMilliseconds( 200 ) ) )
         {
            BeginTime = TimeSpan.FromMilliseconds( 80 )
         };

         Storyboard.SetTarget( widthAnimation, ellipse );
         Storyboard.SetTargetProperty( widthAnimation, new PropertyPath( WidthProperty ) );

         Storyboard.SetTarget( heightAnimation, ellipse );
         Storyboard.SetTargetProperty( heightAnimation, new PropertyPath( HeightProperty ) );

         Storyboard.SetTarget( opacityAnimation, ellipse );
         Storyboard.SetTargetProperty( opacityAnimation, new PropertyPath( OpacityProperty ) );

         storyboard.Children.Add( widthAnimation );
         storyboard.Children.Add( heightAnimation );
         storyboard.Children.Add( opacityAnimation );

         storyboard.Begin();
      }
   }
}
