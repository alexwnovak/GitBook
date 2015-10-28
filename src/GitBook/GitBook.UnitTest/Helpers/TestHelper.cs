using System.Windows;
using System.Windows.Input;
using Moq;

namespace GitWrite.UnitTests.Helpers
{
   public static class TestHelper
   {
      public static KeyEventArgs GetKeyEventArgs( Key key )
      {
         return new KeyEventArgs( null, Mock.Of<PresentationSource>(), 0, key );
      }
   }
}
