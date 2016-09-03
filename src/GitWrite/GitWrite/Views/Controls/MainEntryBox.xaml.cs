using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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

      private void MainEntryBox_OnGotFocus( object sender, RoutedEventArgs e ) => PrimaryTextBox.Focus();
   }
}
