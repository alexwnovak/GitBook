using System;
using System.Threading.Tasks;
using System.Windows;
using GitWrite.Views;
using Resx = GitWrite.Properties.Resources;

namespace GitWrite.Services
{
   public class ViewService : IViewService
   {
      private readonly Func<Window> _windowProvider;

      public ViewService( Func<Window> windowProvider ) => _windowProvider = windowProvider;

      public async Task CloseViewAsync( bool acceptChanges )
      {
         string storyboardName = acceptChanges ? "AcceptDismissal" : "DiscardDismissal";

         await _windowProvider().PlayStoryboardAsync( storyboardName );

         _windowProvider().Close();
      }

      public void DisplaySubjectHint() => _windowProvider().PlayStoryboard( "SubjectHint" );

      public ExitReason ConfirmDiscard()
      {
         var result = MessageBox.Show( Resx.ExitConfirmationBodyText, Resx.ExitConfirmationHeaderText, MessageBoxButton.YesNoCancel, MessageBoxImage.Question );

         switch ( result )
         {
            case MessageBoxResult.Yes: return ExitReason.Save;
            case MessageBoxResult.No: return ExitReason.Discard;
            default: return ExitReason.Cancel;
         }
      }
   }
}
