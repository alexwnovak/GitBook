using GitModel;

namespace GitWrite
{
   public enum ApplicationMode
   {
      Unknown,

      [GitFile( GitFileNames.CommitFileName )]
      Commit,

      [GitFile( GitFileNames.RebaseFileName )]
      Rebase,

      [GitFile( GitFileNames.EditPatchFileName )]
      [GitFile( GitFileNames.AddEditPatchFileName )]
      EditPatch
   }
}
