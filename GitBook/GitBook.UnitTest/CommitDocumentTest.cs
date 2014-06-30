using GalaSoft.MvvmLight.Ioc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GitBook.UnitTest
{
   [TestClass]
   public class CommitDocumentTest
   {
      [TestCleanup]
      public void Cleanup()
      {
         SimpleIoc.Default.Reset();
      }

      [TestMethod]
      public void Save_OnlyHasShortMessage_WritesCommitNotes()
      {
         bool parametersMatch = false;
         const string path = "SomeFile.txt";
         const string shortMessage = "This is the short message";
         
         // Setup

         var fileAdapterMock = new Mock<IFileAdapter>();
         fileAdapterMock.Setup( fa => fa.WriteAllLines( It.IsAny<string>(), It.IsAny<string[]>() ) ).Callback<string, string[]>( ( p, l ) =>
         {
            parametersMatch = ( p == path && l[0] == shortMessage );
         } );    
         SimpleIoc.Default.Register( () => fileAdapterMock.Object );

         // Test

         var commitDocument = new CommitDocument
         {
            Path = path,
            ShortMessage = shortMessage
         };

         commitDocument.Save();

         // Assert

         Assert.IsTrue( parametersMatch );
      }
   }
}
