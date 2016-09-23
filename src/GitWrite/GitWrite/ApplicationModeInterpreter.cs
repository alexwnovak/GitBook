using System;

namespace GitWrite
{
   public static class ApplicationModeInterpreter
   {
      public static ApplicationMode GetFromFileName( string fileName )
      {
         var enumType = typeof( ApplicationMode );
         var enumValues = Enum.GetValues( typeof( ApplicationMode ) );

         foreach ( var enumValue in enumValues )
         {
            var memberType = enumType.GetMember( enumValue.ToString() );
            var gitFileAttributes = memberType[0].GetCustomAttributes( typeof( GitFileAttribute ), false );

            foreach ( GitFileAttribute attribute in gitFileAttributes )
            {
               if ( attribute.FileName == fileName )
               {
                  return (ApplicationMode) enumValue;
               }
            }
         }

         return ApplicationMode.Unknown;
      }
   }
}
