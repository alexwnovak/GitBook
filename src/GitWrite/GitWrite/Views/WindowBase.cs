﻿using System.Windows;
using System.Windows.Interactivity;
using GitWrite.Behaviors;
using GitWrite.Views.Dwm;

namespace GitWrite.Views
{
   public class WindowBase : Window
   {
      public WindowBase()
      {
         Interaction.GetBehaviors( this ).Add( new WindowDragBehavior() );
         Interaction.GetBehaviors( this ).Add( new WindowPlacementBehavior() );

         Loaded += ( sender, e ) =>
         {
            WindowCompositionManager.EnableWindowBlur( this );
         };
      }
   }
}
