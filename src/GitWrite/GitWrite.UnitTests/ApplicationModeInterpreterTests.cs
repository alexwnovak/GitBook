using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GitWrite.UnitTests
{
   [TestClass]
   public class ApplicationModeInterpreterTests
   {
      [TestMethod]
      public void GetFromFileName_NullFileName_ReturnsUnknown()
      {
         var applicationMode = ApplicationModeInterpreter.GetFromFileName( null );

         Assert.AreEqual( ApplicationMode.Unknown, applicationMode );
      }

      [TestMethod]
      public void GetFromFileName_EmptyStringFileName_ReturnsUnknown()
      {
         var applicationMode = ApplicationModeInterpreter.GetFromFileName( string.Empty );

         Assert.AreEqual( ApplicationMode.Unknown, applicationMode );
      }
   }
}
