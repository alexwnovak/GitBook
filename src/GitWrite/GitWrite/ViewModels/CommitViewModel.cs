using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GitWrite.Resources;
using GitWrite.Services;
using GitWrite.Views;
using GitWrite.Views.Controls;

namespace GitWrite.ViewModels
{
   public class CommitViewModel : ViewModelBase
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

      public RelayCommand SaveCommand
      {
         get;
         protected internal set;
      }

      public RelayCommand AbortCommand
      {
         get;
         protected internal set;
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
            _shortMessage = value;
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
            _extraCommitText = value;
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

      private ExitReason _exitReason;
      public ExitReason ExitReason
      {
         get
         {
            return _exitReason;
         }
         set
         {
            Set( () => ExitReason, ref _exitReason, value );
         }
      }

      public bool IsExpanded
      {
         get;
         set;
      }

      private bool _hasEditedCommitMessage;

      public event EventHandler ExpansionRequested;
      public event AsyncEventHandler AsyncExitRequested;
      public event AsyncEventHandler<ConfirmationResult> ConfirmExitRequested;
      public event EventHandler HelpRequested;
      public event EventHandler CollapseHelpRequested;
       
      public CommitViewModel()
      {
         PrimaryMessageGotFocusCommand = new RelayCommand( () => ControlState = CommitControlState.EditingPrimaryMessage );
         SecondaryNotesGotFocusCommand = new RelayCommand( ExpandUI );
         ExpandCommand = new RelayCommand( ExpandUI );
         SaveCommand = new RelayCommand( SaveCommit );
         AbortCommand = new RelayCommand( CancelCommit );
         HelpCommand = new RelayCommand( ActivateHelp );
         CloseCommand = new RelayCommand<CancelEventArgs>( CloseWindow );

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

      protected virtual Task<ConfirmationResult> OnConfirmExitRequestedAsync( object sender, EventArgs e )
         => ConfirmExitRequested?.Invoke( sender, e );

      private async Task BeginShutDownAsync( ExitReason exitReason )
      {
         ExitReason = exitReason;
         IsExiting = true;

         await OnExitRequestedAsync( this, EventArgs.Empty );
         await SimpleIoc.Default.GetInstance<IStoryboardHelper>().PlayAsync( "WindowExitStoryboard" );
      }

      private void ShutDown() => SimpleIoc.Default.GetInstance<IAppService>().Shutdown();

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

      private async void SaveCommit()
      {
         if ( string.IsNullOrWhiteSpace( ShortMessage ) || IsExiting )
         {
            return;
         }

         var exitTask = BeginShutDownAsync( ExitReason.AcceptCommit );

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

      private async Task<bool> ConfirmExitForChanges()
      {
         if ( _hasEditedCommitMessage )
         {
            var confirmationResult = await OnConfirmExitRequestedAsync( this, EventArgs.Empty );
         }

         return false;
      }

      private async void CancelCommit()
      {
         if ( IsExiting )
         {
            return;
         }

         if ( _hasEditedCommitMessage && !( await ConfirmExitForChanges() ) )
         {
            return;
         }

         InputState = CommitInputState.Exiting;

         var shutDownTask = BeginShutDownAsync( ExitReason.AbortCommit );

         App.CommitDocument?.Clear();

         await shutDownTask;

         ShutDown();
      }

      private async void CloseWindow( CancelEventArgs e )
      {
         if ( IsExiting )
         {
            return;
         }
         
         if ( !( await ConfirmExitForChanges() ) )
         {
            e.Cancel = true;
            return;
         }

         e.Cancel = true;

         await BeginShutDownAsync( ExitReason.AbortCommit );
         ShutDown();
      }
   }
}
