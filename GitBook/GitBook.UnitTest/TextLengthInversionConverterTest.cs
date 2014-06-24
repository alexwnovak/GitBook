using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GitBook.UnitTest
{
   [TestClass]
   public class TextLengthInversionConverterTest
   {
      [TestMethod]
      public void Convert_InputIsZero_ConvertsToMaxOf72()
      {
         var converter = new TextLengthInversionConverter();

         int invertedLength = (int) converter.Convert( 0, null, null, null );

         Assert.AreEqual( 72, invertedLength );
      }
   }
}
