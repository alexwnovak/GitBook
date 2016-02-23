namespace GitWrite
{
   public static class ApplicationModeInterpreter
   {
      public static ApplicationMode GetFromFileName( string fileName )
      {
         switch ( fileName )
         {
            case GitFileNames.CommitFileName:
               return ApplicationMode.Commit;
            case GitFileNames.InteractiveRebaseFileName:
               return ApplicationMode.InteractiveRebase;
            case GitFileNames.EditPatchFileName:
               return ApplicationMode.EditPatch;
            default:
               return ApplicationMode.Unknown;
         }
      }
   }
}
