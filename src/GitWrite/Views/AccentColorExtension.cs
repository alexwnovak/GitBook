using System;
using System.Windows.Markup;
using System.Windows.Media;

namespace GitWrite.Views
{
   [MarkupExtensionReturnType( typeof( Brush ) )]
   public class AccentColorExtension : MarkupExtension
   {
      public override object ProvideValue( IServiceProvider serviceProvider )
      {
         throw new NotImplementedException();
      }
   }
}
