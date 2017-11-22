//using System;
//using System.Collections.Generic;
//using GitWrite.ViewModels;

//namespace GitWrite
//{
//   public class RebaseFileReader
//   {
//      private readonly IFileAdapter _fileAdapter;
//      private static readonly RebaseItemAction[] _rebaseItemActions = GetRebaseItemActions();

//      public RebaseFileReader( IFileAdapter fileAdapter )
//      {
//         _fileAdapter = fileAdapter;
//      }

//      private static RebaseItemAction[] GetRebaseItemActions()
//         => (RebaseItemAction[]) Enum.GetValues( typeof( RebaseItemAction ) );

//      public RebaseDocument FromFile( string path )
//      {
//         var rebaseItems = new List<RebaseItem>();
//         var document = CreateBasicDocument( path );

//         foreach ( string line in document.RawLines )
//         {
//            if ( line.StartsWith( "#" ) || string.IsNullOrWhiteSpace( line ) )
//            {
//               continue;
//            }

//            int firstSpaceIndex = line.IndexOf( " " );
//            string action = line.Substring( 0, firstSpaceIndex ).Trim();

//            int secondSpaceIndex = line.IndexOf( " ", firstSpaceIndex + 1 );
//            string commitHash = line.Substring( firstSpaceIndex, secondSpaceIndex - firstSpaceIndex ).Trim();

//            string commitText = line.Substring( secondSpaceIndex ).Trim();

//            var rebaseItem = new RebaseItem( commitText )
//            {
//               Action = ActionFromString( action ),
//               CommitHash = commitHash
//            };

//            rebaseItems.Add( rebaseItem );
//         }

//         document.RebaseItems = rebaseItems.ToArray();
//         return document;
//      }

//      private static RebaseItemAction ActionFromString( string action )
//      {
//         foreach ( var rebaseAction in _rebaseItemActions )
//         {
//            if ( rebaseAction.ToString().Equals( action, StringComparison.InvariantCultureIgnoreCase ) )
//            {
//               return rebaseAction;
//            }
//         }

//         throw new ArgumentException( $"Unknown action: {action}", nameof( action ) );
//      }

//      private RebaseDocument CreateBasicDocument( string path )
//      {
//         return new RebaseDocument
//         {
//            RawLines = _fileAdapter.ReadAllLines( path ),
//            Name = path
//         };
//      }
//   }
//}
