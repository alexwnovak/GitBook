using System;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GitModel;
using GitWrite.Messages;
using GitWrite.Services;
using Resx = GitWrite.Properties.Resources;

namespace GitWrite.ViewModels
{
   public class CommitViewModel : ViewModelBase
   {
      private readonly string _commitFilePath;
      public string CommitFilePath => _commitFilePath;
      private readonly IClipboardService _clipboardService;
      public CommitDocument CommitDocument { get; }
      private readonly IGitService _gitService;

      public RelayCommand LoadCommand
      {
         get;
      }

      public RelayCommand SaveCommand
      {
         get;
      }

      public RelayCommand AbortCommand
      {
         get;
      }

      public bool IsDirty
      {
         get;
         set;
      }

      public string Title
      {
         get
         {
            string branchName = _gitService.GetCurrentBranchName();

            if ( string.IsNullOrEmpty( branchName ) )
            {
               return Resx.CommittingHeaderText;
            }

            return string.Format( Resx.CommittingToBranchText, branchName, Resx.ApplicationName );
         }
      }

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
         IClipboardService clipboardService,
         CommitDocument commitDocument,
         IGitService gitService,
         IMessenger messenger )
         : base( messenger )
      {
         _commitFilePath = commitFilePath;
         _clipboardService = clipboardService;
         CommitDocument = commitDocument;
         _gitService = gitService;

         //LoadCommand = new RelayCommand( ViewLoaded );
         //SaveCommand = new RelayCommand( OnSaveCommand );
         //AbortCommand = new RelayCommand( OnAbortCommand );

         IsDirty = false;
         IsAmending = !string.IsNullOrEmpty( CommitDocument.Subject );
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

      //private void PasteFromClipboard()
      //{
      //   string clipboardText = _clipboardService.GetText();

      //   if ( !string.IsNullOrEmpty( clipboardText ) )
      //   {
      //      clipboardText = clipboardText.Trim( '\r', '\n' );
      //      int lineBreakIndex = clipboardText.IndexOf( Environment.NewLine );

      //      if ( lineBreakIndex != -1 )
      //      {
      //         ExpandUI();

      //         ShortMessage = clipboardText.Substring( 0, lineBreakIndex );

      //         string extraNotes = clipboardText.Substring( lineBreakIndex + Environment.NewLine.Length );
      //         extraNotes = extraNotes.TrimStart( '\r', '\n' ).TrimEnd( '\r', '\n' );
      //         ExtraCommitText = extraNotes;
      //      }
      //      else
      //      {
      //         ShortMessage = clipboardText;
      //      }
      //   }
      //}
   }
}
