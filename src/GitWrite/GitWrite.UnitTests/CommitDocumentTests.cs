using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using GalaSoft.MvvmLight.Ioc;
using Moq;
using Xunit;

namespace GitWrite.UnitTests
{
   public class CommitDocumentTests
   {
      public CommitDocumentTests()
      {
         SimpleIoc.Default.Reset();
      }

      [Fact]
      public void Save_OnlyHasShortMessage_WritesCommitNotes()
      {
         bool parametersMatch = false;
         const string path = "SomeFile.txt";
         const string shortMessage = "This is the short message";
         
         // Setup

         var fileAdapterMock = new Mock<IFileAdapter>();
         fileAdapterMock.Setup( fa => fa.WriteAllLines( It.IsAny<string>(), It.IsAny<IEnumerable<string>>() ) ).Callback<string, IEnumerable<string>>( ( p, l ) =>
         {
            var lines = l.ToList();
            parametersMatch = ( p == path && lines[0] == shortMessage );
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

         parametersMatch.Should().BeTrue();
      }

      [Fact]
      public void Save_HasLongMessage_WritesLongMessage()
      {
         bool parametersMatch = false;
         const string path = "SomeFile.txt";
         const string shortMessage = "This is the short message";
         string longMessage = "This is the longer message" + Environment.NewLine + " with a new line";

         // Setup

         var fileAdapterMock = new Mock<IFileAdapter>();
         fileAdapterMock.Setup( fa => fa.WriteAllLines( It.IsAny<string>(), It.IsAny<IEnumerable<string>>() ) ).Callback<string, IEnumerable<string>>( ( p, l ) =>
         {
            var lines = l.ToList();
            parametersMatch = (p == path && lines[0] == shortMessage && lines[1] == string.Empty && lines[2] == longMessage);
         } );
         SimpleIoc.Default.Register( () => fileAdapterMock.Object );

         // Test

         var commitDocument = new CommitDocument
         {
            Name = path,
            ShortMessage = shortMessage,
            LongMessage = longMessage,
         };

         commitDocument.Save();

         // Assert

         parametersMatch.Should().BeTrue();
      }

      [Fact]
      public void Save_HasLongMessageInTwoParagraphs_WritesLongMessage()
      {
         bool parametersMatch = false;
         const string path = "SomeFile.txt";
         const string shortMessage = "This is the short message";
         string longMessage = $"First line {Environment.NewLine}{Environment.NewLine} Second line after one blank line";

         // Setup

         var fileAdapterMock = new Mock<IFileAdapter>();
         fileAdapterMock.Setup( fa => fa.WriteAllLines( It.IsAny<string>(), It.IsAny<IEnumerable<string>>() ) ).Callback<string, IEnumerable<string>>( ( p, l ) =>
         {
            var lines = l.ToList();
            parametersMatch = ( p == path && lines[0] == shortMessage && lines[1] == string.Empty && lines[2] == longMessage );
         } );
         SimpleIoc.Default.Register( () => fileAdapterMock.Object );

         // Test

         var commitDocument = new CommitDocument
         {
            Name = path,
            ShortMessage = shortMessage,
            LongMessage = longMessage,
         };

         commitDocument.Save();

         // Assert

         parametersMatch.Should().BeTrue();
      }
   }
}
