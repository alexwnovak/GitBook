using System.Windows;

namespace GitWrite.Services
{
   public class ClipboardService : IClipboardService
   {
      public string GetText() => Clipboard.GetText();
   }
}
