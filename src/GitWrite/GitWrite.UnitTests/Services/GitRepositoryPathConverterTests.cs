using FluentAssertions;
using GitWrite.Services;
using Xunit;

namespace GitWrite.UnitTests.Services
{
   public class GitRepositoryPathConverterTests
   {
      [Fact]
      public void GetPath_CommitDocumentIsNull_ReturnsEmptyString()
      {
         string path = GitRepositoryPathConverter.GetPath( null );

         path.Should().Be( string.Empty );
      }

      [Theory]
      [InlineData( null, "" )]
      [InlineData( "", "" )]
      public void GetPath_DocumentNameVaries( string documentName, string expectedPath )
      {
         var commitDocument = new CommitDocument( null )
         {
            Name = documentName
         };

         string path = GitRepositoryPathConverter.GetPath( commitDocument );

         path.Should().Be( expectedPath );
      }
   }
}
