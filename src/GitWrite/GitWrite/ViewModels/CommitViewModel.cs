using System;
using System.ComponentModel;
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
      private readonly ICommitFileWriter _commitFileWriter;

      public RelayCommand SecondaryNotesGotFocusCommand
      {
         get;
      }

      public RelayCommand<CancelEventArgs> CloseCommand
      {
         get;
      }

      public RelayCommand LoadCommand
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

      public event AsyncEventHandler<ShutdownEventArgs> AsyncExitRequested;
       
      public CommitViewModel( string commitFilePath,
         IViewService viewService,
         IAppService appService,
         IClipboardService clipboardService,
         CommitDocument commitDocument,
         IGitService gitService,
         ICommitFileWriter commitFileWriter,
         IMessenger messenger )
         : base( viewService, appService, messenger )
      {
         _commitFilePath = commitFilePath;
         _clipboardService = clipboardService;
         _commitDocument = commitDocument;
         _gitService = gitService;
         _commitFileWriter = commitFileWriter;

         SecondaryNotesGotFocusCommand = new RelayCommand( ExpandUI );
         LoadCommand = new RelayCommand( ViewLoaded );
         PasteCommand = new RelayCommand( PasteFromClipboard );

         ShortMessage = _commitDocument?.Subject;

         if ( _commitDocument != null && _commitDocument.Body?.Length > 0 )
         {
            ExtraCommitText = _commitDocument.Body.Aggregate( ( i, j ) => $"{i}{Environment.NewLine}{j}");
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

      protected virtual async Task OnExitRequestedAsync( object sender, ShutdownEventArgs e )
         => await RaiseAsync( AsyncExitRequested, sender, e );

      protected override async Task<bool> OnSaveAsync()
      {
         if ( string.IsNullOrWhiteSpace( ShortMessage ) || IsExiting )
         {
            MessengerInstance.Send( new ShakeRequestedMessage() );
            return false;
         }

         IsExiting = true;

         CollapseUI();

         await OnExitRequestedAsync( this, new ShutdownEventArgs( ExitReason.Save ) );

         _commitDocument.Subject = ShortMessage;

         if ( string.IsNullOrEmpty( ExtraCommitText ) )
         {
            _commitDocument.Body = null;
         }
         else
         {
            _commitDocument.Body = ExtraCommitText.Split( new[] { Environment.NewLine }, StringSplitOptions.None );
         }

         _commitFileWriter.ToFile( _commitFilePath, _commitDocument );

         return true;
      }

      protected override async Task<bool> OnDiscardAsync()
      {
         CollapseUI();

         await OnExitRequestedAsync( this, new ShutdownEventArgs( ExitReason.Discard ) );

         _commitDocument.Subject = null;
         _commitDocument.Body = new string[0];

         _commitFileWriter.ToFile( _commitFilePath, _commitDocument );

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
