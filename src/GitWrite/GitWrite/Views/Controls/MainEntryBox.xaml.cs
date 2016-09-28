using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using GalaSoft.MvvmLight.Ioc;
using GitWrite.Services;

namespace GitWrite.Views.Controls
{
   public partial class MainEntryBox : UserControl
   {
      public static DependencyProperty TextProperty = DependencyProperty.Register( nameof( Text ),
         typeof( string ),
         typeof( MainEntryBox ),
         new FrameworkPropertyMetadata( string.Empty,
            FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
            null,
            null,
            false,
            UpdateSourceTrigger.PropertyChanged ) );

      public string Text
      {
         get
         {
            return (string) GetValue( TextProperty );
         }
         set
         {
            SetValue( TextProperty, value );
         }
      }

      public static DependencyProperty MaxLengthProperty = DependencyProperty.Register( nameof( MaxLength ),
         typeof( int ),
         typeof( MainEntryBox ),
         new PropertyMetadata( 0 ) );

      public int MaxLength
      {
         get
         {
            return (int) GetValue( MaxLengthProperty );
         }
         set
         {
            SetValue( MaxLengthProperty, value );
         }
      }

      public static DependencyProperty RadialTextProperty = DependencyProperty.Register( nameof( RadialText ),
         typeof( string ),
         typeof( MainEntryBox ),
         new PropertyMetadata( string.Empty ) );

      public string RadialText
      {
         get
         {
            return (string) GetValue( RadialTextProperty );
         }
         set
         {
            SetValue( RadialTextProperty, value );
         }
      }

      public static DependencyProperty IsEditableProperty = DependencyProperty.Register( nameof( IsEditable ),
         typeof( bool ),
         typeof( MainEntryBox ),
         new PropertyMetadata( true, OnIsEditableChanged ) );

      public bool IsEditable
      {
         get
         {
            return (bool) GetValue( IsEditableProperty );
         }
         set
         {
            SetValue( IsEditableProperty, value );
         }
      }

      public MainEntryBox()
      {
         InitializeComponent();
         LayoutRoot.DataContext = this;
      }

      private static void OnIsEditableChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
      {
         bool oldValue = (bool) e.OldValue;
         bool newValue = (bool) e.NewValue;

         if ( oldValue == newValue )
         {
            return;
         }

         var source = (MainEntryBox) d;
         var primaryBorder = (Border) source.FindName( "PrimaryBorder" );
         FrameworkElement newPrimaryTextBox;

         if ( newValue )
         {
            newPrimaryTextBox = new TextBox
            {
               Text = "Editable"
            };
         }
         else
         {
            newPrimaryTextBox = new TextBlock
            {
               FontFamily = new FontFamily( "Consolas" ),
               FontSize = 24,
               HorizontalAlignment = HorizontalAlignment.Stretch,
               Text = "Not editable"
            };

            newPrimaryTextBox.SetResourceReference( TextBlock.ForegroundProperty, "TextColor" );

            var binding = new Binding( "Text" )
            {
               Source = source,
               Mode = BindingMode.OneWay,
            };

            BindingOperations.SetBinding( newPrimaryTextBox, TextBlock.TextProperty, binding );
         }

         primaryBorder.Child = newPrimaryTextBox;
         primaryBorder.InvalidateVisual();
      }

      public void HideCaret() => PrimaryTextBox.CaretBrush = new SolidColorBrush( Colors.Transparent );
      public void ShowCaret() => PrimaryTextBox.ClearValue( TextBoxBase.CaretBrushProperty );
      public void MoveCaretToEnd() => PrimaryTextBox.SelectionStart = PrimaryTextBox.Text.Length;

      private void MainEntryBox_OnGotFocus( object sender, RoutedEventArgs e ) => PrimaryTextBox.Focus();

      private void MainEntryBox_OnLoaded( object sender, RoutedEventArgs e )
         => PrimaryTextBox.SelectionStart = PrimaryTextBox.Text.Length;

      public void AnimateRadialTextTo( string text )
      {
         RadialTextBlock.AnimateTextTo( text );
      }

      public void RestoreCounter()
      {
         RadialTextBlock.RestoreCounter();
      }
   }
}
