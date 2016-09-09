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

      public MainEntryBox()
      {
         InitializeComponent();
         LayoutRoot.DataContext = this;
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
