using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GitModel;
using GitWrite.Messages;
using Resx = GitWrite.Properties.Resources;

namespace GitWrite.ViewModels
{
   public class CommitViewModel : ViewModelBase
   {
      private readonly string _commitFilePath;
      public string CommitFilePath => _commitFilePath;
      public CommitDocument CommitDocument { get; }

      public RelayCommand AcceptCommand { get; }
      public RelayCommand DiscardCommand { get; }
      public RelayCommand SettingsCommand { get; }

      public bool IsDirty
      {
         get;
         set;
      }

      public string Title => Resx.ApplicationName;

      public bool IsExpanded
      {
         get;
         set;
      }

      public bool IsAmending
      {
         get;
         set;
      }

      public CommitViewModel( string commitFilePath,
         CommitDocument commitDocument,
         IMessenger messenger )
         : base( messenger )
      {
         _commitFilePath = commitFilePath;
         CommitDocument = commitDocument;

         AcceptCommand = new RelayCommand( OnAcceptCommand );
         DiscardCommand = new RelayCommand( OnDiscardCommand );
         SettingsCommand = new RelayCommand( OnSettingsCommand );

         IsDirty = false;
         IsAmending = !string.IsNullOrEmpty( CommitDocument.Subject );
      }

      private void OnAcceptCommand()
      {
      }

      private void OnDiscardCommand()
      {
      }

      private void OnSettingsCommand()
      {
      }

      //public void ViewLoaded()
      //{
      //   if ( !string.IsNullOrEmpty( ExtraCommitText ) )
      //   {
      //      ExpandUI();
      //   }
      //}

      //private async void OnSaveCommand()
      //{
      //   ExitReason = ExitReason.Save;

      //   bool shouldContinue = await OnSaveAsync();

      //   if ( !shouldContinue )
      //   {
      //      return;
      //   }

      //   MessengerInstance.Send( new ShutdownRequestedMessage() );
      //}

      //private async void OnAbortCommand()
      //{
      //   if ( IsExiting )
      //   {
      //      return;
      //   }

      //   ExitReason = ExitReason.Discard;
      //   ExitReason exitReason = ExitReason.Discard;

      //   if ( IsDirty )
      //   {
      //      var confirmationResult = ViewService.ConfirmExit();

      //      if ( confirmationResult == ExitReason.Cancel )
      //      {
      //         return;
      //      }

      //      if ( confirmationResult == ExitReason.Save )
      //      {
      //         ExitReason = ExitReason.Save;

      //         bool shouldReallyExit = await OnSaveAsync();

      //         if ( !shouldReallyExit )
      //         {
      //            return;
      //         }
      //      }
      //      else
      //      {
      //         await OnDiscardAsync();
      //         IsExiting = true;
      //      }
      //   }
      //   else if ( exitReason == ExitReason.Discard )
      //   {
      //      await OnDiscardAsync();
      //   }

      //   IsExiting = true;
      //   MessengerInstance.Send( new ShutdownRequestedMessage() );
      //}

      protected async Task OnExitRequestedAsync( ExitReason exitReason )
      {
         var message = new ExitRequestedMessage( exitReason );

         MessengerInstance.Send( message );

         await message.Task;
      }

      //protected async Task<bool> OnSaveAsync()
      //{
      //   if ( string.IsNullOrWhiteSpace( ShortMessage ) || IsExiting )
      //   {
      //      MessengerInstance.Send( new ShakeRequestedMessage() );
      //      return false;
      //   }

      //   IsExiting = true;

      //   CollapseUI();

      //   await OnExitRequestedAsync( ExitReason.Save );

      //   _commitDocument.Subject = ShortMessage;

      //   if ( string.IsNullOrEmpty( ExtraCommitText ) )
      //   {
      //      _commitDocument.Body = null;
      //   }
      //   else
      //   {
      //      _commitDocument.Body = ExtraCommitText.Split( new[] { Environment.NewLine }, StringSplitOptions.None );
      //   }

      //   MessengerInstance.Send( new WriteCommitDocumentMessage( _commitFilePath, _commitDocument ) );

      //   return true;
      //}

      protected async Task<bool> OnDiscardAsync()
      {
         CollapseUI();

         await OnExitRequestedAsync( ExitReason.Discard );

         CommitDocument.Subject = null;
         CommitDocument.Body = new string[0];

         MessengerInstance.Send( new WriteCommitDocumentMessage( _commitFilePath, CommitDocument ) );

         return true;
      }

      //private void ExpandUI()
      //{
      //   if ( !IsExpanded && !IsExiting )
      //   {
      //      IsExpanded = true;
      //      MessengerInstance.Send( new ExpansionRequestedMessage() );
      //   }
      //}

      private void CollapseUI()
      {
         IsExpanded = false;
         MessengerInstance.Send( new CollapseRequestedMessage() );
      }
   }
}
