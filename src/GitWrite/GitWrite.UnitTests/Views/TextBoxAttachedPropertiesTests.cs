using System.Linq;
using System.Windows.Controls;
using System.Windows.Interactivity;
using FluentAssertions;
using Xunit;
using GitWrite.Behaviors;
using GitWrite.Views;
using Moq;

namespace GitWrite.UnitTests.Views
{
   public class TextBoxAttachedPropertiesTests
   {
      [StaFact]
      public void SetIsTripleClickEnabled_TextBoxHasNoBehaviors_TextBoxGetsATripleClickBehavior()
      {
         // Act

         var textBox = new TextBox();

         TextBoxAttachedProperties.SetIsTripleClickEnabled( textBox, true );

         // Assert

         Interaction.GetBehaviors( textBox ).Single().Should().BeOfType<TripleClickSelectionBehavior>();
      }

      [StaFact]
      public void SetIsTripleClickEnabled_ObjectIsNotATextBox_DoesNotAddBehavior()
      {
         // Act

         var button = new Button();

         TextBoxAttachedProperties.SetIsTripleClickEnabled( button, true );

         // Assert

         Interaction.GetBehaviors( button ).Should().BeEmpty();
      }

      [StaFact]
      public void SetIsTripleClickEnabled_TextBoxAlreadyHasBehavior_DoesNotAddASecondBehavior()
      {
         // Arrange

         var textBox = new TextBox();
         
         var textBoxBehaviors = Interaction.GetBehaviors( textBox );
         textBoxBehaviors.Add( new TripleClickSelectionBehavior() );

         // Act

         TextBoxAttachedProperties.SetIsTripleClickEnabled( textBox, true );

         // Assert

         Interaction.GetBehaviors( textBox ).Single().Should().BeOfType<TripleClickSelectionBehavior>();
      }

      [StaFact]
      public void SetIsTripleClickEnabled_TextBoxHasBehaviorAndIsRemoving_BehaviorIsRemoved()
      {
         // Arrange

         var textBox = new TextBox();

         var textBoxBehaviors = Interaction.GetBehaviors( textBox );
         textBoxBehaviors.Add( new TripleClickSelectionBehavior() );

         // Act

         TextBoxAttachedProperties.SetIsTripleClickEnabled( textBox, false );

         // Assert

         Interaction.GetBehaviors( textBox ).Should().BeEmpty();
      }

      [StaFact]
      public void SetIsTripleClickEnabled_TextBoxHasNoBehaviorAndIsRemoving_Behaviors()
      {
         // Arrange

         var textBox = new TextBox();

         // Act

         TextBoxAttachedProperties.SetIsTripleClickEnabled( textBox, false );

         // Assert

         Interaction.GetBehaviors( textBox ).Should().BeEmpty();
      }

      [StaFact]
      public void SetIsTripleClickEnabled_PassingTrueToATextBoxThatAlreadyHasADifferentBehavior_BehaviorIsAdded()
      {
         // Arrange

         var textBox = new TextBox();
         var textBoxBehaviors = Interaction.GetBehaviors( textBox );

         var behavior = new Mock<Behavior<TextBox>>();
         textBoxBehaviors.Add( behavior.Object );

         // Act
         
         TextBoxAttachedProperties.SetIsTripleClickEnabled( textBox, true );

         // Assert

         Interaction.GetBehaviors( textBox ).Should().ContainSingle( b => b is TripleClickSelectionBehavior );
      }
   }
}
