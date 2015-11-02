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
            default:
               return ApplicationMode.Unknown;
         }
      }
   }
}
