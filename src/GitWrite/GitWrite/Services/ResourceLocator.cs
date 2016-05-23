using System.Windows;

namespace GitWrite.Services
{
   public class ResourceLocator : IResourceLocator
   {
      public object FromCurrentApplication( string key ) => Application.Current.Resources[key];
   }
}
