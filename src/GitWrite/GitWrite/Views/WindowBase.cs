using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;
using GalaSoft.MvvmLight.Ioc;
using GitWrite.Behaviors;
using GitWrite.Services;
using GitWrite.Views.Controls;

namespace GitWrite.Views
{
   public class WindowBase : Window, IViewService
   {
      public WindowBase()
      {
         SimpleIoc.Default.Register<IViewService>( () => this );

         var behaviors = Interaction.GetBehaviors( this );
         behaviors.Add( new WindowDragBehavior() );
         behaviors.Add( new WindowPlacementBehavior() );
      }

      public async Task<ConfirmationResult> ConfirmExitAsync()
      {
         throw new NotImplementedException();
      }
   }
}
