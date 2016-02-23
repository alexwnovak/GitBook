using System;

namespace GitWrite
{
   [AttributeUsage( AttributeTargets.All, AllowMultiple = true )]
   public class GitFileAttribute : Attribute
   {
      public string FileName
      {
         get;
      }

      public GitFileAttribute( string fileName )
      {
         FileName = fileName;
      }
   }
}
