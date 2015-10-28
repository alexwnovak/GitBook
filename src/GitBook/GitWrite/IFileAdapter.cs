namespace GitWrite
{
   public interface IFileAdapter
   {
      bool Exists( string path );

      string[] ReadAllLines( string path );

      void WriteAllLines( string path, string[] lines );
   }
}
