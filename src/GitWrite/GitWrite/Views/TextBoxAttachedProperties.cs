using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using GitWrite.Behaviors;

namespace GitWrite.Views
{
   public static class TextBoxAttachedProperties
   {
      public static readonly DependencyProperty IsTripleClickEnabledProperty = DependencyProperty.RegisterAttached( "IsTripleClickEnabled",
         typeof( bool ),
         typeof( TextBoxAttachedProperties ),
         new FrameworkPropertyMetadata( false, OnIsTripleClickEnabledChanged ) );

      public static void SetIsTripleClickEnabled( DependencyObject obj, bool value )
      {
         // This never seems to be called when setting the attached property...
         // However, the change handler is called, so we'll do our thinking there.
         // We keep this around for the compiler, and to give our tests something to call

         OnIsTripleClickEnabledChanged( obj, new DependencyPropertyChangedEventArgs( IsTripleClickEnabledProperty, false, value ) );
      }

      private static void OnIsTripleClickEnabledChanged( DependencyObject obj, DependencyPropertyChangedEventArgs e )
      {
         var textBox = obj as TextBox;

         if ( textBox == null )
         {
            return;
         }

         var textBoxBehaviors = Interaction.GetBehaviors( textBox );
         textBoxBehaviors.Add( new TripleClickSelectionBehavior() );
      }
   }
}
