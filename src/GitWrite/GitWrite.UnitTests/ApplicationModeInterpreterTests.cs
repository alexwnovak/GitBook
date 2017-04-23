using FluentAssertions;
using Xunit;

namespace GitWrite.UnitTests
{
   public class ApplicationModeInterpreterTests
   {
      [Fact]
      public void GetFromFileName_NullFileName_ReturnsUnknown()
      {
         var applicationMode = ApplicationModeInterpreter.GetFromFileName( null );

         applicationMode.Should().Be( ApplicationMode.Unknown );
      }

      [Fact]
      public void GetFromFileName_EmptyStringFileName_ReturnsUnknown()
      {
         var applicationMode = ApplicationModeInterpreter.GetFromFileName( string.Empty );

         applicationMode.Should().Be( ApplicationMode.Unknown );
      }

      [Fact]
      public void GetFromFileName_FileNameIsGibberish_ReturnsUnknown()
      {
         var applicationMode = ApplicationModeInterpreter.GetFromFileName( "AnUnknownFile.txt" );

         applicationMode.Should().Be( ApplicationMode.Unknown );
      }

      [Fact]
      public void GetFromFileName_PassingCommitFileName_ReturnsCommitMode()
      {
         var applicationMode = ApplicationModeInterpreter.GetFromFileName( GitFileNames.CommitFileName );

         applicationMode.Should().Be( ApplicationMode.Commit );
      }

      [Fact]
      public void GetFromFileName_PassingRebaseFileName_ReturnsRebaseMode()
      {
         var applicationMode = ApplicationModeInterpreter.GetFromFileName( GitFileNames.RebaseFileName );

         applicationMode.Should().Be( ApplicationMode.Rebase );
      }

      [Fact]
      public void GetFromFileName_PassingPatchFileName_ReturnsPatchMode()
      {
         var applicationMode = ApplicationModeInterpreter.GetFromFileName( GitFileNames.EditPatchFileName );

         applicationMode.Should().Be( ApplicationMode.EditPatch );
      }

      [Fact]
      public void GetFromFileName_PassingAddEditPatch_ReturnsEditMode()
      {
         var applicationMode = ApplicationModeInterpreter.GetFromFileName( GitFileNames.AddEditPatchFileName );

         applicationMode.Should().Be( ApplicationMode.EditPatch );
      }
   }
}
