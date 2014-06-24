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

      [TestMethod]
      public void Convert_InputIsMaxOf72_ReturnsZero()
      {
         var converter = new TextLengthInversionConverter();

         int invertedLength = (int) converter.Convert( 72, null, null, null );

         Assert.AreEqual( 0, invertedLength );
      }

      [TestMethod]
      public void Convert_InputIsNegative_ReturnsZero()
      {
         var converter = new TextLengthInversionConverter();

         int invertedLength = (int) converter.Convert( -1, null, null, null );

         Assert.AreEqual( 0, invertedLength );
      }

      [TestMethod]
      public void Convert_InputIsGreaterThanMaxOf72_ReturnsZero()
      {
         var converter = new TextLengthInversionConverter();

         int invertedLength = (int) converter.Convert( 73, null, null, null );

         Assert.AreEqual( 0, invertedLength );
      }
   }
}
