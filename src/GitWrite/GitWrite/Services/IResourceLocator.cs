namespace GitWrite.Services
{
   public interface IResourceLocator
   {
      object FromCurrentApplication( string key );
   }
}
