using GitModel;

namespace GitWrite.ViewModels
{
   public delegate string GetCommitFilePathFunction();
   public delegate CommitDocument ReadCommitFileFunction( string filePath );
}
