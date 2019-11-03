using System.Threading.Tasks;
using AutoFixture;
using Caliburn.Micro;
using FluentAssertions;
using GitModel;
using GitWrite.Tests.Internal;
using GitWrite.ViewModels;
using TechTalk.SpecFlow;

namespace GitWrite.Tests
{
   [Binding]
   public class CommitSteps
   {
      private readonly ScenarioContext _scenarioContext;
      private readonly TemporaryFolder _temporaryFolder;
      private readonly IFixture _fixture;

      private CommitViewModel _sut;

      public CommitSteps( ScenarioContext scenarioContext )
      {
         _scenarioContext = scenarioContext;
         _temporaryFolder = new TemporaryFolder();
         _fixture = new Fixture();

         PlatformProvider.Current = new TestPlatformProvider();
      }

      [Given( "I am editing a new commit" )]
      public async Task IAmEditingANewCommit()
      {
         string commitFilePath = _temporaryFolder.CreateFile( "" );
         _scenarioContext["CommitFilePath"] = commitFilePath;

         _fixture.RegisterFunction<GetCommitFilePathFunction>( () => commitFilePath );
         _fixture.RegisterFunction<ReadCommitFileFunction>( filePath => new CommitFileReader().FromFile( filePath ) );
         _fixture.RegisterFunction<WriteCommitFileFunction>( ( filePath, document ) => new CommitFileWriter().ToFile( filePath, document ) );

         _sut = _fixture.Build<CommitViewModel>()
                        .OmitAutoProperties()
                        .Create();

         await _sut.ActivateAsync();
      }

      [Given( @"I have entered (.*) into the subject field" )]
      public void GivenIHaveEnteredIntoTheSubjectField( string subject )
      {
         _scenarioContext["ExpectedSubject"] = subject;
         _sut.Commit.Subject = subject;
      }

      [When( @"I save the commit" )]
      public async Task WhenISaveTheCommit()
      {
         await _sut.Save();
      }

      [Then( @"the commit data is written to the commit file" )]
      public void ThenTheCommitDataIsWrittenToDisk()
      {
         var commitFileReader = new CommitFileReader();
         var commitDocument = commitFileReader.FromFile( (string) _scenarioContext["CommitFilePath"] );

         commitDocument.Subject.Should().Be( (string) _scenarioContext["ExpectedSubject"] );
      }

      [Then( "the window is closed" )]
      public void AndTheWindowIsClosed()
      {
         PlatformProvider.Current.As<TestPlatformProvider>().WasClosed.Should().BeTrue();
      }

      [AfterScenario]
      public void AfterScenario()
      {
         _temporaryFolder.Dispose();
      }
   }
}
