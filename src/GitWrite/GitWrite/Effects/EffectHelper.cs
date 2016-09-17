using System;
using System.Reflection;
using System.Windows.Media.Effects;

namespace GitWrite.Effects
{
   public static class EffectHelper
   {
      public static PixelShader CreateShader( string path )
      {
         return new PixelShader
         {
            UriSource = MakePackUri( path )
         };
      }

      public static Uri MakePackUri( string relativeFile )
      {
         Assembly assembly = typeof( EffectHelper ).Assembly;

         string assemblyShortName = assembly.ToString().Split( ',' )[0];
         string uriString = $"pack://application:,,,/{assemblyShortName};component/{relativeFile}";

         return new Uri( uriString );
      }
   }
}
