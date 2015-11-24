using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GitWrite.UnitTests
{
   public partial class CircularListTests
   {
      [TestMethod]
      [ExpectedException( typeof( InvalidOperationException ) )]
      public void Indexer_ListIsEmpty_ThrowsInvalidOperationException()
      {
         var circularList = new CircularList<int>();

         int value = circularList[0];
      }

      [TestMethod]
      public void Indexer_RequestFirstFromListWithOneElement_ReturnsTheElement()
      {
         const int value = 123;

         var circularList = new CircularList<int>();
         circularList.Add( value );

         int actualValue = circularList[0];

         Assert.AreEqual( value, actualValue );
      }

      [TestMethod]
      public void Indexer_RequestTheTwoItemsFromList_GetsElementsAtTheCorrectPositions()
      {
         const int value1 = 256;
         const int value2 = 512;

         var circularList = new CircularList<int>();
         circularList.Add( value1 );
         circularList.Add( value2 );

         Assert.AreEqual( value1, circularList[0] );
         Assert.AreEqual( value2, circularList[1] );
      }
   }
}
