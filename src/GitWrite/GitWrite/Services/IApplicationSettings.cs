namespace GitWrite.Services
{
   public interface IApplicationSettings
   {
      string Theme
      {
         get;
         set;
      }

      int WindowX
      {
         get;
         set;
      }

      int WindowY
      {
         get;
         set;
      }
   }
}
