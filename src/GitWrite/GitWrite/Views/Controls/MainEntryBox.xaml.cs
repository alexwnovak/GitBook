using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

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

      public void HideRadialText() => RadialCounter.TextColor = Colors.Transparent;

      public void ShowRadialText() => RadialCounter.ClearValue( RadialCounter.TextColorProperty );

      private void MainEntryBox_OnGotFocus( object sender, RoutedEventArgs e ) => PrimaryTextBox.Focus();

      private void MainEntryBox_OnLoaded( object sender, RoutedEventArgs e )
         => PrimaryTextBox.SelectionStart = PrimaryTextBox.Text.Length;
   }
}
