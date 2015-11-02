using System;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GitWrite.UnitTests
{
   [TestClass]
   public class CommitDocumentTests
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
            Name = path,
            ShortMessage = shortMessage
         };

         commitDocument.Save();

         // Assert

         Assert.IsTrue( parametersMatch );
      }

      [TestMethod]
      public void Save_HasLongMessage_WritesLongMessage()
      {
         bool parametersMatch = false;
         const string path = "SomeFile.txt";
         const string shortMessage = "This is the short message";
         string longMessage = "This is the longer message" + Environment.NewLine + " with a new line";

         // Setup

         var fileAdapterMock = new Mock<IFileAdapter>();
         fileAdapterMock.Setup( fa => fa.WriteAllLines( It.IsAny<string>(), It.IsAny<string[]>() ) ).Callback<string, string[]>( ( p, l ) =>
         {
            parametersMatch = (p == path && l[0] == shortMessage && l[1] == string.Empty && l[2] == longMessage);
         } );
         SimpleIoc.Default.Register( () => fileAdapterMock.Object );

         // Test

         var commitDocument = new CommitDocument
         {
            Name = path,
            ShortMessage = shortMessage,
            LongMessage = longMessage
         };

         commitDocument.Save();

         // Assert

         Assert.IsTrue( parametersMatch );
      }
   }
}
