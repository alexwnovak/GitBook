using System;
using System.Threading.Tasks;

namespace GitWrite
{
   public delegate Task AsyncEventHandler( object sender, EventArgs e );

   public delegate Task<T> AsyncEventHandler<T>( object sender, EventArgs e );
}
