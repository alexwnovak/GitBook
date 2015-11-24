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
   }
}
