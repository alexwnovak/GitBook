using System;
using System.Threading.Tasks;

namespace GitWrite
{
   public delegate Task AsyncEventHandler( object sender, EventArgs e );
}
