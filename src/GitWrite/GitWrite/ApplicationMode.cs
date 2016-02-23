namespace GitWrite
{
   public enum ApplicationMode
   {
      Unknown,

      [GitFile( GitFileNames.CommitFileName )]
      Commit,

      [GitFile( GitFileNames.InteractiveRebaseFileName )]
      InteractiveRebase,

      [GitFile( GitFileNames.EditPatchFileName )]
      EditPatch
   }
}
