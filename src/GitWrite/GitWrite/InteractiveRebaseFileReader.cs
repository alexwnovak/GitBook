namespace GitWrite
{
   public class InteractiveRebaseFileReader
   {
      private readonly IFileAdapter _fileAdapter;

      public InteractiveRebaseFileReader( IFileAdapter fileAdapter )
      {
         _fileAdapter = fileAdapter;
      }
   }
}
