using System;
using System.ComponentModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GitWrite.Services;
using GitWrite.Views;

namespace GitWrite.ViewModels
{
   public class CommitViewModel : GitWriteViewModelBase
   {
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

      public RelayCommand PasteCommand
      {
         get;
      }

      public CommitInputState InputState
      {
         get;
         set;
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
            _hasEditedCommitMessage = true;
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
            _hasEditedCommitMessage = true;
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

      private bool _isExiting;
      public bool IsExiting
      {
         get
         {
            return _isExiting;
         }
         set
         {
            Set( () => IsExiting, ref _isExiting, value );
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

      private bool _hasEditedCommitMessage;

      public event EventHandler ExpansionRequested;
      public event AsyncEventHandler AsyncExitRequested;
      public event EventHandler HelpRequested;
      public event EventHandler CollapseHelpRequested;
       
      public CommitViewModel()
      {
         PrimaryMessageGotFocusCommand = new RelayCommand( () => ControlState = CommitControlState.EditingPrimaryMessage );
         SecondaryNotesGotFocusCommand = new RelayCommand( ExpandUI );
         ExpandCommand = new RelayCommand( ExpandUI );
         HelpCommand = new RelayCommand( ActivateHelp );
         CloseCommand = new RelayCommand<CancelEventArgs>( CloseWindow );
         LoadCommand = new RelayCommand( ViewLoaded );
         PasteCommand = new RelayCommand( PasteFromClipboard );

         ShortMessage = App.CommitDocument?.ShortMessage;
         ExtraCommitText = App.CommitDocument?.LongMessage;

         _hasEditedCommitMessage = false;
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

      private void ShutDown() => SimpleIoc.Default.GetInstance<IAppService>().Shutdown();

      protected override async Task OnSaveAsync()
      {
      }

      protected override async Task OnDiscardAsync()
      {
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

      public async void Save()
      {
         if ( string.IsNullOrWhiteSpace( ShortMessage ) || IsExiting )
         {
            return;
         }

         var appController = SimpleIoc.Default.GetInstance<IAppController>();
         var exitTask = appController.ShutDownAsync( ExitReason.AcceptCommit );

          //= BeginShutDownAsync( ExitReason.AcceptCommit );

         App.CommitDocument.ShortMessage = ShortMessage;
         App.CommitDocument.LongMessage = ExtraCommitText;

         App.CommitDocument.Save();

         await exitTask;
         ShutDown();
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

      private async void CloseWindow( CancelEventArgs e )
      {
         if ( IsExiting )
         {
            return;
         }

         e.Cancel = true;

         if ( _hasEditedCommitMessage )
         {
            //CancelCommit();
            return;
         }

         //var appController = SimpleIoc.Default.GetInstance<IAppController>();
         //await appController.ShutDownAsync( ExitReason.AbortCommit );

         //await BeginShutDownAsync( ExitReason.AbortCommit );
         //ShutDown();
      }

      private void PasteFromClipboard()
      {
         var clipboard = SimpleIoc.Default.GetInstance<IClipboardService>();
         string clipboardText = clipboard.GetText();

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
