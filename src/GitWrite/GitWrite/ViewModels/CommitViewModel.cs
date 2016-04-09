using System;
using System.ComponentModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GitWrite.Services;
using GitWrite.Views;

namespace GitWrite.ViewModels
{
   public class CommitViewModel : GitWriteViewModelBase
   {
      private readonly IClipboardService _clipboardService;
      private readonly IGitService _gitService;

      public RelayCommand PrimaryMessageGotFocusCommand
      {
         get;
      }

      public RelayCommand SecondaryNotesGotFocusCommand
      {
         get;
      }

      public RelayCommand ExpandCommand
      {
         get;
      }

      public RelayCommand HelpCommand
      {
         get;
         protected internal set;
      }

      public RelayCommand<CancelEventArgs> CloseCommand
      {
         get;
      }

      public RelayCommand LoadCommand
      {
         get;
      }

      public ICommitDocument CommitDocument
      {
         get;
      }

      public CommitInputState InputState
      {
         get;
         set;
      }

      public string Title => $"Commiting to {_gitService.GetCurrentBranchName()}";

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

      public string HelpText => HelpTextProvider.GetTextForCommitState( ControlState );

      private CommitControlState _commitControlState;
      public CommitControlState ControlState
      {
         get
         {
            return _commitControlState;
         }
         set
         {
            Set( () => ControlState, ref _commitControlState, value );
            RaisePropertyChanged( () => HelpText );
         }
      }

      public bool IsHelpStateActive
      {
         get;
         set;
      }

      public bool IsExpanded
      {
         get;
         set;
      }

      public event EventHandler ExpansionRequested;
      public event AsyncEventHandler AsyncExitRequested;
      public event EventHandler HelpRequested;
      public event EventHandler CollapseHelpRequested;
       
      public CommitViewModel( IViewService viewService, IAppService appService, IClipboardService clipboardService, ICommitDocument commitDocument, IGitService gitService )
         : base( viewService, appService )
      {
         _clipboardService = clipboardService;
         _gitService = gitService;

         CommitDocument = commitDocument;

         PrimaryMessageGotFocusCommand = new RelayCommand( () => ControlState = CommitControlState.EditingPrimaryMessage );
         SecondaryNotesGotFocusCommand = new RelayCommand( ExpandUI );
         ExpandCommand = new RelayCommand( ExpandUI );
         HelpCommand = new RelayCommand( ActivateHelp );
         LoadCommand = new RelayCommand( ViewLoaded );
         SaveCommand = new RelayCommand( async () => await OnSaveAsync() );
         PasteCommand = new RelayCommand( PasteFromClipboard );

         ShortMessage = CommitDocument?.ShortMessage;
         ExtraCommitText = CommitDocument?.LongMessage;

         IsDirty = false;
      }

      public void ViewLoaded()
      {
         if ( !string.IsNullOrEmpty( ExtraCommitText ) )
         {
            ExpandUI();
         }
      }

      protected virtual void OnExpansionRequested( object sender, EventArgs e ) => ExpansionRequested?.Invoke( sender, e );

      protected virtual void OnHelpRequested( object sender, EventArgs e ) => HelpRequested?.Invoke( sender, e );

      protected virtual void OnCollapseHelpRequested( object sender, EventArgs e ) => CollapseHelpRequested?.Invoke( sender, e );

      protected virtual Task OnExitRequestedAsync( object sender, EventArgs e ) => AsyncExitRequested?.Invoke( sender, e );

      protected override async Task OnSaveAsync()
      {
         if ( string.IsNullOrWhiteSpace( ShortMessage ) || IsExiting )
         {
            return;
         }

         var shutdownTask = OnShutdownRequested( this, new ShutdownEventArgs( ExitReason.Accept ) );

         CommitDocument.ShortMessage = ShortMessage;
         CommitDocument.LongMessage = ExtraCommitText;
         CommitDocument.Save();

         await shutdownTask;

         AppService.Shutdown();
      }

      public bool DismissHelpIfActive()
      {
         if ( IsHelpStateActive )
         {
            OnCollapseHelpRequested( this, EventArgs.Empty );
            IsHelpStateActive = false;

            return true;
         }

         return false;
      }

      private void ExpandUI()
      {
         if ( !IsExpanded && !IsExiting )
         {
            IsExpanded = true;
            OnExpansionRequested( this, EventArgs.Empty );
         }
      }

      private void ActivateHelp()
      {
         if ( !IsHelpStateActive )
         {
            IsHelpStateActive = true;
            OnHelpRequested( this, EventArgs.Empty );
         }
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
