using GitModel;

namespace GitWrite.Services
{
   public static class GitRepositoryPathConverter
   {
      public static string GetPath( CommitDocument document )
      {
         return null;
         //if ( document == null || string.IsNullOrEmpty( document.Name ) )
         //{
         //   return string.Empty;
         //}

         //int repoPosition = document.Name.IndexOf( ".git" );

         //if ( repoPosition == -1 )
         //{
         //   return string.Empty;
         //}

         //return document.Name.Substring( 0, repoPosition + 4 );
      }
   }
}
