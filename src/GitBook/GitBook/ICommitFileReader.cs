namespace GitBook
{
   public interface ICommitFileReader
   {
      CommitDocument FromFile( string path );
   }
}
