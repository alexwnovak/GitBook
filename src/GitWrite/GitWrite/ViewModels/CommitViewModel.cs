using System;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GitModel;
using GitWrite.Messages;
using GitWrite.Services;
using Resx = GitWrite.Properties.Resources;

namespace GitWrite.ViewModels
{
   public class CommitViewModel : GitWriteViewModelBase
   {
      private readonly string _commitFilePath;
      private readonly IClipboardService _clipboardService;
      private readonly CommitDocument _commitDocument;
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

      private string _shortMessage;
      public string ShortMessage
      {
         get => _shortMessage;
         set
         {
            Set( () => ShortMessage, ref _shortMessage, value );
            IsDirty = true;
         }
      }

      private string _extraCommitText;
      public string ExtraCommitText
      {
         get => _extraCommitText;
         set
         {
            Set( () => ExtraCommitText, ref _extraCommitText, value );
            IsDirty = true;
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
         IViewService viewService,
         IAppService appService,
         IClipboardService clipboardService,
         CommitDocument commitDocument,
         IGitService gitService,
         IMessenger messenger )
         : base( viewService, appService, messenger )
      {
         _commitFilePath = commitFilePath;
         _clipboardService = clipboardService;
         _commitDocument = commitDocument;
         _gitService = gitService;

         LoadCommand = new RelayCommand( ViewLoaded );
         SaveCommand = new RelayCommand( OnSaveCommand );
         AbortCommand = new RelayCommand( OnAbortCommand );
         PasteCommand = new RelayCommand( PasteFromClipboard );

         ShortMessage = _commitDocument?.Subject;

         if ( _commitDocument != null && _commitDocument.Body?.Length > 0 )
         {
            ExtraCommitText = _commitDocument.Body.Aggregate( ( i, j ) => $"{i}{Environment.NewLine}{j}" );
         }

         IsDirty = false;
         IsAmending = !string.IsNullOrEmpty( ShortMessage );
      }

      public void ViewLoaded()
      {
         if ( !string.IsNullOrEmpty( ExtraCommitText ) )
         {
            ExpandUI();
         }
      }

      private async void OnSaveCommand()
      {
         ExitReason = ExitReason.Save;

         bool shouldContinue = await OnSaveAsync();

         if ( !shouldContinue )
         {
            return;
         }

         await OnShutdownRequested( this, new ShutdownEventArgs( ExitReason.Save ) );

         AppService.Shutdown();
      }

      private async void OnAbortCommand()
      {
         if ( IsExiting )
         {
            return;
         }

         ExitReason = ExitReason.Discard;
         ExitReason exitReason = ExitReason.Discard;

         if ( IsDirty )
         {
            var confirmationResult = ViewService.ConfirmExit();

            if ( confirmationResult == ExitReason.Cancel )
            {
               return;
            }

            if ( confirmationResult == ExitReason.Save )
            {
               ExitReason = ExitReason.Save;

               bool shouldReallyExit = await OnSaveAsync();

               if ( !shouldReallyExit )
               {
                  return;
               }

               exitReason = ExitReason.Save;
            }
            else
            {
               await OnDiscardAsync();
               exitReason = ExitReason.Discard;
               IsExiting = true;
            }
         }
         else if ( exitReason == ExitReason.Discard )
         {
            await OnDiscardAsync();
         }

         IsExiting = true;
         await OnShutdownRequested( this, new ShutdownEventArgs( exitReason ) );

         AppService.Shutdown();
      }

      protected async Task OnExitRequestedAsync( ExitReason exitReason )
      {
         var message = new ExitRequestedMessage( exitReason );

         MessengerInstance.Send( message );

         await message.Task;
      }

      protected async Task<bool> OnSaveAsync()
      {
         if ( string.IsNullOrWhiteSpace( ShortMessage ) || IsExiting )
         {
            MessengerInstance.Send( new ShakeRequestedMessage() );
            return false;
         }

         IsExiting = true;

         CollapseUI();

         await OnExitRequestedAsync( ExitReason.Save );

         _commitDocument.Subject = ShortMessage;

         if ( string.IsNullOrEmpty( ExtraCommitText ) )
         {
            _commitDocument.Body = null;
         }
         else
         {
            _commitDocument.Body = ExtraCommitText.Split( new[] { Environment.NewLine }, StringSplitOptions.None );
         }

         MessengerInstance.Send( new WriteCommitDocumentMessage( _commitFilePath, _commitDocument ) );

         return true;
      }

      protected async Task<bool> OnDiscardAsync()
      {
         CollapseUI();

         await OnExitRequestedAsync( ExitReason.Discard );

         _commitDocument.Subject = null;
         _commitDocument.Body = new string[0];

         MessengerInstance.Send( new WriteCommitDocumentMessage( _commitFilePath, _commitDocument ) );

         return true;
      }

      private void ExpandUI()
      {
         if ( !IsExpanded && !IsExiting )
         {
            IsExpanded = true;
            MessengerInstance.Send( new ExpansionRequestedMessage() );
         }
      }

      private void CollapseUI()
      {
         IsExpanded = false;
         MessengerInstance.Send( new CollapseRequestedMessage() );
      }

      private void PasteFromClipboard()
      {
         string clipboardText = _clipboardService.GetText();

         if ( !string.IsNullOrEmpty( clipboardText ) )
         {
            clipboardText = clipboardText.Trim( '\r', '\n' );
            int lineBreakIndex = clipboardText.IndexOf( Environment.NewLine );

            if ( lineBreakIndex != -1 )
            {
               ExpandUI();

               ShortMessage = clipboardText.Substring( 0, lineBreakIndex );

               string extraNotes = clipboardText.Substring( lineBreakIndex + Environment.NewLine.Length );
               extraNotes = extraNotes.TrimStart( '\r', '\n' ).TrimEnd( '\r', '\n' );
               ExtraCommitText = extraNotes;
            }
            else
            {
               ShortMessage = clipboardText;
            }
         }
      }
   }
}
