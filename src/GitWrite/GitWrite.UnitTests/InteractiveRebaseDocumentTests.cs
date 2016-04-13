using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using Xunit;
using GitWrite.ViewModels;

namespace GitWrite.UnitTests
{
   public class InteractiveRebaseDocumentTests
   {
      [Fact]
      public void Save_DocumentHasOneItem_ItemIsSavedToFileAdapter()
      {
         const string documentName = "git-rebase-todo";

         var rebaseItemAction = RebaseItemAction.Pick;
         const string commitHash = "0123456";
         const string commitNotes = "Commit notes";

         IEnumerable<string> actualLines = null;

         // Setup

         var rebaseItems = new[]
         {
            RebaseItemHelper.Create( rebaseItemAction, commitHash, commitNotes )
         };

         var fileAdapterMock = new Mock<IFileAdapter>();
         fileAdapterMock.Setup( fa => fa.WriteAllLines( documentName, It.IsAny<IEnumerable<string>>() ) ).Callback<string, IEnumerable<string>>( ( _, lines ) =>
         {
            actualLines = lines;
         } );

         // Test

         var document = new InteractiveRebaseDocument( fileAdapterMock.Object )
         {
            Name = documentName,
            RebaseItems = rebaseItems
         };

         document.Save();

         // Assert

         actualLines.Should().HaveCount( 1 );
         actualLines.Single().Should().Be( $"{rebaseItemAction.ToString().ToLower()} {commitHash} {commitNotes}" );
      }
   }
}
