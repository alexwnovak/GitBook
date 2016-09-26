using System.Windows;
using System.Windows.Interactivity;

namespace GitWrite.Behaviors
{
   public class SuppressAltBehavior : Behavior<Window>
   {
      private readonly SuppressAltLogic _logicController;

      public SuppressAltBehavior()
      {
         _logicController = new SuppressAltLogic();
      }

      protected override void OnAttached()
      {
         AssociatedObject.PreviewKeyDown += ( _, e ) => e.Handled = _logicController.ShouldSuppress( e.SystemKey );
      }
   }
}
