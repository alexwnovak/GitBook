namespace GitWrite
{
   public enum ApplicationMode
   {
      Unknown,

      [GitFile( GitFileNames.CommitFileName )]
      Commit,

      [GitFile( GitFileNames.RebaseFileName )]
      InteractiveRebase,

      [GitFile( GitFileNames.EditPatchFileName )]
      [GitFile( GitFileNames.AddEditPatchFileName )]
      EditPatch
   }
}
