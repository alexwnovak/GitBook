using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GitWrite.Resources;
using GitWrite.Services;
using GitWrite.Views;

namespace GitWrite.ViewModels
{
   public class CommitViewModel : ViewModelBase
   {
      public RelayCommand<KeyEventArgs> CommitNotesKeyDownCommand
      {
         get;
      }

      public RelayCommand OnPrimaryMessageGotFocusCommand
      {
         get;
      }

      public RelayCommand OnSecondaryNotesGotFocusCommand
      {
         get;
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

      private bool _isAborting;
      public bool IsAborting
      {
         get
         {
            return _isAborting;
         }
         set
         {
            Set( () => IsAborting, ref _isAborting, value );
         }
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

      private bool _hasActivatedExpandedState;
      private bool _hasEditedCommitMessage;

      public event EventHandler ExpansionRequested;
       
      public CommitViewModel()
      {
         CommitNotesKeyDownCommand = new RelayCommand<KeyEventArgs>( OnCommitNotesKeyDown );
         OnPrimaryMessageGotFocusCommand = new RelayCommand( () => ControlState = CommitControlState.EditingPrimaryMessage );
         OnSecondaryNotesGotFocusCommand = new RelayCommand( () => ControlState = CommitControlState.EditingSecondaryNotes );

         ShortMessage = App.CommitDocument?.ShortMessage;

         if ( App.CommitDocument != null && App.CommitDocument.LongMessage.Any() )
         {
            ExtraCommitText = App.CommitDocument?.LongMessage.Aggregate( ( i, j ) => $"{i} {j}" );      
         }

         _hasEditedCommitMessage = false;
      }

      protected virtual void OnExpansionRequested( object sender, EventArgs e ) => ExpansionRequested?.Invoke( sender, e );

      public void OnCommitNotesKeyDown( KeyEventArgs e )
      {
         switch ( e.Key )
         {
            case Key.Enter:
               SaveCommit();
               break;
            case Key.Escape:
               CancelCommit();
               break;
            case Key.Tab:
               ExpandUI();
               break;
         }
      }

      private async void SaveCommit()
      {
         var storyboard = SimpleIoc.Default.GetInstance<IStoryboardHelper>();
         await storyboard.PlayAsync( "SaveCommitStoryboard" );
         
         App.CommitDocument.ShortMessage = ShortMessage;
         App.CommitDocument.LongMessage.Add( ExtraCommitText );

         App.CommitDocument.Save();

         var environmentAdapter = SimpleIoc.Default.GetInstance<IAppService>();
         environmentAdapter.Shutdown();
      }

      private async void CancelCommit()
      {
         var appService = SimpleIoc.Default.GetInstance<IAppService>();
         var storyboardHelper = SimpleIoc.Default.GetInstance<IStoryboardHelper>();

         if ( _hasEditedCommitMessage )
         {
            var result = appService.DisplayMessageBox( Strings.ConfirmDiscardMessage, MessageBoxButton.YesNo );

            if ( result == MessageBoxResult.Yes )
            {
               await storyboardHelper.PlayAsync( "AbortCommitStoryboard" );
               appService.Shutdown();
            }
         }
         else
         {
            await storyboardHelper.PlayAsync( "AbortCommitStoryboard" );
            appService.Shutdown();
         }
      }

      private void ExpandUI()
      {
         if ( !_hasActivatedExpandedState )
         {
            _hasActivatedExpandedState = true;
            OnExpansionRequested( this, EventArgs.Empty );
         }
      }
   }
}
