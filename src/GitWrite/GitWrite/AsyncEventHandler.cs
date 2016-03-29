using System;
using System.Threading.Tasks;

namespace GitWrite
{
   public delegate Task AsyncEventHandler( object sender, EventArgs e );

   public delegate Task AsyncEventHandler<in T>( object sender, T e ) where T : EventArgs;
}
