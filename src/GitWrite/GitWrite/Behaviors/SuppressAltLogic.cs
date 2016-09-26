using System.Windows.Input;

namespace GitWrite.Behaviors
{
   public class SuppressAltLogic
   {
      public bool ShouldSuppress( Key key ) => key == Key.LeftAlt || key == Key.RightAlt;
   }
}
