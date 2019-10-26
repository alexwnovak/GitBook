//using System.Windows.Controls;
//using System.Windows.Input;
//using System.Windows.Interactivity;
//using GitWrite.Views;

//namespace GitWrite.Behaviors
//{
//   public class TripleClickSelectionBehavior : Behavior<TextBox>
//   {
//      protected override void OnAttached()
//      {
//         AssociatedObject.PreviewMouseDown += AssociatedObjectOnMouseDown;
//      }

//      private void AssociatedObjectOnMouseDown( object sender, MouseButtonEventArgs e )
//      {
//         if ( e.ChangedButton == MouseButton.Left && e.ClickCount == 3 )
//         {
//            var textBox = (TextBox) sender;

//            var charIndex = textBox.GetCharacterIndexFromPoint( Mouse.GetPosition( textBox ), true );
//            var lineIndex = textBox.GetLineIndexFromCharacterIndex( charIndex );

//            textBox.HighlightLine( lineIndex );
//         }
//      }
//   }
//}
