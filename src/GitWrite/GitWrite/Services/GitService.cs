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
         using ( var repo = new Repository( _repositoryPath ) )
         {
            return repo.Head.FriendlyName;
         }
      }
   }
}
