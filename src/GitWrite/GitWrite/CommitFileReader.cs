using System;
using GalaSoft.MvvmLight.Ioc;

namespace GitWrite
{
   public class CommitFileReader : ICommitFileReader
   {
      private readonly IFileAdapter _fileAdapter;

      public CommitFileReader()
      {
         _fileAdapter = SimpleIoc.Default.GetInstance<IFileAdapter>();
      }

      public CommitFileReader( IFileAdapter fileAdapter )
      {
         _fileAdapter = fileAdapter;
      }

      public CommitDocument FromFile( string path )
      {
         ThrowIfNotExists( path );

         var commitDocument = CreateBasicDocument( path );

         if ( commitDocument.RawLines == null || commitDocument.RawLines.Length == 0 )
         {
            throw new GitFileLoadException( "Incoming Git commit file is empty" );
         }

         ResolveExistingCommitMessages( commitDocument );

         return commitDocument;
      }

      private void ThrowIfNotExists( string path )
      {
         bool fileExists = _fileAdapter.Exists( path );

         if ( !fileExists )
         {
            throw new GitFileLoadException();
         }
      }

      private CommitDocument CreateBasicDocument( string path )
      {
         return new CommitDocument( _fileAdapter )
         {
            RawLines = _fileAdapter.ReadAllLines( path ),
            Name = path
         };
      }

      private static void ResolveExistingCommitMessages( CommitDocument commitDocument )
      {
         bool hasFoundShortMessage = false;
         bool hasStartedLongMessage = false;
         bool firstLineOfLongMessage = true;

         foreach ( string line in commitDocument.RawLines )
         {
            if ( line.StartsWith( "#" ) )
            {
               continue;
            }

            if ( !hasFoundShortMessage )
            {
               if ( string.IsNullOrEmpty( line ) )
               {
                  continue;
               }

               hasFoundShortMessage = true;
               commitDocument.ShortMessage = line.TrimEnd();
            }
            else
            {
               if ( !hasStartedLongMessage && string.IsNullOrEmpty( line ) )
               {
                  continue;
               }

               hasStartedLongMessage = true;

               if ( !firstLineOfLongMessage )
               {
                  commitDocument.LongMessage += Environment.NewLine;
               }

               firstLineOfLongMessage = false;
               commitDocument.LongMessage += line;
            }
         }

         if ( commitDocument.LongMessage != null )
         {
           commitDocument.LongMessage = commitDocument.LongMessage.TrimEnd( ' ', '\r', '\n' );
         }
      }
   }
}
