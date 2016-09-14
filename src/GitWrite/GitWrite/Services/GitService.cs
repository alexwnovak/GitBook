using LibGit2Sharp;

namespace GitWrite.Services
{
   public class GitService : IGitService
   {
      private readonly string _repositoryPath;

      public GitService( string repositoryPath )
      {
         _repositoryPath = repositoryPath;
      }

      public string GetCurrentBranchName()
      {
         if ( string.IsNullOrWhiteSpace( _repositoryPath ) )
         {
            return string.Empty;
         }

         using ( var repo = new Repository( _repositoryPath ) )
         {
            return repo.Head.FriendlyName;
         }
      }
   }
}
