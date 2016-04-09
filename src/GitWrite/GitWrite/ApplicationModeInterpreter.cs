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

            if ( gitFileAttributes.Length == 1 )
            {
               var attribute = (GitFileAttribute) gitFileAttributes[0];

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
