using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GitWrite.UnitTests
{
   public partial class CircularListTests
   {
      [TestMethod]
      public void Count_NewList_CountIsZero()
      {
         var circularList = new CircularList<int>();

         Assert.AreEqual( 0, circularList.Count );
      }

      [TestMethod]
      public void Add_NewList_CountIsOne()
      {
         var circularList = new CircularList<int>();

         circularList.Add( 0 );

         Assert.AreEqual( 1, circularList.Count );
      }

      [TestMethod]
      public void Add_NewListAndAddsTwoThings_CountIsTwo()
      {
         var circularList = new CircularList<int>();

         circularList.Add( 0 );
         circularList.Add( 0 );

         Assert.AreEqual( 2, circularList.Count );
      }
   }
}
