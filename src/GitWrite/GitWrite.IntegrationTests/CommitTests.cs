using Xunit;
using FluentAssertions;
using GitModel;
using GitWrite.IntegrationTests.Infrastructure;

namespace GitWrite.IntegrationTests
{
   public class CommitTests
   {
      [Fact]
      public void CommitDetailsAreEditedThenSaved()
      {
         var commitScenario = CommitScenarioBuilder.Create().Build();

         commitScenario.SetSubject( "Something else" );
         commitScenario.SetBody( new[] { "Body line 1", "Body line 2" } );
         commitScenario.AcceptChanges();

         var actualCommit = new CommitFileReader().FromFile( commitScenario.FilePath );
         actualCommit.Subject.Should().Be( "Something else" );
         actualCommit.Body[0].Should().Be( "Body line 1" );
         actualCommit.Body[1].Should().Be( "Body line 2" );
      }
   }
}
