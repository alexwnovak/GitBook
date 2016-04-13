using FluentAssertions;
using GitWrite.ViewModels;
using Moq;
using Xunit;

namespace GitWrite.UnitTests
{
   public class InteractiveRebaseFileReaderTests
   {
      [Fact]
      public void FromFile_FileHasNoLines_ReturnsBlankDocument()
      {
         // Setup

         var fileAdapterMock = new Mock<IFileAdapter>();

         // Test

         var documentReader = new InteractiveRebaseFileReader( fileAdapterMock.Object );

         var document = documentReader.FromFile( "git-rebase-todo" );

         // Assert

         document.RawLines.Should().HaveCount( 0 );
      }

      [Fact]
      public void FromFile_FileHasNoItems_ReturnsNoItems()
      {
         const string fileName = "git-rebase-todo";

         string[] lines =
         {
            "# First line comment",
            "",
            "# Second line comment"
         };

         // Setup

         var fileAdapterMock = new Mock<IFileAdapter>();
         fileAdapterMock.Setup( fa => fa.ReadAllLines( fileName ) ).Returns( lines );

         // Test

         var documentReader = new InteractiveRebaseFileReader( fileAdapterMock.Object );

         var document = documentReader.FromFile( fileName );

         // Assert

         document.RawLines.Should().HaveCount( 3 );
         document.RebaseItems.Should().HaveCount( 0 );
      }

      [Fact]
      public void FromFile_FileHasOneCommit_ReadsSuccessfully()
      {
         const string fileName = "git-rebase-todo";
         RebaseItemAction action = RebaseItemAction.Pick;
         const string hash = "0123456";
         const string commit = "Some commit notes";

         string[] lines =
         {
            $"{action.ToString().ToLower()} {hash} {commit}"
         };

         // Setup

         var fileAdapterMock = new Mock<IFileAdapter>();
         fileAdapterMock.Setup( fa => fa.ReadAllLines( fileName ) ).Returns( lines );

         // Test

         var documentReader = new InteractiveRebaseFileReader( fileAdapterMock.Object );

         var document = documentReader.FromFile( fileName );

         // Assert

         document.RebaseItems.Should().HaveCount( 1 );
         document.RebaseItems[0].Action.Should().Be( action );
         document.RebaseItems[0].CommitHash.Should().Be( hash );
         document.RebaseItems[0].Text.Should().Be( commit );
      }
   }
}
