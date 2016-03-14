using System.Windows.Controls;
using System.Windows.Input;

namespace GitWrite.Views.Controls
{
   public partial class InteractiveRebasePanel : ListBox
   {
      public InteractiveRebasePanel()
      {
         InitializeComponent();
      }

      private void InteractiveRebaseWindow_OnPreviewKeyDown( object sender, KeyEventArgs e )
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
