using System;
using System.Collections.Generic;
using GitWrite.ViewModels;

namespace GitWrite
{
   public class InteractiveRebaseFileReader
   {
      private readonly IFileAdapter _fileAdapter;

      public InteractiveRebaseFileReader( IFileAdapter fileAdapter )
      {
         _fileAdapter = fileAdapter;
      }

      public InteractiveRebaseDocument FromFile( string path )
      {
         var rebaseItems = new List<RebaseItem>();
         var document = CreateBasicDocument( path );

         foreach ( string line in document.RawLines )
         {
            if ( line.StartsWith( "#" ) || string.IsNullOrWhiteSpace( line ) )
            {
               continue;
            }

            int firstSpaceIndex = line.IndexOf( " " );
            string action = line.Substring( 0, firstSpaceIndex ).Trim();

            int secondSpaceIndex = line.IndexOf( " ", firstSpaceIndex + 1 );
            string commitHash = line.Substring( firstSpaceIndex, secondSpaceIndex - firstSpaceIndex ).Trim();

            string commitText = line.Substring( secondSpaceIndex ).Trim();

            var rebaseItem = new RebaseItem( commitText )
            {
               Action = ActionFromString( action ),
               CommitHash = commitHash
            };

            rebaseItems.Add( rebaseItem );
         }

         document.RebaseItems = rebaseItems.ToArray();
         return document;
      }

      private static RebaseItemAction ActionFromString( string action )
      {
         switch ( action )
         {
            case "pick":
               return RebaseItemAction.Pick;
         }

         throw new ArgumentException( $"Unknown action: {action}", nameof( action ) );
      }

      private InteractiveRebaseDocument CreateBasicDocument( string path )
      {
         return new InteractiveRebaseDocument
         {
            RawLines = _fileAdapter.ReadAllLines( path ),
            Name = path
         };
      }
   }
}
