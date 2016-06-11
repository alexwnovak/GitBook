using FluentAssertions;
using GitWrite.ViewModels;
using GitWrite.Views.Controls;
using Xunit;

namespace GitWrite.UnitTests.ViewModels
{
   public class ConfirmationViewModelTests
   {
      [Fact]
      public void CloseRequested_RaisedFromSaveCommand_PassesSaveConfirmationResult()
      {
         ConfirmationResult actualResult = (ConfirmationResult) (-1);

         var confirmationViewModel = new ConfirmationViewModel();
         confirmationViewModel.CloseRequested += ( o, args ) => actualResult = args.ConfirmationResult;

         confirmationViewModel.SaveCommand.Execute( null );

         actualResult.Should().Be( ConfirmationResult.Save );
      }

      [Fact]
      public void CloseRequested_RaisedFromDiscardCommand_PassesDiscardConfirmationResult()
      {
         ConfirmationResult actualResult = (ConfirmationResult) ( -1 );

         var confirmationViewModel = new ConfirmationViewModel();
         confirmationViewModel.CloseRequested += ( o, args ) => actualResult = args.ConfirmationResult;

         confirmationViewModel.DiscardCommand.Execute( null );

         actualResult.Should().Be( ConfirmationResult.Discard );
      }

      [Fact]
      public void CloseRequested_RaisedFromCancelCommand_PassesCancelConfirmationResult()
      {
         ConfirmationResult actualResult = (ConfirmationResult) ( -1 );

         var confirmationViewModel = new ConfirmationViewModel();
         confirmationViewModel.CloseRequested += ( o, args ) => actualResult = args.ConfirmationResult;

         confirmationViewModel.CancelCommand.Execute( null );

         actualResult.Should().Be( ConfirmationResult.Cancel );
      }
   }
}
