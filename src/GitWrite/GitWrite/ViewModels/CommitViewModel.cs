using System;
using System.ComponentModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GitWrite.Services;
using Resx = GitWrite.Properties.Resources;

namespace GitWrite.ViewModels
{
   public class CommitViewModel : GitWriteViewModelBase
   {
      private readonly IClipboardService _clipboardService;
      private readonly ICommitDocument _commitDocument;
      private readonly IGitService _gitService;

      public RelayCommand SecondaryNotesGotFocusCommand
      {
         get;
      }

      public RelayCommand ExpandCommand
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
         get
         {
            return _shortMessage;
         }
         set
         {
            Set( () => ShortMessage, ref _shortMessage, value );
            IsDirty = true;
         }
      }

      private string _extraCommitText;
      public string ExtraCommitText
      {
         get
         {
            return _extraCommitText;
         }
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

      public event AsyncEventHandler AsyncExpansionRequested;
      public event AsyncEventHandler AsyncCollapseRequested;
      public event AsyncEventHandler AsyncShakeRequested;
      public event AsyncEventHandler<ShutdownEventArgs> AsyncExitRequested;
       
      public CommitViewModel( IViewService viewService, IAppService appService, IClipboardService clipboardService, ICommitDocument commitDocument, IGitService gitService )
         : base( viewService, appService )
      {
         _clipboardService = clipboardService;
         _commitDocument = commitDocument;
         _gitService = gitService;

         SecondaryNotesGotFocusCommand = new RelayCommand( async () => await ExpandUI() );
         ExpandCommand = new RelayCommand( async () => await ExpandUI() );
         LoadCommand = new RelayCommand( ViewLoaded );
         PasteCommand = new RelayCommand( async () => await PasteFromClipboard() );

         ShortMessage = _commitDocument?.ShortMessage ?? string.Empty;
         ExtraCommitText = _commitDocument?.LongMessage;

         IsDirty = false;
         IsAmending = !string.IsNullOrEmpty( ShortMessage );
      }

      public async void ViewLoaded()
      {
         if ( !string.IsNullOrEmpty( ExtraCommitText ) )
         {
            await ExpandUI();
         }
      }

      protected virtual async Task OnExpansionRequestedAsync( object sender, EventArgs e )
         => await RaiseAsync( AsyncExpansionRequested, sender, e );

      protected virtual async Task OnCollapseRequestedAsync( object sender, EventArgs e )
         => await RaiseAsync( AsyncCollapseRequested, sender, e );

      protected virtual async Task OnShakeRequestedAsync( object sender, EventArgs e )
         => await RaiseAsync( AsyncShakeRequested, sender, e );

      protected virtual async Task OnExitRequestedAsync( object sender, ShutdownEventArgs e )
         => await RaiseAsync( AsyncExitRequested, sender, e );

      protected override async Task<bool> OnSaveAsync()
      {
         if ( string.IsNullOrWhiteSpace( ShortMessage ) || IsExiting )
         {
            await OnShakeRequestedAsync( this, EventArgs.Empty );
            return false;
         }

         IsExiting = true;

         await CollapseUIAsync();

         await OnExitRequestedAsync( this, new ShutdownEventArgs( ExitReason.Save ) );

         _commitDocument.ShortMessage = ShortMessage;
         _commitDocument.LongMessage = ExtraCommitText;
         _commitDocument.Save();

         return true;
      }

      protected override async Task<bool> OnDiscardAsync()
      {
         await CollapseUIAsync();

         await OnExitRequestedAsync( this, new ShutdownEventArgs( ExitReason.Discard ) );

         _commitDocument.ShortMessage = null;
         _commitDocument.LongMessage = null;
         _commitDocument.Save();

         return true;
      }

      private async Task ExpandUI()
      {
         if ( !IsExpanded && !IsExiting )
         {
            IsExpanded = true;
            await OnExpansionRequestedAsync( this, EventArgs.Empty );
         }
      }

      private async Task CollapseUIAsync()
      {
         IsExpanded = false;
         await OnCollapseRequestedAsync( this, EventArgs.Empty );
      }

      private async Task PasteFromClipboard()
      {
         string clipboardText = _clipboardService.GetText();

         if ( !string.IsNullOrEmpty( clipboardText ) )
         {
            clipboardText = clipboardText.Trim( '\r', '\n' );
            int lineBreakIndex = clipboardText.IndexOf( Environment.NewLine );

            if ( lineBreakIndex != -1 )
            {
               await ExpandUI();

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
